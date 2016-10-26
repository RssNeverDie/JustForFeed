using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.ThirdPartyAPISDK;

namespace UnitTestForThirdPartyAPISDK
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            NewsBlurAPI.Login("","");
        }
    }
}
