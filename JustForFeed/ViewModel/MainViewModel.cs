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
        /// �ղء������ƶ���Դ����
        /// </summary>
        public FeedViewModel FavoritesFeed { get; set; } = new FeedViewModel();

        /// <summary>
        /// ����Դ�б�
        /// </summary>
        public ObservableCollection<FeedViewModel> NewFeeds { get; } = new ObservableCollection<FeedViewModel>();

        private FeedViewModel currentFeed = new FeedViewModel();
        /// <summary>
        /// ��ǰ����Դ
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
        /// �����ղصĶ���Դ�б�
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
        /// ��ǰѡ�е�����
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
        /// ��Ӷ�������
        /// </summary>
        public ICommand AddNewFeedCommand { get; set; }
        /// <summary>
        /// ��Ӷ���Դ����
        /// </summary>
        public RelayCommand ConfirmAddCommand { get; set; }

        /// <summary>
        /// �Ƴ�����Դ����
        /// </summary>
        public RelayCommand RemoveFeedCommand { get; set; }

        /// <summary>
        /// ���¶���Դ����
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
        /// ���ݳ�ʼ��
        /// </summary>
        async void Init()
        {
            FavoritesFeed = await FeedDataHandler.GetFavoritesAsync() ?? FavoritesFeed;
            if (string.IsNullOrWhiteSpace(FavoritesFeed.Name)) FavoritesFeed.Name = "�ղ�";
            if (FavoritesFeed.Articles == null) FavoritesFeed.Articles = new ObservableCollection<ArticleViewModel>();

            NewFeeds.CollectionChanged += NewFeeds_CollectionChanged;
            NewFeeds.Clear();
            FeedDataHandler.GetFeedsAsync().ForEach(feed => NewFeeds.Add(feed));

            FavoritesFeed.Articles.CollectionChanged += Articles_CollectionChanged;
        }

        /// <summary>
        /// ����Դ�и���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewFeeds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => FeedsWithFavorites);
        }

        /// <summary>
        /// �����б��и���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Articles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await FavoritesFeed.SaveFavoritesAsync();
        }

        /// <summary>
        /// ��Ӷ���Դ
        /// </summary>
        void AddNewFeed()
        {
            CurrentFeed = new ViewModel.FeedViewModel();
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("����Ӷ���Դ����", new Action(() =>
            {

            })), "ShowAddFeed");
        }

        /// <summary>
        /// ȷ����Ӷ���Դ 
        /// </summary>
        async void ConfirmAdd()
        {
            try
            {
                NewFeeds.Add(CurrentFeed);
                await CurrentFeed.RefreshAsync();
                await NewFeeds.SaveAsync();

                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("�ɹ����"), "AddSuccess");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// �Ƴ�����Դ
        /// </summary>
        void RemoveFeed()
        {
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("ȷ��ɾ������Դ\"" + CurrentFeed.Name + "\"��", (Action)(async () =>
                {
                    if (NewFeeds.Contains(CurrentFeed))
                    {
                        NewFeeds.Remove(CurrentFeed);
                        await NewFeeds.SaveAsync();
                    }
                })), "ShowUserCheckedForm");
        }
        /// <summary>
        /// �Ƿ�����Ƴ�����Դ
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
        /// ��֤����Դ������_����������
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
        /// ���¶���Դ����
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
        /// ͬ���ղ�����
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
        /// feed����
        /// </summary>
        public async void Offline(FeedViewModel feed)
        {
            await feed.OfflineFeed();
        }
    }
}