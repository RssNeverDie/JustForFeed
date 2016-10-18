using JustForFeed.Data.FeedsData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        /// <summary>
        /// 获取订阅信息
        /// </summary>
        /// <param name="feedsinfopath">文件目录路径</param>
        /// <returns>订阅信息</returns>
        public static List<FeedInfo> ReadFromXml(string feedsinfopath)
        {
            Stream fs = new FileStream(Path.Combine(feedsinfopath, "feeds.xml"), FileMode.Open);
            XmlSerializer ser = new XmlSerializer(typeof(List<FeedInfo>));
            return ser.Deserialize(fs) as List<FeedInfo>;
        }

        /// <summary>
        /// 保存订阅信息
        /// </summary>
        /// <param name="feedsinfopath">文件目录</param>
        /// <param name="feeds">订阅信息</param>
        public static void SaveFeedToXml(string feedsinfopath, List<FeedInfo> feeds)
        {
            Stream fs = new FileStream(Path.Combine(feedsinfopath, "feeds.xml"), FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer ser = new XmlSerializer(typeof(List<FeedInfo>));
            ser.Serialize(fs, feeds);
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
            Stream fs = new FileStream(Path.Combine(feedsinfopath, feedinfo.DataRelativePath, "reading.xml"), FileMode.Open);
            XmlSerializer ser = new XmlSerializer(typeof(List<ReadInfo>));
            return ser.Deserialize(fs) as List<ReadInfo>;
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
            Stream fs = new FileStream(Path.Combine(readingfolderpath, "reading.xml"), FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer ser = new XmlSerializer(typeof(List<ReadInfo>));
            ser.Serialize(fs, reading);
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
            w.DownloadFile(feedinfo.FeedUrl, Path.Combine(readingfolderpath, "rss.xml"));
        }

    }
}
