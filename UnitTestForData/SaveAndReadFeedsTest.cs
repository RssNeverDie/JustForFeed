using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustForFeed.Data;
using System.Collections.Generic;
using System.IO;
using JustForFeed.Data.FeedsData;

namespace UnitTestForData
{
    [TestClass]
    public class SaveAndReadFeedsTest
    {
        [TestMethod]
        public void SaveFeedsDataTest()
        {
            FeedInfo info = new FeedInfo();
            info.Sort = 1;
            List<FeedInfo> list = new List<FeedInfo>();
            list.Add(info);

            RSSHandle.SaveFeedToXml(AppDomain.CurrentDomain.BaseDirectory, list);
        }

        [TestMethod]
        public void ReadFeedsFromFile()
        {
            List<FeedInfo> list = new List<FeedInfo>();
            list = RSSHandle.ReadFromXml(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var item in list)
            {
                Console.WriteLine(item.Sort + "——" + item.LastUpdateTime);
            }
        }

        [TestMethod]
        public void SavsReadingInfo()
        {
            FeedInfo info = new FeedInfo();
            info.Sort = 1;
            info.FeedUrl = "http://www.shisujie.com/rss?containerid=31";

            ReadInfo reading = new ReadInfo();
            reading.Id = "http://www.baidu.com";
            List<ReadInfo> readinglist = new List<ReadInfo>();
            readinglist.Add(reading);
            RSSHandle.SaveReadsInfo(AppDomain.CurrentDomain.BaseDirectory, info, readinglist);

            List<FeedInfo> list = new List<FeedInfo>();
            list.Add(info);

            RSSHandle.SaveFeedToXml(AppDomain.CurrentDomain.BaseDirectory, list);
        }

        [TestMethod]
        public void GetReadingInfoFromFile()
        {
            List<FeedInfo> list = new List<FeedInfo>();
            list = RSSHandle.ReadFromXml(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var item in list)
            {
                Console.WriteLine(item.Sort + "——" + item.DataRelativePath + "——" + item.LastUpdateTime);
                List<ReadInfo> readinglist = RSSHandle.GetReadInfo(AppDomain.CurrentDomain.BaseDirectory, item);
                foreach (var subitem in readinglist)
                {
                    Console.WriteLine("\t" + subitem.Id);
                }
            }
        }

        [TestMethod]
        public void UpdateRss()
        {
            List<FeedInfo> list = new List<FeedInfo>();
            list = RSSHandle.ReadFromXml(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var item in list)
            {
                RSSHandle.UpdateRssInfo(AppDomain.CurrentDomain.BaseDirectory, item);
            }
        }
    }
}
