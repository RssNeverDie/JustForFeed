using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using JustForFeed.ThirdPartyAPISDK.NewsBlurModel;
using System.Threading.Tasks;
using JustForFeed.ThirdPartyAPISDK.Core;

namespace JustForFeed.ThirdPartyAPISDK
{
    public class NewsBlurAPI
    {
        static string host = "http://www.newsblur.com";

        static HttpClient client = new HttpClient();

        static NewsBlurAPI()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd(AboutJustForFeed.useragent);
        }

        /// <summary>
        /// 登录
        /// POST /api/login
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        public static async Task<AuthenticationBase> Login(string username, string pwd = null)
        {
            try
            {
                Dictionary<string, string> dic_content = new Dictionary<string, string>();
                dic_content.Add("username", username);
                if (!string.IsNullOrEmpty(pwd))
                {
                    dic_content.Add("password", pwd);
                }
                FormUrlEncodedContent content = new FormUrlEncodedContent(dic_content);
                var response = await client.PostAsync(host + "/api/login", content);

                var stringresponse = await response.Content.ReadAsStringAsync();

                var loginobj = JsonConvert.DeserializeObject<loginresponse>(stringresponse);

                var auth = new AuthenticationBase { IsAuthSuccess = loginobj.authenticated, Message = loginobj.errors.ToString() };
                return auth;
            }
            catch (Exception ex)
            {
                var auth = new AuthenticationBase { IsAuthSuccess = false, Message = ex.Message };
                return auth;
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public static async Task Logout()
        {
            await client.PostAsync(host + "/api/logout", new StringContent(""));
        }


        /// <summary>
        /// 获取用户订阅信息
        /// </summary>
        /// <param name="include_favicons"></param>
        /// <param name="flat"></param>
        /// <param name="update_counts"></param>
        public static async Task<List<FeedInfoBase>> GetUserFeedsList(bool include_favicons = false, bool flat = false, bool update_counts = false)
        {
            string getparam = string.Empty;
            getparam += (include_favicons ? "?include_favicons=true" : "");
            getparam += (flat ? (string.IsNullOrEmpty(getparam) ? "?" : "&") + "flat=true" : "");
            getparam += (update_counts ? (string.IsNullOrEmpty(getparam) ? "?" : "&") + "update_counts=true" : "");

            string tempurl = host + "/reader/feeds";
            var responsestr =await client.GetStringAsync(tempurl + getparam);
            var responseobj = JsonConvert.DeserializeObject<feedsresponse>(responsestr);
            var feedlist = from c in responseobj.feeds select new FeedInfoBase {
                Id = c.Key,
                FeedUrl=c.Value.feed_address.ToString(),
                FeedName=c.Value.feed_title
            };
            return feedlist.ToList();
        }


    }
}
