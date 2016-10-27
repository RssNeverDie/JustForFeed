using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.Core
{
    /// <summary>
    /// 用户认证信息基类
    /// </summary>
    public class AuthenticationBase
    {
        /// <summary>
        /// 是否认证成功
        /// </summary>
        public Boolean IsAuthSuccess { get; set; } = false;

        /// <summary>
        /// 认证返回信息——错误信息或认证成功信息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
