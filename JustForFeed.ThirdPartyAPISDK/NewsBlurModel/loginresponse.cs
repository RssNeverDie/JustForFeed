using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    class loginresponse
    {
        /// <summary>
        /// 失败返回-1，成功返回1
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 已经认证返回true，未认证返回false——即是不是已经登录的状态
        /// </summary>
        public bool authenticated { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public errorresponse errors { get; set; }

        /// <summary>
        /// 是否执行成功——与用户密码验证是否正确无关
        /// </summary>
        public string result { get; set; }
    }
}
