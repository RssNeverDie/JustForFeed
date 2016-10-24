using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed
{
    public class RunTime
    {
        public static string DataPath
        {
            get
            {
                return UserConfigHandler.Current.DataFolderPath;
            }
            set
            {
                UserConfigHandler.Current.DataFolderPath = value;
            }
        }

        public static string OfflineDetailPath { get { return Path.Combine(DataPath, "FeedsData"); } }

        static RunTime()
        {
            if (!Directory.Exists(OfflineDetailPath))
            {
                Directory.CreateDirectory(OfflineDetailPath);
            }
        }
    }
}
