using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using JustForFeed.ViewModel;
using JustForFeed.Model;
using System.Threading;
using System.Xml;
using System.Net.Http;
using System.ServiceModel.Syndication;
using Microsoft.Practices.ServiceLocation;

namespace JustForFeed.Helper
{
    /// <summary>
    /// 订阅数据处理
    /// </summary>
    public static class FeedDataHandler
    {
        static HttpClient client = new HttpClient();

        static FeedDataHandler()
        {
            //经测试——部分网站要求一定要有useragent才有返回数据——如博客园的rss
            //useragent格式：“产品名称/产品版本（系统信息等）”
            client.DefaultRequestHeaders.UserAgent.ParseAdd("JustForFeed/1.0");
        }

        /// <summary>
        /// 收藏文章保存到本地
        /// </summary>
        public static async Task SaveFavoritesAsync(this FeedViewModel favorites)
        {
            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "favorites.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                await Task.Run(() => dcs.WriteObject(fs, favorites));
            }
        }

        /// <summary>
        /// 保存订阅数据列表
        /// </summary>
        public static async Task SaveAsync(this IEnumerable<FeedViewModel> feeds)
        {
            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "feeds.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var q = from c in feeds select new FeedSketch { Link = c.Link.ToString(), Name = c.Name };
                DataContractSerializer dcs = new DataContractSerializer(typeof(IEnumerable<FeedSketch>));
                await Task.Run(() => dcs.WriteObject(fs, q));
            }
        }

        /// <summary>
        /// 离线存储订阅数据
        /// </summary>
        /// <param name="feed"></param>
        public static async Task OfflineFeed(this FeedViewModel feed)
        {
            using (Stream fs = new FileStream(Path.Combine(RunTime.OfflineDetailPath, feed.Name + ".xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                await Task.Run(() => dcs.WriteObject(fs, feed));
            }
        }

        /// <summary>
        /// 获取本地已收藏文章——不存在则初始化一个FeedViewModel
        /// </summary>
        public static async Task<FeedViewModel> GetFavoritesAsync()
        {
            if (!File.Exists(Path.Combine(RunTime.DataPath, "favorites.xml")))
            {
                return new FeedViewModel();
            }
            try
            {
                using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "favorites.xml"), FileMode.OpenOrCreate))
                {
                    DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                    return await Task.Run(() => (FeedViewModel)dcs.ReadObject(fs));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取已订阅的rss源——并从网络获取源数据
        /// </summary>
        public static List<FeedViewModel> GetFeedsAsync()
        {
            var feeds = new List<FeedViewModel>();
            if (!File.Exists(Path.Combine(RunTime.DataPath, "feeds.xml")))
            {
                return new List<FeedViewModel>();
            }
            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "feeds.xml"), FileMode.Open))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(IEnumerable<FeedSketch>));
                FeedSketch[] feeddata = (FeedSketch[])dcs.ReadObject(fs);

                foreach (var item in feeddata)
                {
                    if (item.Link == null)
                    {
                        continue;
                    }
                    if (item.Name != null && File.Exists(Path.Combine(RunTime.OfflineDetailPath, item.Name + ".xml")))
                    {
                        using (Stream fs1 = new FileStream(Path.Combine(RunTime.OfflineDetailPath, item.Name + ".xml"), FileMode.Open))
                        {
                            DataContractSerializer dcs1 = new DataContractSerializer(typeof(FeedViewModel));
                            var tempa = (FeedViewModel)dcs1.ReadObject(fs1);
                            feeds.Add(tempa);
                            continue;
                        }
                    }

                    var feedvm = new FeedViewModel { Name = item.Name, Link = new Uri(item.Link) };
                    feeds.Add(feedvm);
                    var withoutAwait = feedvm.RefreshAsync();
                }
                return feeds;

            }
        }

        /// <summary>
        /// 刷新订阅数据——从服务器获取订阅数据
        /// </summary>
        public static async Task RefreshAsync(this FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            if (feedViewModel.Link.Host == "localhost" || (feedViewModel.Link.Scheme != "http" && feedViewModel.Link.Scheme != "https"))
                return;

            int numberOfAttempts = 5;
            bool success = false;
            do
            {
                success = await TryGetFeedAsync(feedViewModel, cancellationToken);
            }
            while (!success && numberOfAttempts-- > 0 && (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested));
        }

        /// <summary>
        /// 从服务器获取订阅数据更新订阅对象
        /// </summary>
        private static async Task<bool> TryGetFeedAsync(FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            try
            {
                if (string.IsNullOrEmpty(feedViewModel.LinkString))
                {
                    return false;
                }

                XmlReader r = XmlReader.Create(await GetRSSStreamInfo(feedViewModel.Link));
                SyndicationFeed feed = SyndicationFeed.Load(r);

                if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                    return false;

                // feedViewModel.LastSyncDateTime = DateTime.Now;
                feedViewModel.Name = String.IsNullOrEmpty(feedViewModel.Name) ? feed.Title.Text : feedViewModel.Name;
                feedViewModel.Description = feed.Description?.Text ?? feed.Title.Text;
                var qq = feedViewModel.Articles == null || feedViewModel.Articles.Count <= 0
                    ? new DateTimeOffset(new DateTime(1900, 1, 1))
                    : feedViewModel.Articles.Max(p => p.PublishedDate);

                feed.Items.Select(item => new ArticleViewModel
                {
                    Title = item.Title.Text,
                    Summary = item.Summary == null ? string.Empty : item.Summary.Text,
                    Author = item.Authors.Count > 0 ? item.Authors.Select(a => a.Name).FirstOrDefault() : feed.Title.Text,
                    Link = item.BaseUri ?? item.Links.Select(l => l.Uri).FirstOrDefault(),
                    PublishedDate = item.PublishDate
                })
                .ToList().ForEach(article =>
                {
                    var favorites = ServiceLocator.Current.GetInstance<MainViewModel>().FavoritesFeed;
                    var existingCopy = favorites.Articles.FirstOrDefault(a => a.Equals(article));
                    article = existingCopy ?? article;
                    if (!feedViewModel.Articles.Contains(article) && article.PublishedDate > qq)
                        feedViewModel.Articles.Add(article);
                });
                return true;
            }
            catch (Exception)
            {
                if (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested)
                {
                }
                return false;
            }
        }

        /// <summary>
        /// 获取 rss内容——返回数据流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Stream> GetRSSStreamInfo(Uri url)
        {
            return await client.GetStreamAsync(url);
        }

        /// <summary>
        /// 获取 rss内容——返回数据流
        /// ——由于部分外部调用用异步方法时直接卡死，故添加此同步方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetRSSStreamInfo1(Uri url)
        {
            return client.GetStreamAsync(url).Result;
        }

    }
}
