using GalaSoft.MvvmLight.Messaging;
using JustForFeed.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JustForFeed.View
{
    /// <summary>
    /// WindowAddNewFeed.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAddNewFeed : Window
    {

        //AddNewFeedViewModel ViewModel
        //{
        //    get { return this.DataContext as AddNewFeedViewModel; }
        //    set { this.DataContext = value; }
        //}

        public WindowAddNewFeed()
        {
            InitializeComponent();
            this.btn_cancle.Click += Btn_cancle_Click;
            // ViewModel = new AddNewFeedViewModel();
            this.Loaded += WindowAddNewFeed_Loaded;
            this.Unloaded += WindowAddNewFeed_Unloaded;

        }

        private void WindowAddNewFeed_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<NotificationMessage>(this, "AddSuccess", c);
        }

        private void WindowAddNewFeed_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<NotificationMessage>(this, "AddSuccess", c);
        }


        /// <summary>
        /// 取消添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_cancle_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 添加成功
        /// </summary>
        void c(NotificationMessage sender)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
