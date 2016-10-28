using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    class feedinforesponse
    {
        public int subs { get; set; }

        /// <summary>
        /// 图片文件——当请求需要包含订阅源图标时，才有此字段
        /// </summary>
        public byte[] favicon { get; set; }

        /// <summary>
        /// 图标文件地址_有时为一个相对路径-此处不用Uri
        /// </summary>
        public string favicon_url { get; set; }

        public bool is_push { get; set; }

        public int feed_opens { get; set; }

        public long id { get; set; }

        public bool s3_icon { get; set; }

        /// <summary>
        /// 有时返回uuid-故此处不适用Uri
        /// </summary>
        public string feed_link { get; set; }

        public long updated_seconds_ago { get; set; }

        public bool favicon_fetching { get; set; }

        public int ng { get; set; }

        public string favicon_border { get; set; }

        /// <summary>
        /// 非local时间，用时需手动转local
        /// </summary>
        public DateTime last_story_date { get; set; }

        public int nt { get; set; }

        public bool not_yet_fetched { get; set; }

        public string updated { get; set; }

        public int average_stories_per_month { get; set; }

        public int ps { get; set; }

        public Uri feed_address { get; set; }

        public string feed_title { get; set; }

        public string favicon_fade { get; set; }

        public bool is_newsletter { get; set; }

        public long last_story_seconds_ago { get; set; }

        public string favicon_color { get; set; }

        public int stories_last_month { get; set; }

        public bool active { get; set; }

        public bool fetched_once { get; set; }

        public string favicon_text_color { get; set; }

        public bool subscribed { get; set; }
        public int num_subscribers { get; set; }

        public bool s3_page { get; set; }

        public int min_to_decay { get; set; }

        public dynamic search_indexed { get; set; }

        public bool disabled_page { get; set; }

    }
}
