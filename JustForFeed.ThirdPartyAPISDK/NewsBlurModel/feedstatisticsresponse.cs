using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed.ThirdPartyAPISDK.NewsBlurModel
{
    /// <summary>
    /// 统计信息
    /// </summary>
    class feedstatisticsresponse
    {
        public int premium_subscribers { get; set; }

        public int subscriber_count { get; set; }

        public object classifier_counts { get; set; }

        public fetchstatistics[] page_fetch_history { get; set; }

        public string result { get; set; }

        public object last_load_time { get; set; }

        public int active_subscribers { get; set; }

        public int premium_update_interval_minutes { get; set; }

        public bool authenticated { get; set; }

        public string last_update { get; set; }

        public fetchstatistics[] feed_fetch_history { get; set; }

        public dynamic feed_push_history { get; set; }
        public int average_stories_per_month { get; set; }
        public Dictionary<string,int> story_days_history { get; set; }
        public int errors_since_good { get; set; }
        public int stories_last_month { get; set; }
        public bool active { get; set; }
        public long user_id { get; set; }
        public string next_update { get; set; }
        public int num_subscribers { get; set; }
        public  int update_interval_minutes { get; set; }
        public object[][] story_count_history { get; set; }

        public bool push { get; set; }

        public Dictionary<string,int> story_hours_history { get; set; }

        public int active_premium_subscribers { get; set; }
    }

    class fetchstatistics
    {
        public DateTime fetch_date { get; set; }
        public string message { get; set; }
        public int status_code { get; set; }
    }

    
}
