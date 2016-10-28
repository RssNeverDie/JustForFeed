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
        [TestMethod]
        public void TestMethod1()
        {
            NewsBlurAPI.Login(UserInfoForTest.username, UserInfoForTest.password).Wait();
            var withoutwait = NewsBlurAPI.GetUserFeedsList(include_favicons: false);
            foreach (var item in withoutwait.Result)
            {
                Console.WriteLine(item.Id + ";" + item.FeedName + ";" + item.FeedUrl);
            }
        }
    }
}
