using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForFeed
{
    /// <summary>
    /// 用户配置信息处理
    /// </summary>
    public class UserConfigHandler
    {
        static UserConfigHandler()
        {
            InitUserConfig();
        }

        static void InitUserConfig()
        {
            string configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user.config");
            if (!File.Exists(configpath))
            {
                Configuration temp = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                temp.SaveAs(configpath);
            }

            ConfigurationFileMap map = new ConfigurationFileMap(configpath);
            Configuration userconfig = ConfigurationManager.OpenMappedMachineConfiguration(map);
            if (userconfig.AppSettings == null)
            {
                userconfig.Sections.Add("appSettings", new AppSettingsSection());
                userconfig.Save();
            }
            AppSettingsSection appsetting = userconfig.AppSettings;
            if (appsetting.Settings.AllKeys.Contains("DataFolderPath"))
            {
                Current.DataFolderPath = appsetting.Settings["DataFolderPath"].Value;
            }

            if (!Directory.Exists(Current.DataFolderPath))
            {
                string datapath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                appsetting.Settings.Remove("DataFolderPath");
                appsetting.Settings.Add("DataFolderPath", datapath);
                userconfig.Save();

                Current.DataFolderPath = datapath;
                Directory.CreateDirectory(Current.DataFolderPath);
            }
        }

        private static UserConfigHandler current = new UserConfigHandler();
        public static UserConfigHandler Current
        {
            get { return current; }
        }

        /// <summary>
        /// 本地数据存储路径
        /// </summary>
        public string DataFolderPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig()
        {
            string configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user.config");

            ConfigurationFileMap map = new ConfigurationFileMap(configpath);
            Configuration userconfig = ConfigurationManager.OpenMappedMachineConfiguration(map);

            AppSettingsSection appsetting = userconfig.AppSettings;

            //获取改变目录前数据存储路径
            string previewdatapath = string.Empty;
            if (!appsetting.Settings.AllKeys.Contains("DataFolderPath"))
            {
                previewdatapath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            }
            else
            {
                previewdatapath = appsetting.Settings["DataFolderPath"].Value;
            }

            appsetting.Settings.Remove("DataFolderPath");
            appsetting.Settings.Add("DataFolderPath", DataFolderPath);

            #region 数据移动
            if (Directory.Exists(DataFolderPath))
            {
                Directory.Delete(DataFolderPath);
            }
            if (Directory.Exists(previewdatapath))
            {
                Directory.CreateDirectory(previewdatapath).MoveTo(DataFolderPath);
                //Directory.Move(previewdatapath, DataFolderPath);
            }
            #endregion

            userconfig.Save();
        }



    }
}
