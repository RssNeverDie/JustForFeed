using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            XmlReader r = XmlReader.Create("http://www.shisujie.com/rss?containerid=31");
            string rr = r.ReadInnerXml();
            //SyndicationFeed test = SyndicationFeed.Load(r); // new SyndicationFeed("", "", new Uri("http://www.shisujie.com/rss?containerid=31"));

            //IEnumerable<SyndicationItem> aa = test.Items;
            HtmlAgilityPack.HtmlDocument a = new HtmlAgilityPack.HtmlDocument();
            a.LoadHtml("<p>aaaa</p>");
            WebBrowser web = new WebBrowser();
            web.NavigateToString("<p>aaaa</p><img title=\"changerowgroupproperty\"  alt=\"修改行组属性\" src=\"http://www.canself.com/Media/Default/Open-Live-Writer/ReportViewer_1372A/image_thumb_2.png\">");
            this.frame.Navigate(web);
          

        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
           
        }
        
    }
}
