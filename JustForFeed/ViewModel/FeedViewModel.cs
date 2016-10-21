using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ViewModel
{
    public class FeedViewModel
    {


        public Uri Link { get; set; }

        public string Name { get; set; }

        public ObservableCollection<ArticleViewModel> Articles { get; set; } = new ObservableCollection<ArticleViewModel>();
    }
}
