using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Data;
using JustForFeed.Helper;

namespace JustForFeed.ViewModel
{
    /// <summary>
    /// 订阅源类
    /// </summary>
    [DataContract]
    public class FeedViewModel : BaseViewModel
    {
        private Uri link;
        /// <summary>
        /// 订阅源地址
        /// </summary>
        [DataMember]
        public Uri Link
        {
            get { return link; }
            set
            {
                this.link = value;
                RaisePropertyChanged(() => Link);
            }
        }

        /// <summary>
        /// 界面绑定地址字段
        /// </summary>
        public string LinkString
        {
            get { return this.Link?.OriginalString ?? string.Empty; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                if (!value.Trim().StartsWith("http://") && !value.Trim().StartsWith("https://"))
                {
                    //IsInError = true;
                    //ErrorMessage = NOT_HTTP_MESSAGE;
                }
                else
                {
                    Uri uri = null;
                    if (Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri))
                    {
                        Link = uri;
                    }
                    else
                    {
                        //IsInError = true;
                        //ErrorMessage = INVALID_URL_MESSAGE;
                    }
                }
            }
        }

        private string name = string.Empty;
        /// <summary>
        /// 订阅源名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        [DataMember]
        public ObservableCollection<ArticleViewModel> Articles { get; set; } = new ObservableCollection<ArticleViewModel>();

        private ListCollectionView articlesview = null;
        public ListCollectionView ArticlesView
        {
            get
            {
                if (articlesview == null && Articles != null)
                {
                    ArticlesView = new ListCollectionView(Articles);
                    ArticlesView.SortDescriptions.Add(new System.ComponentModel.SortDescription("PublishedDate", System.ComponentModel.ListSortDirection.Descending));
                }
                return articlesview;
            }
            set
            {
                this.articlesview = value;
                RaisePropertyChanged(() => ArticlesView);
            }
        }

        /// <summary>
        /// 订阅源描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否需要离线
        /// </summary>
        [DataMember]
        public bool NeedOffline { get; set; } = false;


        private ICommand sta;
        /// <summary>
        /// 由于反序列时，未正确执行构造函数，无法在构造里初始化，故利用属性初始化
        /// 离线存储命令
        /// </summary>
        public ICommand OfflineCommand
        {
            get
            {
                if (sta == null)
                {
                    sta = new RelayCommand(Offline);
                }
                return sta;
            }
        }

        private RelayCommand refresharticlecommand;
        /// <summary>
        /// 更新订阅源文章
        /// </summary>
        public RelayCommand RefreshArticleCommand
        {
            get
            {
                if (refresharticlecommand == null)
                {
                    refresharticlecommand = new RelayCommand(RefreshArticles);
                }
                return refresharticlecommand;
            }
            set { refresharticlecommand = value; }
        }


        /// <summary>
        /// 离线feed
        /// </summary>
        void Offline()
        {
            NeedOffline = true;
            ServiceLocator.Current.GetInstance<MainViewModel>().Offline(this);
        }

        /// <summary>
        /// 更新文章信息
        /// </summary>
        async void RefreshArticles()
        {
            await this.RefreshAsync();
            RaisePropertyChanged(() => Articles);
            if (NeedOffline)
            {
                OfflineCommand.Execute(null);
            }
        }
    }
}
