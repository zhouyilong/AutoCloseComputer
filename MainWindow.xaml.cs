using AutoCloseComputer.Storage;
using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows;

namespace AutoCloseComputer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime closeDateTime;//数据库中的关机时间

        public MainWindow()
        {
            InitializeComponent();

            using( OleDbConnection conn = AccessHelper.getConn()) //getConn():得到连接对象
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                string sqlstr = "Select * from CloseDate where ID=1";
                conn.Open();
                DataSet mydataset = new DataSet();
                adapter.SelectCommand = new OleDbCommand(sqlstr, conn);
                adapter.Fill(mydataset, "CloseDate");
                string strCloseDateTime=Convert.ToString(mydataset.Tables["CloseDate"].Rows[0]["CDate"]);
                if (DateTime.TryParse(strCloseDateTime, out closeDateTime))
                {
                    if(closeDateTime>DateTime.Now)
                    {
                        StartShutDown.IsEnabled = false;
                        StopShutDown.IsEnabled = true;

                        showTime.Text = "小龙温馨提示：\n当前计算机将在" + strCloseDateTime+"关闭\n请如果需要取消定时关机，请点击取消定时关机！";
                    }
                }
            }
        }

        //开始定时关机
        private void StartShutDown_Click(object sender, RoutedEventArgs e)
        {
            string strMinutes = Minutes.Text;
            string strHours = Hours.Text;
            if(string.IsNullOrEmpty(strMinutes)&&string.IsNullOrEmpty(strHours))
            {
                MessageBox.Show("请输入要定时的时间哦！");
                return;
            }

            //格式化时间
            strMinutes = string.IsNullOrEmpty(strMinutes) ? "0" : strMinutes;
            strHours = string.IsNullOrEmpty(strHours) ? "0" : strHours;

            int minutes;
            int hours;
            if (Int32.TryParse(strMinutes, out minutes)&&Int32.TryParse(strHours,out hours))
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe ";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("shutdown -s -t " + Convert.ToString(minutes * 60+hours*60*60));   //填CMD命令
                //p.StandardInput.WriteLine("exit");
                //string strMessage = p.StandardOutput.ReadToEnd();
                //p.WaitForExit();
                //p.Close();
                //MessageBox.Show(strMessage);

                //将关机时间更新到数据库中
                DateTime dtCloseTime = DateTime.Now.AddSeconds(minutes * 60 + hours * 60 * 60);
                string strCloseTime = dtCloseTime.ToString();
                using (OleDbConnection conn = AccessHelper.getConn()) //getConn():得到连接对象
                {
                    conn.Open();
                    string strSQL = "update CloseDate set CDate='" + strCloseTime + "' where ID=1";
                    //定义command对象，并执行相应的SQL语句
                    OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
                    myCommand.ExecuteNonQuery(); 
                }

                if (dtCloseTime > DateTime.Now)
                {
                    StartShutDown.IsEnabled = false;
                    StopShutDown.IsEnabled = true;

                    showTime.Text = "小龙温馨提示：\n当前计算机将在" + strCloseTime + "关闭\n请如果需要取消定时关机，请点击取消定时关机！";
                }
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

            using (OleDbConnection conn = AccessHelper.getConn()) //getConn():得到连接对象
            {
                conn.Open();
                string strSQL = "update CloseDate set CDate='1900/1/1 00:00:00' where ID=1";
                //定义command对象，并执行相应的SQL语句
                OleDbCommand myCommand = new OleDbCommand(strSQL, conn);
                myCommand.ExecuteNonQuery(); 
            }

            StartShutDown.IsEnabled = true;
            StopShutDown.IsEnabled = false;

            showTime.Text = "已经取消了当前定时计划！";
        }

    }
}
