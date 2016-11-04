using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// 刷新未读计数返回对象
    /// </summary>
    class refreshfeedsresponse
    {
        public bool authenticated { get; set; }
        public int interactions_count { get; set; }
        public string result { get; set; }
        public long user_id { get; set; }

        public Dictionary<string,refreshfeedsdetail> feeds { get; set; }

        public dynamic social_feeds { get; set; }
    }

    class refreshfeedsdetail
    {
        /// <summary>
        /// 未读计数
        /// </summary>
        public  int nt { get; set; }

        public int ps { get; set; }
        public Uri feed_address { get; set; }
        public int ng { get; set; }
        public string exception_type { get; set; }
        public int exception_code { get; set; }

        public string id { get; set; }

        public bool has_exception { get; set; }
    }
}
