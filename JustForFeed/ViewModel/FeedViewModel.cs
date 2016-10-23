using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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

        /// <summary>
        /// 订阅源描述
        /// </summary>
        public string Description { get; set; }

    }
}
