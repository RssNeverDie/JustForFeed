using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace JustForFeed.ThirdPartyAPISDK
{
    public class NewsBlurAPI
    {
        static string host = "http://www.newsblur.com";

        static HttpClient client = new HttpClient();

        static NewsBlurAPI()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("JustForFeed/1.0");
        }

        /// <summary>
        /// 登录
        /// POST /api/login
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        public static void Login(string username, string pwd = null)
        {
            Dictionary<string, string> dic_content = new Dictionary<string, string>();
            dic_content.Add("username", username);
            if (!string.IsNullOrEmpty(pwd))
            {
                dic_content.Add("password", pwd);
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(dic_content);
            var q = client.PostAsync(host + "/api/login", content).Result;
            
        }
    }
}
