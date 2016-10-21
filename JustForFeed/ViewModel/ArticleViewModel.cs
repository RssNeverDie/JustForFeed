using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JustForFeed.ViewModel
{
    [DataContract(Name = "Article")]
    public class ArticleViewModel
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Summary { get; set; }
    }
}
