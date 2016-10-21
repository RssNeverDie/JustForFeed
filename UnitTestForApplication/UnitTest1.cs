using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.ViewModel;
using JustForFeed.Helper;

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
    }
}
