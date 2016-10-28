using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// feed订阅信息返回
    /// TODO 注：类中未特别注释的dynamic类型均为待建立新类的类型
    /// </summary>
    class feedsresponse
    {
        /// <summary>
        /// 目录树形结构-故使用dynamic类型
        /// </summary>
        public dynamic folders { get; set; }

        /// <summary>
        /// 参数flat为true时，才有此属性
        /// </summary>
        public Dictionary<string, long[]> flat_folders { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// TODO 需定义新类
        /// </summary>
        public dynamic social_profile { get; set; }

        /// <summary>
        /// TODO 需定义新类
        /// </summary>
        public dynamic user_profile { get; set; }

        public dynamic starred_counts { get; set; }

        public int starred_count { get; set; }

        public bool is_staff { get; set; }

        public string result { get; set; }

        public bool authenticated { get; set; }

        /// <summary>
        /// 订阅源信息列表
        /// </summary>
        public Dictionary<string,feedinforesponse> feeds { get; set; }

        public dynamic social_services { get; set; }

        public dynamic categories { get; set; }

        public dynamic social_feeds { get; set; }

    }
}
