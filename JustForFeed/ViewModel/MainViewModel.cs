using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using JustForFeed.Helper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
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
        /// <summary>
        /// 收藏——类似订阅源处理
        /// </summary>
        public FeedViewModel FavoritesFeed { get; set; } = new FeedViewModel();

        /// <summary>
        /// 订阅源列表
        /// </summary>
        public ObservableCollection<FeedViewModel> NewFeeds { get; } = new ObservableCollection<FeedViewModel>();

        private FeedViewModel currentFeed = new FeedViewModel();
        /// <summary>
        /// 当前订阅源
        /// </summary>
        public FeedViewModel CurrentFeed
        {
            get { return currentFeed; }
            set
            {
                this.currentFeed = value;
                RaisePropertyChanged(() => CurrentFeed);
                RemoveFeedCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 含有收藏的订阅源列表
        /// </summary>
        public ObservableCollection<FeedViewModel> FeedsWithFavorites
        {
            get
            {
                return new ObservableCollection<FeedViewModel>(new FeedViewModel[] { FavoritesFeed }.Concat(NewFeeds));
            }
        }

        private ArticleViewModel currentarticle;
        /// <summary>
        /// 当前选中的文章
        /// </summary>
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
        public RelayCommand ConfirmAddCommand { get; set; }

        /// <summary>
        /// 移除订阅源命令
        /// </summary>
        public RelayCommand RemoveFeedCommand { get; set; }

        /// <summary>
        /// 更新订阅源名称
        /// </summary>
        public ICommand RefreshFeedNameCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"

                AddNewFeedCommand = new RelayCommand(AddNewFeed);
                ConfirmAddCommand = new RelayCommand(ConfirmAdd, IsFeedCanUse);
                RemoveFeedCommand = new RelayCommand(RemoveFeed, CanRemoveFeed);
                RefreshFeedNameCommand = new RelayCommand(RefreshFeedName);

                Init();
            }
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        async void Init()
        {
            FavoritesFeed = await FeedDataHandler.GetFavoritesAsync() ?? FavoritesFeed;
            if (string.IsNullOrWhiteSpace(FavoritesFeed.Name)) FavoritesFeed.Name = "收藏";
            if (FavoritesFeed.Articles == null) FavoritesFeed.Articles = new ObservableCollection<ArticleViewModel>();

            NewFeeds.CollectionChanged += NewFeeds_CollectionChanged;
            NewFeeds.Clear();
            FeedDataHandler.GetFeedsAsync().ForEach(feed => NewFeeds.Add(feed));

            FavoritesFeed.Articles.CollectionChanged += Articles_CollectionChanged;
        }

        /// <summary>
        /// 订阅源有更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewFeeds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => FeedsWithFavorites);
        }

        /// <summary>
        /// 文章列表有更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Articles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await FavoritesFeed.SaveFavoritesAsync();
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

        /// <summary>
        /// 确定添加订阅源 
        /// </summary>
        async void ConfirmAdd()
        {
            try
            {
                NewFeeds.Add(CurrentFeed);
                await CurrentFeed.RefreshAsync();
                await NewFeeds.SaveAsync();

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("成功添加"), "AddSuccess");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 移除订阅源
        /// </summary>
        void RemoveFeed()
        {
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("确定删除订阅源\"" + CurrentFeed.Name + "\"吗？", (Action)(async () =>
                {
                    if (NewFeeds.Contains(CurrentFeed))
                    {
                        NewFeeds.Remove(CurrentFeed);
                        await NewFeeds.SaveAsync();
                    }
                })), "ShowUserCheckedForm");
        }
        /// <summary>
        /// 是否可以移除订阅源
        /// </summary>
        /// <returns></returns>
        bool CanRemoveFeed()
        {
            if (CurrentFeed == FavoritesFeed)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证订阅源可用性_并更新名称
        /// </summary>
        bool IsFeedCanUse()
        {
            try
            {
                if (CurrentFeed.Link == null)
                {
                    return false;
                }

                var withoutwait = FeedDataHandler.GetRSSStreamInfo1(CurrentFeed.Link);
                XmlReader r = XmlReader.Create(withoutwait);

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

        /// <summary>
        /// 同步收藏文章
        /// </summary>
        public void SyncFavoritesFeed(ArticleViewModel article)
        {
            if (article.IsStarred)
            {
                if (FavoritesFeed.Articles.Contains(article))
                    return;
                FavoritesFeed.Articles.Insert(0, article);
            }
            else
            {
                FavoritesFeed.Articles.Remove(article);
                //var ar = FavoritesFeed.Articles.FirstOrDefault(a => a.Equals(article));
                //if (ar != null) { FavoritesFeed.Articles.Remove(ar); }
            }
        }

        /// <summary>
        /// feed离线
        /// </summary>
        public async void Offline(FeedViewModel feed)
        {
            await feed.OfflineFeed();
        }
    }
}