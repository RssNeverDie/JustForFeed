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
        public static string DataPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public static string OfflineDetailPath { get; set; } = Path.Combine(DataPath, "Data");

        static RunTime()
        {
            if (!Directory.Exists(OfflineDetailPath))
            {
                Directory.CreateDirectory(OfflineDetailPath);
            }
        }
    }
}
