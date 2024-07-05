using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;
using Newtonsoft.Json;
using WebSocketSharp;
using Win_Test;


namespace Win_Test
{
    public class WebSocketHelper
    {
        /// <summary>
        /// WebSocketHelper类的构造函数
        /// </summary>
        /// <param name="systemVariables">系统变量类的对象</param>
        public WebSocketHelper(Form1 form)
        {
            form1 = form;
        }

        Form1 form1;


        /// <summary>
        /// 服务器WebSocket绑定的IP地址及端口号
        /// </summary>
        public string point;

        /// <summary>
        /// 系统变量的对象
        /// </summary>
        //public SystemVariables systemVariables;

        /// <summary>
        /// 在服务器端维护一个列表，用于存储已连接的套接字
        /// </summary>
        private List<IWebSocketConnection> connectClients = new List<IWebSocketConnection>();

        /// <summary>
        /// 存储 WebSocket 服务器对象的引用，用来管理 WebSocket 服务器实例，用于监听指定端口上的 WebSocket 连接。
        /// </summary>
        private WebSocketServer socketWatcher;


        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };

        #region 服务器监听方法
        /// <summary>
        /// 监听给定的WebSocket端口，创建服务
        /// </summary>
        /// <param name="point">要监听的端口</param>
        /// <returns>无</returns>
        public void Listen(string point)
        {
            try
            {
                socketWatcher = new WebSocketServer($"ws://{point}");
                socketWatcher.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        connectClients.Add(socket);
                        ReportException("有客户端连接");
                    };

                    socket.OnClose = () =>
                    {
                        connectClients.Remove(socket);
                        ReportException("有客户端断开");
                    };

                    socket.OnMessage = message =>
                    {
                        ReportException("收到信息：" + message);


                    };
                });
            }
            catch (Exception ey)
            {
                ReportException("错误： " + ey);
            }
        }
        #endregion

        #region 客户端连接方法
        /// <summary>
        /// 连接至指定端口的WebSocket并发送消息
        /// </summary>
        /// <param name="point">指定端口</param>
        /// <param name="text">要发送的信息</param>
        public void Send(string point, string text)
        {
            WebSocket mainSocket;

            mainSocket = new WebSocket($"ws://{point}");

            mainSocket.Connect();

            //发送json字符串
            mainSocket.Send(Compress(text));

            mainSocket.OnMessage += (sender, e) =>
            {
                Stopwatch stopwatch = new Stopwatch();


                // 开始计时
                stopwatch.Start();

                string receiveMsg = e.Data;

                Thread thread = new Thread(() => ShowMsg(receiveMsg)); // Lambda表达式，匿名方法
                thread.Start();

                // 结束计时
                stopwatch.Stop();

                ShowMsg(stopwatch.ElapsedMilliseconds.ToString());

                mainSocket.Close();
            };
        }



        #endregion

        #region 压缩方法
        public static string Decompress(string compressedText)
        {
            byte[] compressedBuffer = Convert.FromBase64String(compressedText); using (MemoryStream compressedStream = new MemoryStream(compressedBuffer))
            {
                using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(resultStream);
                        return Encoding.UTF8.GetString(resultStream.ToArray());
                    }
                }
            }
        }
        //压缩字符串
        public static string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(buffer, 0, buffer.Length);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        #endregion

        #region 异常日志展示
        /// <summary>
        /// 异常日志展示
        /// </summary>
        /// <param name="addContent">异常文本</param>
        public void ReportException(string addContent)
        {
            Thread thread6 = new Thread(() => ShowMsg(addContent));

            thread6.Start();
        }

        /// <summary>
        /// 大于100时 就清空文本框
        /// </summary>
        public int displaySequence = 0;
        /// <summary>
        /// 异常日志展示
        /// </summary>
        /// <param name="receiveMsg">异常文本</param>
        public void ShowMsg(string receiveMsg)
        {
            form1.Invoke(new MethodInvoker(() =>
            {
                if (displaySequence >= 100)
                {
                    form1.rtxtDisplay.Clear();
                    displaySequence = 0;
                }
                form1.rtxtDisplay.AppendText(receiveMsg + "\n");
                form1.rtxtDisplay.ScrollToCaret();
            }));
        }

        #endregion
    }
}
