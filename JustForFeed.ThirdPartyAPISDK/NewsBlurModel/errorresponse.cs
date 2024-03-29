﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    class errorresponse
    {
        /// <summary>
        /// 请求方式错误
        /// </summary>
        public string method { get; set; } = string.Empty;

        /// <summary>
        /// 用户名相关错误信息列表 
        /// </summary>
        public string[] username { get; set; }

        /// <summary>
        /// 错误信息列表——先包括密码错误、用户名未注册等
        /// </summary>
        public string[] __all__ { get; set; }

        public override string ToString()
        {
            string msg = string.Empty;
            msg += method;
            msg += "\r\n";
            foreach (var item in username)
            {
                msg += item;
                msg += "\r\n";
            }

            foreach (var item in __all__)
            {
                msg += item;
                msg += "\r\n";
            }
            return msg;

            //return base.ToString();
        }
    }
}
