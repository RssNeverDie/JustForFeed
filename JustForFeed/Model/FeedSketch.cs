using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JustForFeed.Model
{
    /// <summary>
    /// 订阅信息概述——用于订阅列表存储
    /// </summary>
    [DataContract]
    public class FeedSketch
    {
        /// <summary>
        /// 订阅链接
        /// </summary>
        [DataMember(Order = 1)]
        public string Link { get; set; }

        /// <summary>
        /// 订阅名称
        /// </summary>
        [DataMember(Order = 2)]
        public string Name  { get; set; }
    }
}
