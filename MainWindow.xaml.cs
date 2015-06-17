using System;
using System.Diagnostics;
using System.Windows;

namespace AutoCloseComputer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //开始定时关机
        private void StartShutDown_Click(object sender, RoutedEventArgs e)
        {
            string strTime = Time.Text;
            if(string.IsNullOrEmpty(strTime))
            {
                MessageBox.Show("请输入要定时的时间哦！");
                return;
            }

            int minutes;
            if (Int32.TryParse(strTime, out minutes))
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe ";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("shutdown -s -t " + Convert.ToString(minutes * 60));   //填CMD命令

            }
            else
            {
                MessageBox.Show("不要乱输入哦，需要输入数字哦！");
                return;
            }
        }

        //取消定时关机
        private void StopShutDown_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe ";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("shutdown -a");   //填CMD命令
        }

    }
}
