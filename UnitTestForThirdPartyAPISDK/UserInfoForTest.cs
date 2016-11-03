using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForThirdPartyAPISDK
{
    /// <summary>
    /// 用于写测试信息
    /// </summary>
    class UserInfoForTest
    {
        /// <summary>
        /// 用于传递用户名
        /// </summary>
        public static string username = "yourname";
        /// <summary>
        /// 用于传递密码
        /// </summary>
        public static string password = "yourpwd";

        /// <summary>
        /// 从配置文件加载用户信息
        /// </summary>
        /// <returns></returns>
        public static bool GetUserInfo()
        {
            LoginNewsBlur login = new LoginNewsBlur();
            return System.Windows.Forms.DialogResult.OK == login.ShowDialog(null);
        }
    }

}
