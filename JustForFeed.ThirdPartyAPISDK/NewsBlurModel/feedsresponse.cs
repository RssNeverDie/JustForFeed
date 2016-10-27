using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    class feedsresponse
    {
      /// <summary>
      /// 目录树形结构-故使用dynamic类型
      /// </summary>
        public dynamic folders { get; set; }

        /// <summary>
        /// 参数flat为true时，才有此属性
        /// </summary>
        public Dictionary<string,long[]> flat_folders { get; set; }
    }
}
