using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JustForFeed.View
{
    /// <summary>
    /// WindowSetting.xaml 的交互逻辑
    /// 设置窗口
    /// </summary>
    public partial class WindowSetting : Window
    {
        public WindowSetting()
        {
            InitializeComponent();
            this.Loaded += WindowSetting_Loaded;
        }

        private void WindowSetting_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBox.Text = RunTime.DataPath;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (System.Windows.Forms.DialogResult.OK == folder.ShowDialog())
            {
                this.textBox.Text = System.IO.Path.Combine(folder.SelectedPath, "Data");
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("移动到\"" + this.textBox.Text + "\"目录，将会清空目录内容，请确定是否移动?","确认",MessageBoxButton.YesNoCancel))
            {
                UserConfigHandler.Current.DataFolderPath = this.textBox.Text;
                UserConfigHandler.Current.SaveConfig();
                MessageBox.Show("保存完成");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
