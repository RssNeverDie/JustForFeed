using SharpConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTestForThirdPartyAPISDK
{
    /// <summary>
    /// NewsBlur登录框
    /// </summary>
    public partial class LoginNewsBlur : Form
    {
        public LoginNewsBlur()
        {
            InitializeComponent();
            this.tb_UserName.Focus();

            IniConfig();

        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            UserInfoForTest.username = this.tb_UserName.Text;
            UserInfoForTest.password = this.tb_Password.Text;
            SaveConfig();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void IniConfig()
        {
            string configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "User.cfg");
            if (!File.Exists(configpath))
                return;

            Configuration config = Configuration.LoadFromFile("User.cfg");
            Section section = config["General"];
            this.tb_UserName.Text = section["UserName"].StringValue;
            this.tb_Password.Text = section["Password"].StringValue;
            this.btn_OK.Focus();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig()
        {
            string configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "User.cfg");
            Configuration config;
            if (!File.Exists(configpath))
            {
                config = new Configuration();
            }
            else
            {
                config = Configuration.LoadFromFile("User.cfg");
            }

            config["General"]["UserName"].StringValue = this.tb_UserName.Text;
            config["General"]["Password"].StringValue = this.tb_Password.Text;
            config.SaveToFile("User.cfg");
        }

    }
}
