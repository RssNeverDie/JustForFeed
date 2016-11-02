using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.ThirdPartyAPISDK;

namespace UnitTestForThirdPartyAPISDK
{
    /// <summary>
    /// 关于UserInfoForTest类——设计个人信息，不纳入版本管理，需手动建立
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            var loginresult = NewsBlurAPI.Login(UserInfoForTest.username, UserInfoForTest.password).Result;
            Assert.IsTrue(loginresult.IsAuthSuccess);
        }

        [TestMethod]
        public void TestMethod1()
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
    }
}
