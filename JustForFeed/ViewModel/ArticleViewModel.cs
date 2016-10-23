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
    [DataContract(Name = "Article")]
    [KnownType(typeof(ICommand))]
    public class ArticleViewModel : BaseViewModel
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public Uri Link { get; set; }

        private bool isstarred = false;
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

        public ICommand StarArticleCommand { get; set; }
        public string aa { get; set; } = "222";
       
        public ArticleViewModel()
        {
            //
            //MainViewModel aa = new ViewModel.MainViewModel();
            StarArticleCommand = new RelayCommand(StarArticle);
            aa = "111";
        }

        void StarArticle()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().SyncFavoritesFeed(this);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object. 
        /// </summary>
        public override bool Equals(object obj) =>
            obj is ArticleViewModel ? (obj as ArticleViewModel).GetHashCode() == GetHashCode() : false;

        /// <summary>
        /// Returns the hash code of the ArticleViewModel, which is based on 
        /// a string representation the Link value, using only the host and path.  
        /// </summary>
        public override int GetHashCode() =>
            Link.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.Unescaped).GetHashCode();

    }
}
