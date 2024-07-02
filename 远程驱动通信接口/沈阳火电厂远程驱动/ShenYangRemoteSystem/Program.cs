using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShenYangRemoteSystem
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string processesName = "ShenYangRemoteSystem";
            System.Diagnostics.Process[] targetProcess = System.Diagnostics.Process.GetProcessesByName(processesName);
            int currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            foreach (System.Diagnostics.Process process in targetProcess)
            {
                if (process.Id != currentProcessId)
                {
                    process.Kill(); // 关闭先前的程序
                }
            }

            // 注册全局异常处理事件
            //Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        #region 全局异常处理
        // 处理在UI线程中发生的异常
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                // 记录异常信息
                Exception ex = e.Exception;

                Thread thread = new Thread(() => OperationLogs(ex.ToString()));
                thread.Start();

                // 显示错误信息给用户，或者执行其他操作
                MessageBox.Show("应用程序遇到了一个未处理的异常，请联系技术支持。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }

        // 处理在非UI线程中发生的异常
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // 记录异常信息
                Exception ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    Thread thread = new Thread(() => OperationLogs(ex.ToString()));
                    thread.Start();

                    // 显示错误信息给用户，或者执行其他操作
                    MessageBox.Show("应用程序遇到了一个未处理的异常，请联系技术支持。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch { }
        }

        // 记录异常信息的方法，将异常信息保存到数据库
        public static void OperationLogs(string operationinfo)
        {
            //记录异常信息，将异常信息存入数据库
            string insertSql = String.Format("insert into debug0328 (time, info) values('{0}','{1}')", DateTime.Now, operationinfo);

            ExecuteSql(insertSql);

            //删除debug0328表三个月前的数据
            string deleteSql = String.Format("delete from debug0328 where time < '{0}' ;", DateTime.Now.AddMonths(-3));
            ExecuteSql(deleteSql);
        }

        public static string connstr = "server=" + "192.168.1.180" + ";database= " + "csr" + ";username=" + "root" + ";password=" + "123456" + ";Charset=utf8";

        public static void ExecuteSql(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        conn.Close();
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }
        #endregion
    }
}
