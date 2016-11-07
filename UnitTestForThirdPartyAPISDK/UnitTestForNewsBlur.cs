using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.ThirdPartyAPISDK;

namespace UnitTestForThirdPartyAPISDK
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UnitTestForNewsBlur
    {
        [TestInitialize]
        public void Init()
        {
            if (!UserInfoForTest.GetUserInfo())
                return;
            var loginresult = NewsBlurAPI.Login(UserInfoForTest.username, UserInfoForTest.password).Result;
            Assert.IsTrue(loginresult.IsAuthSuccess);
        }

        [TestMethod]
        public void TestGetFeeds()
        {
            var withoutwait = NewsBlurAPI.GetUserFeedsList(include_favicons: false);
            foreach (var item in withoutwait.Result)
            {
                Console.WriteLine(item.Id + ";" + item.FeedName + ";" + item.FeedUrl);
            }
        }

        [TestMethod]
        public void TestGetIcon()
        {
            var withourwait = NewsBlurAPI.GetFeedIcon("6461499", "6464950");
            foreach (var item in withourwait.Result)
            {
                Console.WriteLine(item.Key + ":" + System.Text.Encoding.UTF8.GetString(item.Value ?? (new byte[0])));
            }

        }

        [TestMethod]
        public void TestGetOriginalPage()
        {
            var withoutwair = NewsBlurAPI.GetOriginalPage("6461499");
            Console.WriteLine(withoutwair.Result);
        }

        /// <summary>
        /// 刷新未读计数
        /// </summary>
        [TestMethod]
        public void TestRefreshFeeds()
        {
            var withoutwait = NewsBlurAPI.GetRefreshFeeds();
            foreach (var item in withoutwait.Result)
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }
        }

        [TestMethod]
        public void TestGetFeedsTrainer()
        {
            var withoutwait = NewsBlurAPI.GetFeedsTrainer();
            foreach (var item in withoutwait.Result)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void TestGetStatistics()
        {
            NewsBlurAPI.GetStatisticsInfo("6461499").Wait();
        }

        [TestMethod]
        public void TestSearchFeeds()
        {
            var withoutwait = NewsBlurAPI.SearchFeeds("tech");
            foreach (var item in withoutwait.Result)
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }
        }
    }
}
