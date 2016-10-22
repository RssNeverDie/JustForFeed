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
            this.listarticle.SelectionChanged += Listarticle_SelectionChanged;
        }

        private void Listarticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.CurrentArticle == null)
            {
                this.frame.Content = string.Empty;
                return;
            }

            PageArticle page = new View.PageArticle();
            page.SetViewModel(ViewModel.CurrentArticle);
            //WebBrowser web = new WebBrowser();
            //web.NavigateToString(ConvertExtendedASCII(ViewModel.CurrentArticle.Summary));
            this.frame.Navigate(page);
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<NotificationMessageWithCallback>(this, "ShowAddFeed", ShowAddFeed);
        }

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
    }
}
