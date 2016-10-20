using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JustForFeed.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace JustForFeed.ViewModel
{
    class AddNewFeedViewModel : ViewModelBase
    {
        private string feedurl;
        /// <summary>
        /// 订阅源地址
        /// </summary>
        public string FeedUrl
        {
            get { return feedurl; }
            set
            {
                feedurl = value;
                RaisePropertyChanged(() => FeedUrl);
                (ConfirmAddCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        private string feedName;
        /// <summary>
        /// 订阅源标题
        /// </summary>
        public string FeedName
        {
            get { return feedName; }
            set
            {
                feedName = value;
                RaisePropertyChanged(() => FeedName);
            }
        }

        /// <summary>
        /// 添加订阅源命令
        /// </summary>
        public ICommand ConfirmAddCommand { get; set; }

        /// <summary>
        /// 更新订阅源名称
        /// </summary>
        public ICommand RefreshFeedNameCommand { get; set; }

        public AddNewFeedViewModel()
        {
            ConfirmAddCommand = new RelayCommand(ConfirmAdd, IsFeedCanUse);
            RefreshFeedNameCommand = new RelayCommand(RefreshFeedName);
        }

        void ConfirmAdd()
        {
            try
            {

                string path = AppDomain.CurrentDomain.BaseDirectory;
                List<FeedInfo> infolist = RSSHandle.ReadFromXml(path);
                FeedInfo newfeed = new FeedInfo { FeedUrl = this.FeedUrl, Name = FeedName };
                if (infolist == null)
                    infolist = new List<FeedInfo>();
                infolist.Add(newfeed);
                RSSHandle.SaveFeedToXml(path, infolist);

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("成功添加"), "AddSuccess");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 验证订阅源可用性_并更新名称
        /// </summary>
        bool IsFeedCanUse()
        {
            try
            {         
                if(string.IsNullOrEmpty(FeedUrl))
                {
                    return false;
                }      
                 
                XmlReader r = XmlReader.Create(RSSHandle.GetRSSStreamInfo(FeedUrl));
              
                SyndicationFeed test = SyndicationFeed.Load(r);
                if (string.IsNullOrEmpty(FeedName))
                {
                    FeedName = test.Title.Text;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 更新订阅源名称
        /// </summary>
        void RefreshFeedName()
        {
            try
            {
                if (string.IsNullOrEmpty(FeedUrl))
                {
                    return;
                }

                XmlReader r = XmlReader.Create(RSSHandle.GetRSSStreamInfo(FeedUrl));
                SyndicationFeed test = SyndicationFeed.Load(r);

                FeedName = test.Title.Text;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
