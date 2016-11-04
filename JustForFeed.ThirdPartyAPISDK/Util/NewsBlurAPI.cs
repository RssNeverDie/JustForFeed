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
            //client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
            //client.DefaultRequestHeaders.AcceptCharset.ParseAdd("UTF-8");
            //client.DefaultRequestHeaders.AcceptCharset.ParseAdd("GB2312");
            //client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3");
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

                var auth = new AuthenticationBase { IsAuthSuccess = loginobj.authenticated, Message = (loginobj.errors ?? (object)"").ToString() };
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
            var responsestr = await client.GetStringAsync(tempurl + getparam);
            var responseobj = JsonConvert.DeserializeObject<feedsresponse>(responsestr);
            var feedlist = from c in responseobj.feeds
                           select new FeedInfoBase
                           {
                               Id = c.Key,
                               FeedUrl = c.Value.feed_address.ToString(),
                               FeedName = c.Value.feed_title
                           };
            return feedlist.ToList();
        }

        /// <summary>
        /// 获取订阅源图标
        /// </summary>
        /// <param name="feedid"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, byte[]>> GetFeedIcon(params string[] feedid)
        {
            string getparam = string.Empty;
            foreach (var item in feedid)
            {
                getparam += ("feed_ids=" + item);
                getparam += "&";
            }
            if (!string.IsNullOrEmpty(getparam))
            {
                getparam = "?" + getparam.TrimEnd('&');
            }

            string tempurl = host + "/reader/favicons" + getparam;
            var reponsestr = await client.GetStringAsync(tempurl);
            var responseobj = JsonConvert.DeserializeObject<faviconsresponse>(reponsestr);
            Dictionary<string, byte[]> tempicons = new Dictionary<string, byte[]>();
            foreach (var item in feedid)
            {
                tempicons.Add(item, responseobj.GetIcon(item));
            }
            return tempicons;
        }

        /// <summary>
        /// 获取订阅源的原始页面
        /// TODO 存在乱码问题，好像还有ssl问题
        /// </summary>
        /// <param name="feedid"></param>
        /// <returns></returns>
        public static async Task<string> GetOriginalPage(string feedid)
        {
            string tempurl = host + "/reader/page/" + feedid;

            //var responsestream = await client.GetStreamAsync(tempurl);
            //System.IO.StreamReader reader = new System.IO.StreamReader(responsestream, Encoding.GetEncoding("utf-8"));
            //string aa = reader.ReadToEnd();
            //string bb = System.Net.WebUtility.HtmlDecode(aa);

            var reponsestr = await client.GetStringAsync(tempurl);
            return reponsestr;
        }

        /// <summary>
        /// 获取刷新未读计数——每分钟至多一次
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, int>> GetRefreshFeeds()
        {
            string tempurl = host + "/reader/refresh_feeds";
            var responsestr = await client.GetStringAsync(tempurl);
            var responseobj = JsonConvert.DeserializeObject<refreshfeedsresponse>(responsestr);
            Dictionary<string, int> feedcount = new Dictionary<string, int>();
            foreach (var item in responseobj.feeds)
            {
                feedcount.Add(item.Key, item.Value.nt);
            }
            return feedcount;
        }

    }
}
