using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using JustForFeed.Data;
using JustForFeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace JustForFeed.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private List<Feed> feeds = new List<Feed>();
        /// <summary>
        /// 订阅源列表
        /// </summary>
        public List<Feed> Feeds
        {
            get { return feeds; }
            set
            {
                Set(() => Feeds, ref feeds, value);
                FeedsView = new ListCollectionView(Feeds);
                RaisePropertyChanged(() => FeedsView);
            }
        }

        public CollectionView FeedsView { get; private set; }

        /// <summary>
        /// 添加订阅命令
        /// </summary>
        public ICommand AddNewFeedCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            AddNewFeedCommand = new RelayCommand(AddNewFeed);

            Init();
        }

        void Init()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                List<FeedInfo> infolist = RSSHandle.ReadFromXml(path);
                if (infolist != null)
                {
                    var feedstemp = from c in infolist
                                    select new Feed
                                    {
                                        Name = c.Name
                                    };
                    Feeds = feedstemp.ToList();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 添加订阅源
        /// </summary>
        void AddNewFeed()
        {
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("打开添加订阅源窗口", new Action(() =>
            {
                //成功添加后，刷新列表
                Init();
            })), "ShowAddFeed");
        }


    }
}