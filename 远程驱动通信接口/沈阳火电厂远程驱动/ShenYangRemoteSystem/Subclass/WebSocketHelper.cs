using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using WebSocketSharp;
using static System.Net.Mime.MediaTypeNames;
using ShenYangRemoteSystem.用户控件;


namespace ShenYangRemoteSystem.Subclass
{
    public class WebSocketHelper
    {
        /// <summary>
        /// WebSocketHelper类的构造函数
        /// </summary>
        /// <param name="systemVariables">系统变量类的对象</param>
        public WebSocketHelper()
        {
        }
        /// <summary>
        /// WebSocketHelper类的有参构造函数重载1
        /// </summary>
        /// <param name="systemVariables"></param>
        public WebSocketHelper(SystemVariables systemVariables)
        {
            this.systemVariables = systemVariables;
        }
        /// <summary>
        /// WebSocketHelper类的有参构造函数重载2
        /// </summary>
        /// /// <param name="form">Form1的对象</param>
        /// <param name="systemVariables">系统变量类的对象</param>
        public WebSocketHelper(Form1 form, SystemVariables systemVariables)
        {
            form1 = form;
            this.systemVariables = systemVariables;
        }

        Form1 form1;


        /// <summary>
        /// 服务器WebSocket绑定的IP地址及端口号
        /// </summary>
        public string point;

        /// <summary>
        /// 系统变量的对象
        /// </summary>
        public SystemVariables systemVariables;

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
                        DisplayRichTextboxContentAndScroll("有客户端连接");
                    };

                    socket.OnClose = () =>
                    {
                        connectClients.Remove(socket);
                        DisplayRichTextboxContentAndScroll("有客户端断开");
                    };

                    socket.OnMessage = message =>
                    {
                        DisplayRichTextboxContentAndScroll("收到信息：" + message);

                        //处理接收到的json字符串

                        SystemCommand systemCommand = JsonConvert.DeserializeObject<SystemCommand>(message);

                        systemVariables.TimeStamp = DateTime.Now;

                        //远程驱动的处理逻辑
                        if (systemCommand.QUERY_TYPE == 1)
                        {
                            string responseMessage = JsonConvert.SerializeObject(systemVariables, settings);

                            socket.Send(Compress(responseMessage));
                        }
                        else if (systemCommand.QUERY_TYPE == 2)
                        {
                            float number = systemCommand.COMMAND_DATA;

                            //选择器
                            switch (systemCommand.COMMAND_NAME)
                            {
                                case "SUPPLYPOWER_ON": // 命令名为 "SUPPLYPOWER_ON"

                                    form1.PLCWrite(Form1.modbusHelper_D1PLC1, 0001, 0, "1");
                                    Thread.Sleep(700);
                                    form1.PLCWrite(Form1.modbusHelper_D1PLC1, 0001, 0, "0");

                                    break;


                                default:
                                    // 处理其他值或未赋值的情况
                                    break;
                            }
                        }
                    };
                });
            }
            catch (Exception ey)
            {
                DisplayRichTextboxContentAndScroll("错误： " + ey);
            }
        }

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
            mainSocket.Send(text);

            mainSocket.OnMessage += (sender, e) =>
            {
                MessageBox.Show(e.Data);
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
        /// 大于100时 就清空文本框
        /// </summary>
        public int displaySequence = 0;
        /// <summary>
        /// 异常日志展示
        /// </summary>
        /// <param name="addContent">异常文本</param>
        public void DisplayRichTextboxContentAndScroll(string addContent)
        {
            Thread thread6 = new Thread(form1.DisplayRichTextboxContentAndScroll);

            thread6.Start(addContent);
        }

        //第二种办法
        //public void DisplayRichTextboxContentAndScroll2(object addContent)
        //{
        //    try
        //    {
        //        form1.Invoke(new MethodInvoker(() =>
        //        {
        //            if (displaySequence >= 100)
        //            {
        //                Form1.uc5.rtxtDisplay.Clear();
        //                displaySequence = 0;
        //            }
        //            Form1.uc5.rtxtDisplay.AppendText(addContent + "\n");
        //            Form1.uc5.rtxtDisplay.ScrollToCaret();
        //        }));
        //    }
        //    catch (Exception)
        //    {
        //        //MessageBox.Show("Invok错误调用");
        //    }
        //}

        #endregion
    }
}
