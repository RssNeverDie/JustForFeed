using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JustForFeed.Data
{
    /// <summary>
    /// 订阅信息存储
    /// </summary>
    [Serializable]
    public class FeedInfo
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 订阅名称——必填项
        /// </summary>
        [XmlElement(IsNullable = false)]
        public String Name { get; set; } = Guid.NewGuid().ToString("P").ToUpper();
        /// <summary>
        /// 订阅地址
        /// </summary>
        public string FeedUrl { get; set; }
        /// <summary>
        /// 存储相对路径
        /// </summary>
        public string DataRelativePath { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [XmlIgnore]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最后更新时间——仅用于序列化
        /// </summary>
        //[XmlAttribute("DateTime")]
        [XmlElement("LastUpdateTime")]
        public string LastUpdateTimeForSerializer
        {
            get
            {
                return LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                DateTime dttemp;
                if (DateTime.TryParse(value, out dttemp)) this.LastUpdateTime = dttemp;
            }
        }
    }

    /// <summary>
    /// 订阅源列表信息
    /// </summary>
    [XmlRoot("Feeds")]
    public class Feeds
    {
        
        public List<FeedInfo> Items { get; set; }
    }
}
