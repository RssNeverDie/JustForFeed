using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace JustForFeed.ViewModel
{
    /// <summary>
    /// 文章内容类
    /// </summary>
    [DataContract(Name = "Article")]
    public class ArticleViewModel : BaseViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public string Author { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// 文章源链接
        /// </summary>
        [DataMember]
        public Uri Link { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [DataMember]
        public DateTimeOffset PublishedDate { get; set; }

        private bool isstarred = false;
        /// <summary>
        /// 是否收藏
        /// </summary>
        [DataMember]
        public bool IsStarred
        {
            get { return this.isstarred; }
            set
            {
                isstarred = value;
                RaisePropertyChanged(() => IsStarred);
            }
        }

        private ICommand sta;
        /// <summary>
        /// 由于反序列时，未正确执行构造函数，无法在构造里初始化，故利用属性初始化
        /// 收藏文章命令
        /// </summary>
        public ICommand StarArticleCommand
        {
            get
            {
                if (sta == null)
                {
                    sta = new RelayCommand(StarArticle);
                }
                return sta;
            }
        }

        public ArticleViewModel()
        {

        }

        /// <summary>
        /// 收藏文章
        /// </summary>
        void StarArticle()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().SyncFavoritesFeed(this);
        }

        /// <summary>
        /// 重写类相等
        /// </summary>
        public override bool Equals(object obj) =>
            obj is ArticleViewModel ? (obj as ArticleViewModel).GetHashCode() == GetHashCode() : false;

        /// <summary>
        /// 重写类hashcode计算方法
        /// </summary>
        public override int GetHashCode() =>
            Link.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.Unescaped).GetHashCode();
    }
}
