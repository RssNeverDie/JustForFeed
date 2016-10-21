using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.ViewModel;
using JustForFeed.Helper;
using System.Collections.Generic;
using System.Threading;

namespace UnitTestForApplication
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FeedViewModel feed = new FeedViewModel();
            feed.Link = new Uri("http://www.baidu.com");
            feed.Name = "baidu";

            feed.SaveFavoritesAsync();
        }

        [TestMethod]
        public void TestMethod4()
        {

            FeedViewModel feed = FeedDataHandler.GetFavoritesAsync();

            Console.WriteLine(feed.Link.ToString());


        }

        [TestMethod]
        public void TestMethod2()
        {
            FeedViewModel feed = new FeedViewModel();
            feed.Link = new Uri("http://www.shisujie.com/rss?containerid=31");
            feed.Name = "奇葩史";
            feed.RefreshAsync().Wait();

            List<FeedViewModel> feeds = new List<FeedViewModel>();
            feeds.Add(feed);
            feeds.SaveAsync();

            feed.OfflineFeed();

        }

        [TestMethod]
        public void TestMethod3()
        {

            List<FeedViewModel> feeds = FeedDataHandler.GetFeedsAsync();
            foreach (var item in feeds)
            {
                Console.WriteLine(item.Link.ToString());
            }
            Thread.Sleep(5000);
            foreach (var item in feeds)
            {
                foreach (var subitem in item.Articles)
                {
                    Console.WriteLine(subitem.Title);
                    //Console.WriteLine("\t" + subitem.Summary);
                }
            }
        }
    }
}
