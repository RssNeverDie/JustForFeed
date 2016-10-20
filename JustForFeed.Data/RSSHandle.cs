using JustForFeed.Data.FeedsData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JustForFeed.Data
{
    /// <summary>
    /// Rss处理工具类
    /// </summary>
    public class RSSHandle
    {

        static HttpClient a = new HttpClient();

        /// <summary>
        /// 获取订阅信息
        /// </summary>
        /// <param name="feedsinfopath">文件目录路径</param>
        /// <returns>订阅信息</returns>
        public static List<FeedInfo> ReadFromXml(string feedsinfopath)
        {
            using (Stream fs = new FileStream(Path.Combine(feedsinfopath, "feeds.xml"), FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Feeds));
                return (ser.Deserialize(fs) as Feeds).Items;
            }
        }

        /// <summary>
        /// 保存订阅信息
        /// </summary>
        /// <param name="feedsinfopath">文件目录</param>
        /// <param name="feeds">订阅信息</param>
        public static void SaveFeedToXml(string feedsinfopath, List<FeedInfo> feeds)
        {
            using (Stream fs = new FileStream(Path.Combine(feedsinfopath, "feeds.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {

                Feeds f = new Feeds();
                f.Items = feeds;

                XmlSerializer ser = new XmlSerializer(typeof(Feeds));
                ser.Serialize(fs, f);
            }
        }

        public static void RefreshFeedsSort(List<FeedInfo> feeds)
        {
            int sort = 0;
            foreach (var item in feeds)
            {
                item.Sort = sort;
                sort++;
            }
        }

        /// <summary>
        /// 获取阅读情况
        /// </summary>
        /// <param name="feedsinfopath">订阅信息目录</param>
        /// <param name="feedinfo">订阅信息</param>
        /// <returns>阅读情况</returns>
        public static List<ReadInfo> GetReadInfo(string feedsinfopath, FeedInfo feedinfo)
        {
            string readingfolderpath = Path.Combine(feedsinfopath, feedinfo.DataRelativePath);
            if (!Directory.Exists(readingfolderpath))
            {
                return new List<ReadInfo>();
            }
            using (Stream fs = new FileStream(Path.Combine(feedsinfopath, feedinfo.DataRelativePath, "reading.xml"), FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Reads));
                return (ser.Deserialize(fs) as Reads).Items;
            }
        }

        /// <summary>
        /// 获取某一订阅源阅读情况
        /// </summary>
        /// <param name="feedsinfopath">订阅信息目录</param>
        /// <param name="feedinfo">某一订阅信息</param>
        /// <param name="reading">阅读情况</param>
        public static void SaveReadsInfo(string feedsinfopath, FeedInfo feedinfo, List<ReadInfo> reading)
        {
            if (string.IsNullOrEmpty(feedinfo.DataRelativePath))
            {
                feedinfo.DataRelativePath = feedinfo.Name;
            }
            string readingfolderpath = Path.Combine(feedsinfopath, feedinfo.DataRelativePath);
            if (!Directory.Exists(readingfolderpath))
            {
                Directory.CreateDirectory(readingfolderpath);
            }
            using (Stream fs = new FileStream(Path.Combine(readingfolderpath, "reading.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Reads r = new FeedsData.Reads();
                r.Items = reading;
                XmlSerializer ser = new XmlSerializer(typeof(Reads));
                ser.Serialize(fs, r);
            }
        }

        /// <summary>
        /// 更新订阅信息
        /// </summary>
        /// <param name="feedsinfopath">数据存储路径</param>
        /// <param name="feedinfo">订阅信息</param>
        public static void UpdateRssInfo(string feedsinfopath, FeedInfo feedinfo)
        {
            if (string.IsNullOrEmpty(feedinfo.DataRelativePath))
            {
                feedinfo.DataRelativePath = feedinfo.Name;
            }
            string readingfolderpath = Path.Combine(feedsinfopath, feedinfo.DataRelativePath);
            if (!Directory.Exists(readingfolderpath))
            {
                Directory.CreateDirectory(readingfolderpath);
            }

            if (string.IsNullOrEmpty(feedinfo.FeedUrl))
            {
                throw new Exception("未配置订阅源地址！");
            }
            WebClient w = new WebClient();
            w.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            w.DownloadFile(feedinfo.FeedUrl, Path.Combine(readingfolderpath, "rss.xml"));
        }

        /// <summary>
        /// 获取 rss内容——返回字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetRSSStringInfo(string url)
        {
            //经测试——部分网站要求一定要有useragent才有返回数据——如博客园的rss
            a.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            return a.GetStringAsync(url).Result;
        }

        /// <summary>
        /// 获取 rss内容——返回数据流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetRSSStreamInfo(string url)
        {
            //经测试——部分网站要求一定要有useragent才有返回数据——如博客园的rss
            a.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            return a.GetStreamAsync(url).Result;
        }

    }
}
