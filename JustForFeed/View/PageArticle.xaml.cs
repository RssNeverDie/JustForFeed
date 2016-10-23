using JustForFeed.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JustForFeed.View
{
    /// <summary>
    /// PageArticle.xaml 的交互逻辑
    /// 具体文章页面
    /// </summary>
    public partial class PageArticle : Page
    {
        ArticleViewModel ViewModel
        {
            get { return this.DataContext as ArticleViewModel; }
            set { this.DataContext = value; }
        }

        public PageArticle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 外部传递viewmodel后设置页面
        /// </summary>
        /// <param name="viewmodel"></param>
        public void SetViewModel(ArticleViewModel viewmodel)
        {
            ViewModel = viewmodel;
            this.webbr.NavigateToString(ConvertExtendedASCII(ViewModel.Summary));
        }
        /// <summary>
        /// 处理中文页面乱码
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        private static string ConvertExtendedASCII(string HTML)
        {
            string retVal = "";
            char[] s = HTML.ToCharArray();

            foreach (char c in s)
            {
                if (Convert.ToInt32(c) > 127)
                    retVal += "&#" + Convert.ToInt32(c) + ";";
                else
                    retVal += c;
            }

            return retVal;
        }

        /// <summary>
        /// 查看原文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOriginalArticle_Click(object sender, RoutedEventArgs e)
        {

            HttpClient a = new HttpClient();
            a.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            string ss = a.GetStringAsync(ViewModel.Link).Result;
            this.webbr.SuppressScriptErrors(true);
            this.webbr.NavigateToString(ConvertExtendedASCII(ss));

            //this.webbr.Navigate(ViewModel.Link);
            //IE Webbrowser 因跨域问题无法打开博客园中需要跨域访问的资源，故使用上述http获取
            //this.NavigationService.Navigate(ViewModel.Link);
        }

        /// <summary>
        /// 外部浏览器打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenArticleWithOutsideWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(ViewModel.Link.ToString());
        }
    }

    /// <summary>
    /// 禁用弹窗
    /// </summary>
    public static class WebBrowserExtensions
    {
        public static void SuppressScriptErrors(this WebBrowser webBrowser, bool hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}
