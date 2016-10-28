using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.Core
{
    /// <summary>
    /// 订阅源基本信息
    /// </summary>
    public class FeedInfoBase
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订阅源地址
        /// </summary>
        public string FeedUrl { get; set; }

        /// <summary>
        /// 订阅源名称
        /// </summary>
        public string FeedName { get; set; }
    }
}
