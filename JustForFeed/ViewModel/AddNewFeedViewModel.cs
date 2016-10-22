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
        //private string feedurl;
        ///// <summary>
        ///// 订阅源地址
        ///// </summary>
        //public string FeedUrl
        //{
        //    get { return feedurl; }
        //    set
        //    {
        //        feedurl = value;
        //        RaisePropertyChanged(() => FeedUrl);
        //        (ConfirmAddCommand as RelayCommand).RaiseCanExecuteChanged();
        //    }
        //}

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

      

        public AddNewFeedViewModel()
        {
         
        }

    }
}
