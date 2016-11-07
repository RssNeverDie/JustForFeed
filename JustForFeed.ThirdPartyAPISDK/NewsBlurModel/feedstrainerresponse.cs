using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// GET /reader/feeds_trainer
    /// 获取订阅的信息——分类标签（自己的+常用的），作者（名称+计数）等。
    /// </summary>
    class feedstrainerresponse
    {
        /// <summary>
        /// 标签+计数
        /// </summary>
        public object[][] feed_tags { get; set; }

        public int num_subscribers { get; set; }

        public string feed_id { get; set; }

        public int stories_last_month { get; set; }

        /// <summary>
        /// 作者名+计数
        /// </summary>
        public object[][] feed_authors { get; set; }

        public feedstrainerclassifier classifiers { get; set; }

    }

    class feedstrainerclassifier
    {
        public dynamic authors { get; set; }
        public dynamic feeds { get; set; }
        public dynamic titles { get; set; }
        public dynamic tags { get; set; }
    }

}
