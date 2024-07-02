using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Net.WebSockets;

namespace ShenYangRemoteSystem.Subclass
{
    public class SocketHelper
    {
        /// <summary>
        /// 监听套接字绑定的IP地址
        /// </summary>
        public IPAddress hostIP;
        /// <summary>
        /// 监听套接字绑定的端口号
        /// </summary>
        public int port;
        /// <summary>
        /// 系统变量的对象
        /// </summary>
        public SystemVariables systemVariables;
        /// <summary>
        /// 监听套接字支持的最大连接数，默认为5
        /// </summary>
        public int SocketMaxCount = 5;
        /// <summary>
        /// 用于连接的套接字（客户端）
        /// </summary>
        public Socket mainSocket;
        /// <summary>
        /// 用于监听的套接字（服务器）
        /// </summary>
        public Socket socketListener;
        /// <summary>
        /// 用于存储已连接的套接字
        /// </summary>
        List<Socket> connectedSockets = new List<Socket>();


        /// <summary>
        /// 用于存储收到的字符串
        /// </summary>
        public string receivedText;
        


        /// <summary>
        /// SocketHelper类有参构造方法
        /// </summary>
        /// <param name=""></param>
        /// <returns>无</returns>
        public SocketHelper(IPAddress hostIP, int port)
        {
            this.hostIP = hostIP;
            this.port = port;
        }
        /// <summary>
        /// SocketHelper类有参构造方法的重载1
        /// </summary>
        /// <param name=""></param>
        /// <returns>无</returns>
        public SocketHelper(IPAddress hostIP, int port ,SystemVariables systemVariables)
        {
            this.hostIP = hostIP;
            this.port = port;
            this.systemVariables = systemVariables;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };


        #region 套接字监听方法
        /// <summary>
        /// 监听给定的套接字端口
        /// </summary>
        /// <param name=""></param>
        /// <returns>无</returns>
        public void Listen()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(hostIP, port);//设置监听端口

                socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketListener.Bind(endPoint);//绑定端口
                socketListener.Listen(10);//开始监听

                while (true)
                {
                    Socket clientSocket = socketListener.Accept();

                    connectedSockets.Add(clientSocket);

                    //调控最大连接数
                    if (connectedSockets.Count > SocketMaxCount)
                    {
                        Socket earliestSocket = connectedSockets[0];
                        connectedSockets.RemoveAt(0);
                        earliestSocket.Shutdown(SocketShutdown.Both);
                        earliestSocket.Close();
                    }

                    Thread thread = new Thread(() => Process_SocketAccepted(1, clientSocket));
                    thread.Start();

                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                ReportException("监听异常： " + e);
            }
        }

        #endregion

        #region 套接字通信线程
        /// <summary>
        /// 实际处理套接字数据的通用方法
        /// </summary>
        /// <param name="type">使用本方法的方式（1-解析数据 *一般用于Listen()方法，2-打印数据 *一般用于Connect()方法）</param>
        /// <param name="socket">接受数据的套接字</param>
        /// <returns>无</returns>
        public void Process_SocketAccepted(int type ,object socket)
        {

            Socket clientSocket = socket as Socket;
            try
            {
                while (true)
                {
                    if (clientSocket.Connected) // 检查套接字是否已释放
                    {
                        try
                        {
                            #region 报文解析
                            byte[] frameLengthBuffer = new byte[4]; // 根据定义，数据帧长度为4个字节
                            int frameLength;

                            //接收报文头部信息
                            clientSocket.Receive(frameLengthBuffer, frameLengthBuffer.Length, 0);
                            //头部转义为整型
                            frameLength = BitConverter.ToInt32(frameLengthBuffer, 0);

                            //动态缓冲区大小
                            byte[] receiveBuffer = new byte[frameLength];

                            //接收报文主体
                            clientSocket.Receive(receiveBuffer, frameLength, 0);
                            string strInfo = Encoding.BigEndianUnicode.GetString(receiveBuffer);
                            
                            receivedText = strInfo;

                            #endregion

                            if(type == 1)
                            {
                                //以并行方式处理请求
                                string param1 = strInfo;
                                Socket param2 = clientSocket;

                                Thread thread = new Thread(() => Process_ReceiveCommand(param1, param2)); // Lambda表达式，匿名方法

                                thread.Start();
                            }
                            else if(type == 2)
                            {
                                string strInfo2 = frameLength.ToString();
                                string text = strInfo + "\n\r\n\r" + strInfo2;

                                ReportException(text);
                            }


                            Thread.Sleep(50);
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode == SocketError.Interrupted)
                            {
                                SocketExhandling(clientSocket);
                                ReportException("套接字通信被中断");
                            }
                            else
                            {
                                SocketExhandling(clientSocket);
                                ReportException("套接字通信异常：" + e);
                            }
                        }
                        catch (Exception e)
                        {
                            SocketExhandling(clientSocket);
                            ReportException("错误： " + e);
                        }

                    }
                    else // 如果套接字已被释放则终止线程
                    {
                        try
                        {
                            Thread.CurrentThread.Abort();
                        }
                        catch
                        {
                            ReportException("套接字线程被终止，原因：套接字已被释放");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ReportException(e.ToString());
            }
        }
        #endregion


        public string Receive()
        {
            return receivedText;
        }



        #region 对系统命令处理线程
        /// <summary>
        /// 对系统命令处理线程（仅远程驱动，其它系统需要修改）
        /// </summary>
        /// <param name="text">套接字收到的字符串</param>
        /// <param name="socket">服务器创建的套接字</param>
        /// <returns>无</returns>
        public void Process_ReceiveCommand(string text, Socket socket)
        {
            try
            {
                //将接收到的“系统命令”JSON 字符串反序列化为对象
                SystemCommand systemCommand = JsonConvert.DeserializeObject<SystemCommand>(text);

                systemVariables.TimeStamp = DateTime.Now;

                //将本地变量对象序列化为一帧JSON字符串
                string json = JsonConvert.SerializeObject(systemVariables, settings);

                //判断是否收到查询命令
                if (systemCommand.QUERY_TYPE == 1)
                {
                    //报文编码
                    byte[] sendByte0 = Encoding.BigEndianUnicode.GetBytes(json);
                    //帧头
                    byte[] frameLengthBuffer = new byte[4];
                    frameLengthBuffer = BitConverter.GetBytes(sendByte0.Length);
                    //组合
                    byte[] sendByte = frameLengthBuffer.Concat(sendByte0).ToArray();

                    //通过套接字发送JSON字符串给到其他子系统
                    socket.Send(sendByte, sendByte.Length, 0);
                }
                else if (systemCommand.QUERY_TYPE == 2)
                {
                    //角度、高度
                    float number = systemCommand.COMMAND_DATA;

                    //选择器
                    switch (systemCommand.COMMAND_NAME)
                    {
                        case "SUPPLYPOWER_ON": // 命令名为 "SUPPLYPOWER_ON"

                        //    UC_page12.PLC2Write("DB414.DBX651.4", 0, "True");
                        //    Thread.Sleep(700);
                        //    UC_page12.PLC2Write("DB414.DBX651.4", 0, "False");

                        //    break;


                        default:
                            // 处理其他值或未赋值的情况
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                ReportException("错误： " + e);
            }
        }
        #endregion

        #region 套接字连接方法
        /// <summary>
        /// 连接至指定端口的套接字
        /// </summary>
        /// <param name="hostIP">指定EndPoint的IP地址</param>
        /// <param name="port">指定EndPoint的端口号</param>
        /// <returns>无</returns>
        public void Connect(string hostIP, string port)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(hostIP), int.Parse(port));//设置目标端口

                mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //实例化Socket对象
                mainSocket.Connect(endPoint);

                ReportException("尝试连接至远程驱动端...\r\n");


                Thread thread = new Thread(() => Process_SocketAccepted(2, mainSocket));
                thread.Start();
            }
            catch (Exception ey)
            {
                ReportException("服务器没有开启\r\n" + ey);
            }
        }
        #endregion

        #region 套接字发送方法
        /// <summary>
        /// 套接字发送方法（发送Json）
        /// </summary>
        /// <param name="TYPE1">当前系统</param>
        /// <param name="TYPE2">数据类型</param>
        /// <param name="TYPE3">命令类型</param>
        /// <param name="TYPE4">命令名</param>
        /// <param name="TYPE5">命令整形</param>
        /// <param name="TYPE6">命令浮点型</param>
        public void Send(string TYPE1,int TYPE2, int TYPE3, string TYPE4 , int TYPE5 , float TYPE6)
        {
            SystemCommand command = new SystemCommand();

            try
            {
                command.QUERY_SYSTEM = TYPE1;
                command.DATA_TYPE = TYPE2;
                command.QUERY_TYPE = TYPE3;
                command.COMMAND_NAME = TYPE4;
                command.COMMAND_TYPE = TYPE5;
                command.COMMAND_DATA = TYPE6;


                string json = JsonConvert.SerializeObject(command, settings);


                //定义报文主体字节数组
                byte[] sendByte0 = Encoding.BigEndianUnicode.GetBytes(json);
                //定义报文头部字节数组
                byte[] frameLengthBuffer = new byte[4];
                //报文主体的数组的元素总数（长度）转义成数组，赋值给定义好的变量
                frameLengthBuffer = BitConverter.GetBytes(sendByte0.Length);
                //通过方法将两个数组拼接在一起
                byte[] sendByte = frameLengthBuffer.Concat(sendByte0).ToArray();


                mainSocket.Send(sendByte, sendByte.Length, 0);
            }
            catch (Exception ex)
            {
                ReportException("命令发送出现问题：" + ex);
                SocketExhandling(mainSocket);
            }
        }
        /// <summary>
        /// Send()重载1
        /// </summary>
        /// <param name="TYPE1">当前系统</param>
        /// <param name="TYPE2">数据类型</param>
        /// <param name="TYPE3">命令类型</param>
        /// <param name="TYPE4">命令名</param>
        /// <param name="TYPE5">命令整形</param>
        public void Send(string TYPE1,int TYPE2, int TYPE3, string TYPE4, int TYPE5)
        {
            Send(TYPE1 ,TYPE2, TYPE3, TYPE4, TYPE5, 0);
        }
        /// <summary>
        /// Send()重载2
        /// </summary>
        /// <param name="TYPE1">当前系统</param>
        /// <param name="TYPE2">数据类型</param>
        /// <param name="TYPE3">命令类型</param>
        /// <param name="TYPE4">命令名</param>
        /// <param name="TYPE6">命令浮点型</param>
        public void Send(string TYPE1, int TYPE2, int TYPE3, string TYPE4, float TYPE6)
        {
            Send(TYPE1, TYPE2, TYPE3, TYPE4, 0, TYPE6);
        }
        /// <summary>
        /// Send()重载3
        /// </summary>
        /// <param name="TYPE1">当前系统</param>
        /// <param name="TYPE2">数据类型</param>
        /// <param name="TYPE3">命令类型</param>
        /// <param name="TYPE4">命令名</param>=
        public void Send(string TYPE1, int TYPE2, int TYPE3, string TYPE4)
        {
            Send(TYPE1, TYPE2, TYPE3, TYPE4, 0, 0);
        }
        /// <summary>
        /// Send()重载4
        /// </summary>
        /// <param name="TYPE1">当前系统</param>
        /// <param name="TYPE2">数据类型</param>
        /// <param name="TYPE3">命令类型</param>
        public void Send(string TYPE1, int TYPE2, int TYPE3)
        {
            Send(TYPE1, TYPE2, TYPE3, null, 0, 0);
        }
        #endregion

        #region 套接字异常处理（主动断开连接）
        /// <summary>
        /// 套接字异常处理
        /// </summary>
        /// <param name="clientSocket">要断开的套接字</param>
        /// <returns>无</returns>
        public void SocketExhandling(Socket clientSocket)
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    clientSocket.Dispose();
                }
            }
            catch (Exception e)
            {
                ReportException("未知错误： " + e);
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
        public void ReportException(string addContent)
        {
            displaySequence++;

            if (Form1.uc5 != null)
            {
                while (!Form1.uc5.IsHandleCreated)
                {
                    ;
                }

                foreach (Control ctrl in Form1.uc5.Controls)
                {
                    if (ctrl is Panel)
                    {
                        Panel panel = (Panel)ctrl;
                        foreach (Control innerCtrl in panel.Controls)
                        {
                            if (innerCtrl.GetType() == typeof(RichTextBox))
                            {
                                RichTextBox richTextBox = (RichTextBox)innerCtrl;

                                Form1.uc5.Invoke(new MethodInvoker(() =>
                                {
                                    if (displaySequence >= 100)
                                    {
                                        richTextBox.Clear();
                                        displaySequence = 0;
                                    }
                                    richTextBox.AppendText(addContent + "\n");
                                    richTextBox.ScrollToCaret();
                                }));
                            }
                        }
                    }
                }
            }
            else
            {
                Thread thread = new Thread(() => MessageBox.Show(addContent)); // Lambda表达式，匿名方法
                thread.Start();
            }
        }
        #endregion
    }
}
