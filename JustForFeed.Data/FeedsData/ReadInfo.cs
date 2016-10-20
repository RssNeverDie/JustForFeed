using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JustForFeed.Data.FeedsData
{
    /// <summary>
    /// 订阅信息阅读情况
    /// </summary>
    public class ReadInfo
    {
        /// <summary>
        /// 标识——即订阅信息中的网址链接
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否已读——默认未读
        /// </summary>
        public bool HasRead { get; set; } = false;

        /// <summary>
        /// 是否已删除——默认未删除
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 本地存储路径——为空或找不到文件夹路径即为未下载，需联网查看或只有部分文字内容
        /// </summary>
        [XmlElement("LocalData")]
        public string LocalDataRelativePath { get; set; }
    }

    [XmlRoot("Reads")]
    public class Reads
    {
        public List<ReadInfo> Items { get; set; }
    }
}
