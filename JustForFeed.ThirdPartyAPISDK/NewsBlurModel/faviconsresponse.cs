using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// GET /reader/favicons
    /// 具体图标格式：订阅id:图标byte
    /// </summary>
    class faviconsresponse : Dictionary<string, object>
    {

        public bool authenticated
        {
            get
            {
                return (bool)base["authenticated"];
            }
            set
            {
                if (this.ContainsKey("authenticated"))
                    this["authenticated"] = value;
                else
                {
                    this.Add("authenticated", value);
                }
            }
        }

        public long user_id
        {
            get
            {
                return (long)base["user_id"];
            }
            set
            {
                if (this.ContainsKey("user_id"))
                    this["user_id"] = value;
                else
                {
                    this.Add("user_id", value);
                }
            }
        }

        public string result
        {
            get
            {
                return (string)base["result"];
            }
            set
            {
                if (this.ContainsKey("result"))
                    this["result"] = value;
                else
                {
                    this.Add("result", value);
                }
            }
        }

        /// <summary>
        /// 获取相应订阅源的图表id
        /// </summary>
        /// <param name="feedid"></param>
        /// <returns></returns>
        public byte[] GetIcon(string feedid)
        {
            try
            {
                if (this.ContainsKey(feedid))
                {
                    if (this[feedid] != null)
                    {
                        return System.Text.Encoding.UTF8.GetBytes(this[feedid].ToString());
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


    }
}
