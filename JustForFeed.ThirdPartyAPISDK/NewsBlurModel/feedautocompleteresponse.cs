using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// 搜索订阅源结果
    /// </summary>
    class feedautocompleteresponse
    {
        public int num_subscribers { get; set; }
        public string tagline { get; set; }
        public string value { get; set; }
        public string label { get; set; }
        public string favicon_color { get; set; }
        public byte[] favicon { get; set; }

        public int id { get; set; }
    }
}
