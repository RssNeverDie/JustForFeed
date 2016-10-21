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

namespace JustForFeed.Helper
{
    public static class FeedDataHandler
    {


        /// <summary>
        /// Saves the favorites feed (the first feed of the feeds list) to local storage. 
        /// </summary>
        public static void SaveFavoritesAsync(this FeedViewModel favorites)
        {
            //var file = await ApplicationData.Current.LocalFolder
            //    .CreateFileAsync("favorites.dat", CreationCollisionOption.ReplaceExisting);
            //var file = File.Create("");
            //byte[] array = Serialize(favorites);
            //file.WriteAsync(array,0,array.Length,null);

            //await FileIO.WriteBytesAsync(file, array);

            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "favorites.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                dcs.WriteObject(fs, favorites);

                //XmlSerializer ser = new XmlSerializer(typeof(FeedViewModel));
                //ser.Serialize(fs, favorites);
            }
        }

        /// <summary>
        /// Saves the feed data (not including the Favorites feed) to local storage. 
        /// </summary>
        public static void SaveAsync(this IEnumerable<FeedViewModel> feeds)
        {
            //var file = await ApplicationData.Current.LocalFolder
            //    .CreateFileAsync("feeds.dat", CreationCollisionOption.ReplaceExisting);
            //byte[] array = Serializer.Serialize(feeds.Select(feed => new[] { feed.Name, feed.Link.ToString() }).ToArray());
            //await FileIO.WriteBytesAsync(file, array);

            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "feeds.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var q = from c in feeds select new FeedSketch { link = c.Link.ToString(), Name = c.Name };
                DataContractSerializer dcs = new DataContractSerializer(typeof(IEnumerable<FeedSketch>));
                dcs.WriteObject(fs, q);

                //XmlSerializer ser = new XmlSerializer(typeof(FeedViewModel));
                //ser.Serialize(fs, favorites);
            }
        }

        /// <summary>
        /// 离线存储订阅数据
        /// </summary>
        /// <param name="feed"></param>
        public static void OfflineFeed(this FeedViewModel feed)
        {
                        
            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "Data", feed.Name + ".xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                dcs.WriteObject(fs, feed);

                //XmlSerializer ser = new XmlSerializer(typeof(FeedViewModel));
                //ser.Serialize(fs, favorites);
            }
        }

        /// <summary>
        /// Gets the favorites feed, either from local storage, 
        /// or by initializing a new FeedViewModel instance. 
        /// </summary>
        public static FeedViewModel GetFavoritesAsync()
        {

            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "favorites.xml"), FileMode.Open))
            {

                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                return (FeedViewModel)dcs.ReadObject(fs);
                //dcs.WriteObject(fs, favorites);

            }

            //var favoritesFile = await ApplicationData.Current.LocalFolder
            //    .TryGetItemAsync("favorites.dat") as StorageFile;
            //if (favoritesFile != null)
            //{
            //    var buffer = await FileIO.ReadBufferAsync(favoritesFile);
            //    return Serializer.Deserialize<FeedViewModel>(buffer.ToArray());
            //}
            //else
            //{
            //    return new FeedViewModel
            //    {
            //        Name = "Favorites",
            //        Description = "Articles that you've starred",
            //        Symbol = Symbol.OutlineStar,
            //        Link = new Uri("http://localhost"),
            //        IsFavoritesFeed = true
            //    };
            //}
        }

        /// <summary>
        /// Gets the initial set of feeds, either from local storage or 
        /// from the app package if there is nothing in local storage.
        /// </summary>
        public static List<FeedViewModel> GetFeedsAsync()
        {
            var feeds = new List<FeedViewModel>();
            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "feeds.xml"), FileMode.Open))
            {
                // var q = from c in feeds select new FeedSketch { link = c.Link.ToString() };
                DataContractSerializer dcs = new DataContractSerializer(typeof(IEnumerable<FeedSketch>));
                // var q=    dcs.ReadObject(fs);
                FeedSketch[] feeddata = (FeedSketch[])dcs.ReadObject(fs);// = (dcs.ReadObject(fs) as Array[]).ToList<FeedSketch>();

                foreach (var item in feeddata)
                {
                    if (item.link == null)
                    {
                        continue;
                    }
                    if (item.Name != null && File.Exists(Path.Combine(RunTime.DataPath, "Data", item.Name + ".xml")))
                    {
                        using (Stream fs1 = new FileStream(Path.Combine(RunTime.DataPath, "Data", item.Name + ".xml"), FileMode.Open))
                        {
                            DataContractSerializer dcs1 = new DataContractSerializer(typeof(FeedViewModel));
                            var tempa = (FeedViewModel)dcs1.ReadObject(fs1);
                            feeds.Add(tempa);
                            continue;
                        }
                    }

                    var feedvm = new FeedViewModel { Link = new Uri(item.link) };
                    feeds.Add(feedvm);
                    var withoutAwait = feedvm.RefreshAsync();
                }
                return feeds;

                //XmlSerializer ser = new XmlSerializer(typeof(FeedViewModel));
                //ser.Serialize(fs, favorites);
            }

            //var feeds = new List<FeedViewModel>();
            //var feedFile =
            //    await ApplicationData.Current.LocalFolder.TryGetItemAsync("feeds.dat") as StorageFile ??
            //    await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/feeds.dat"));
            //if (feedFile != null)
            //{
            //    var bytes = (await FileIO.ReadBufferAsync(feedFile)).ToArray();
            //    var feedData = Serializer.Deserialize<string[][]>(bytes);
            //    foreach (var feed in feedData)
            //    {
            //        var feedVM = new FeedViewModel { Name = feed[0], Link = new Uri(feed[1]) };
            //        feeds.Add(feedVM);
            //        var withoutAwait = feedVM.RefreshAsync();
            //    }
            //}
            //return feeds;
        }


        /// <summary>
        /// Attempts to update the feed with new data from the server.
        /// </summary>
        public static async Task RefreshAsync(this FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            if (feedViewModel.Link.Host == "localhost" ||
                (feedViewModel.Link.Scheme != "http" && feedViewModel.Link.Scheme != "https")) return;

            //  feedViewModel.IsLoading = true;

            int numberOfAttempts = 5;
            bool success = false;
            do { success = await TryGetFeedAsync(feedViewModel, cancellationToken); }
            while (!success && numberOfAttempts-- > 0 &&
                (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested));

            //  feedViewModel.IsLoading = false;
        }


        /// <summary>
        /// Retrieves feed data from the server and updates the appropriate FeedViewModel properties.
        /// </summary>
        private static async Task<bool> TryGetFeedAsync(FeedViewModel feedViewModel, CancellationToken? cancellationToken = null)
        {
            try
            {
                //if (string.IsNullOrEmpty(feedViewModel.Link))
                //{
                //    return false;
                //}

                XmlReader r = XmlReader.Create(await GetRSSStreamInfo(feedViewModel.Link));

                SyndicationFeed feed = SyndicationFeed.Load(r);

                //var feed = await new SyndicationClient().RetrieveFeedAsync(feedViewModel.Link);

                if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested) return false;

                // feedViewModel.LastSyncDateTime = DateTime.Now;
                feedViewModel.Name = String.IsNullOrEmpty(feedViewModel.Name) ? feed.Title.Text : feedViewModel.Name;
                //feedViewModel.Description = feed.Subtitle?.Text ?? feed.Title.Text;

                feed.Items.Select(item => new ArticleViewModel
                {
                    Title = item.Title.Text,
                    Summary = item.Summary == null ? string.Empty :
                        item.Summary.Text//.RegexRemove("\\&.{0,4}\\;").RegexRemove("<.*?>"),
                    //Author = item.Authors.Select(a => a.NodeValue).FirstOrDefault(),
                    //Link = item.ItemUri ?? item.Links.Select(l => l.Uri).FirstOrDefault(),
                    //PublishedDate = item.PublishedDate
                })
                .ToList().ForEach(article =>
                {
                    feedViewModel.Articles.Add(article);
                    //var favorites = AppShell.Current.ViewModel.FavoritesFeed;
                    //var existingCopy = favorites.Articles.FirstOrDefault(a => a.Equals(article));
                    //article = existingCopy ?? article;
                    //if (!feedViewModel.Articles.Contains(article)) feedViewModel.Articles.Add(article);
                });
                //feedViewModel.IsInError = false;
                //feedViewModel.ErrorMessage = null;
                return true;
            }
            catch (Exception)
            {
                if (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested)
                {
                    //feedViewModel.IsInError = true;
                    //feedViewModel.ErrorMessage = feedViewModel.Articles.Count == 0 ? BAD_URL_MESSAGE : NO_REFRESH_MESSAGE;
                }
                return false;
            }
        }

        static HttpClient a = new HttpClient();
        /// <summary>
        /// 获取 rss内容——返回数据流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Stream> GetRSSStreamInfo(Uri url)
        {
            //经测试——部分网站要求一定要有useragent才有返回数据——如博客园的rss
            a.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            return await a.GetStreamAsync(url);
        }

        /// <summary>
        /// Serializes the specified object as a byte array.
        /// </summary>
        public static byte[] Serialize<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(stream, obj);
            //DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            //dcs.WriteObject(stream, obj);
            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes the specified byte array as an instance of type T. 
        /// </summary>
        public static T Deserialize<T>(byte[] buffer)
        {
            MemoryStream stream = new MemoryStream(buffer);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(stream);
            //DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            //return (T)dcs.ReadObject(stream);
        }
    }
}
