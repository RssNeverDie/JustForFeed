using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using JustForFeed.Helper;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Windows.Input;
using System.Xml;

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

        public ObservableCollection<FeedViewModel> NewFeeds { get; } = new ObservableCollection<FeedViewModel>();

        private FeedViewModel currentFeed = new FeedViewModel();
        public FeedViewModel CurrentFeed
        {
            get { return currentFeed; }
            set
            {
                this.currentFeed = value;
                RaisePropertyChanged(() => CurrentFeed);
            }
        }

        private ArticleViewModel currentarticle;

        public ArticleViewModel CurrentArticle
        {
            get { return currentarticle; }
            set
            {
                currentarticle = value;
                RaisePropertyChanged(() => CurrentArticle);
            }
        }


        /// <summary>
        /// 添加订阅命令
        /// </summary>
        public ICommand AddNewFeedCommand { get; set; }
        /// <summary>
        /// 添加订阅源命令
        /// </summary>
        public ICommand ConfirmAddCommand { get; set; }

        public ICommand RemoveFeedCommand { get; set; }

        /// <summary>
        /// 更新订阅源名称
        /// </summary>
        public ICommand RefreshFeedNameCommand { get; set; }

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
            ConfirmAddCommand = new RelayCommand(ConfirmAdd, IsFeedCanUse);
            RemoveFeedCommand = new RelayCommand(RemoveFeed);

            RefreshFeedNameCommand = new RelayCommand(RefreshFeedName);

            Init();
        }

        void Init()
        {
            NewFeeds.Clear();
            FeedDataHandler.GetFeedsAsync().ForEach(feed => NewFeeds.Add(feed));
        }

        /// <summary>
        /// 添加订阅源
        /// </summary>
        void AddNewFeed()
        {
            CurrentFeed = new ViewModel.FeedViewModel();
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("打开添加订阅源窗口", new Action(() =>
            {

            })), "ShowAddFeed");
        }

        async void ConfirmAdd()
        {
            try
            {
                NewFeeds.Add(CurrentFeed);
                await CurrentFeed.RefreshAsync();
                NewFeeds.SaveAsync();
                //string path = AppDomain.CurrentDomain.BaseDirectory;
                //List<FeedInfo> infolist = RSSHandle.ReadFromXml(path);
                //FeedInfo newfeed = new FeedInfo { FeedUrl = this.FeedUrl, Name = FeedName };
                //if (infolist == null)
                //    infolist = new List<FeedInfo>();
                //infolist.Add(newfeed);
                //RSSHandle.SaveFeedToXml(path, infolist);

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("成功添加"), "AddSuccess");
            }
            catch (Exception ex)
            {

            }
        }

        void RemoveFeed()
        {
            if (NewFeeds.Contains(CurrentFeed))
            {
                NewFeeds.Remove(CurrentFeed);
                NewFeeds.SaveAsync();
            }
        }

        /// <summary>
        /// 验证订阅源可用性_并更新名称
        /// </summary>
        bool IsFeedCanUse()
        {
            try
            {
                //if (string.IsNullOrEmpty(CurrentFeed.LinkString))
                //{
                //    return false;
                //}
                if (CurrentFeed.Link == null)
                { return false; }

                XmlReader r = XmlReader.Create(FeedDataHandler.GetRSSStreamInfo1(CurrentFeed.Link));

                SyndicationFeed test = SyndicationFeed.Load(r);
                if (string.IsNullOrEmpty(CurrentFeed.Name))
                {
                    CurrentFeed.Name = test.Title.Text;
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
        async void RefreshFeedName()
        {
            try
            {
                //if (string.IsNullOrEmpty(CurrentFeed.LinkString))
                //{
                //    return false;
                //}
                if (CurrentFeed.Link == null)
                { return; }

                XmlReader r = XmlReader.Create(await FeedDataHandler.GetRSSStreamInfo(CurrentFeed.Link));

                SyndicationFeed test = SyndicationFeed.Load(r);

                CurrentFeed.Name = test.Title.Text;

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}