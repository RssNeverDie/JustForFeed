using GalaSoft.MvvmLight.Messaging;
using JustForFeed.View;
using JustForFeed.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
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
using System.Xml;

namespace JustForFeed
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        MainViewModel ViewModel
        {
            get { return this.DataContext as MainViewModel; }
        }

        public MainWindow()
        {
            InitializeComponent();

            //XmlReader r = XmlReader.Create("http://www.shisujie.com/rss?containerid=31");
            //string rr = r.ReadInnerXml();
            ////SyndicationFeed test = SyndicationFeed.Load(r); // new SyndicationFeed("", "", new Uri("http://www.shisujie.com/rss?containerid=31"));

            ////IEnumerable<SyndicationItem> aa = test.Items;
            //HtmlAgilityPack.HtmlDocument a = new HtmlAgilityPack.HtmlDocument();
            //a.LoadHtml("<p>aaaa</p>");
            //WebBrowser web = new WebBrowser();
            //web.NavigateToString("<p>aaaa</p><img title=\"changerowgroupproperty\"  alt=\"修改行组属性\" src=\"http://www.canself.com/Media/Default/Open-Live-Writer/ReportViewer_1372A/image_thumb_2.png\">");
            //this.frame.Navigate(web);

            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
            this.listarticle.SelectionChanged += Listarticle_SelectionChanged;
            this.btn_setting.Click += Btn_setting_Click;
        }

        private void Btn_setting_Click(object sender, RoutedEventArgs e)
        {
            WindowSetting win = new View.WindowSetting();
            win.Owner = this;
            win.ShowDialog();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<NotificationMessageWithCallback>(this, "ShowAddFeed", ShowAddFeed);
            Messenger.Default.Unregister<NotificationMessageWithCallback>(this, "ShowUserCheckedForm", ShowUserCheckedForm);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<NotificationMessageWithCallback>(this, "ShowAddFeed", ShowAddFeed);
            Messenger.Default.Register<NotificationMessageWithCallback>(this, "ShowUserCheckedForm", ShowUserCheckedForm);
        }

        /// <summary>
        /// 文章列表选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listarticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.CurrentArticle == null)
            {
                this.frame.Content = string.Empty;
                return;
            }

            PageArticle page = new View.PageArticle();
            page.SetViewModel(ViewModel.CurrentArticle);
            this.frame.Navigate(page);
        }

        /// <summary>
        /// 显示添加订阅源窗体
        /// </summary>
        /// <param name="sender"></param>
        void ShowAddFeed(NotificationMessageWithCallback sender)
        {
            WindowAddNewFeed win = new WindowAddNewFeed();
            win.Owner = this;
            win.DataContext = this.DataContext;
            if (win.ShowDialog() ?? false)
            {
                //添加成功执行回调
                sender.Execute();
            }
        }

        /// <summary>
        /// 显示用户确认窗体
        /// </summary>
        /// <param name="sender"></param>
        void ShowUserCheckedForm(NotificationMessageWithCallback sender)
        {
            if (MessageBoxResult.OK == MessageBox.Show(sender.Notification, "确认", MessageBoxButton.OKCancel))
            {
                sender.Execute();
            }
        }
    }
}
