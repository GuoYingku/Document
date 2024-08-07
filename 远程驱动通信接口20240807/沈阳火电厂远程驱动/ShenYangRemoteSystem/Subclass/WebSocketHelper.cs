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
using System.Timers;
using System.Diagnostics;
using MySqlX.XDevAPI.Common;
using System.Reflection;


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







            #region 随机数

            Random random = new Random();

            // 遍历所有属性，给它们赋随机数值
            foreach (var property in Form1.systemVariables.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(bool))
                {
                    //property.SetValue(Form1.systemVariables, random.Next(2) == 0);
                }
                else if (property.PropertyType == typeof(float))
                {
                    property.SetValue(Form1.systemVariables, (float)random.NextDouble());
                }
                else if (property.PropertyType == typeof(long))
                {
                    property.SetValue(Form1.systemVariables, random.Next());
                }
                //else if (property.PropertyType == typeof(ushort))
                //{
                //    property.SetValue(Form1.systemVariables, (ushort)random.Next(-110,110));
                //}
                else if (property.PropertyType == typeof(int))
                {
                    property.SetValue(Form1.systemVariables, (int)random.Next(360));
                }
            }

            Form1.d1PLC1Variables.RotaryAngle = (ushort)random.Next(250, 470);
            Form1.d1PLC1Variables.VariableAmplitudeAngle = (ushort)random.Next(347, 373);
            Form1.d1PLC1Variables.LargeCarTravelDistance = (ushort)random.Next(0, 100);

            Form1.d2PLC1Variables.RotaryAngle_2 = (ushort)random.Next(250, 470);
            Form1.d2PLC1Variables.VariableAmplitudeAngle_2 = (ushort)random.Next(347, 373);
            Form1.d2PLC1Variables.LargeCarTravelDistance_2 = (ushort)random.Next(150, 250);




            #endregion


            Thread thread = new Thread(new ThreadStart(Process1));
            thread.Start();



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
        /// 在服务器端维护一个字典，用于存储已连接的套接字和其对应的子系统名称
        /// </summary>
        private Dictionary<IWebSocketConnection, string> connectClients = new Dictionary<IWebSocketConnection, string>();


        /// <summary>
        /// 存储 WebSocket 服务器对象的引用，用来管理 WebSocket 服务器实例，用于监听指定端口上的 WebSocket 连接。
        /// </summary>
        private WebSocketServer socketWatcher;


        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };


        
        bool ROTATE_L_1;
        bool ROTATE_R_1;
        bool ELEVATE_L_1;
        bool ELEVATE_R_1;
        bool MOVE_L_1;
        bool MOVE_R_1;
        bool ROTATE_L_2;
        bool ROTATE_R_2;
        bool ELEVATE_L_2;
        bool ELEVATE_R_2;
        bool MOVE_L_2;
        bool MOVE_R_2;
        private void Process1()
        {
            int count = 0;

            while (true)
            {

                // 仿真手动操作逻辑
                if(ROTATE_L_1 == true)
                {
                    Form1.d1PLC1Variables.RotaryAngle++;
                }
                if (ROTATE_R_1 == true)
                {
                    Form1.d1PLC1Variables.RotaryAngle--;
                }

                if (ELEVATE_L_1 == true)
                {
                    Form1.d1PLC1Variables.VariableAmplitudeAngle++;
                }
                if (ELEVATE_R_1 == true)
                {
                    Form1.d1PLC1Variables.VariableAmplitudeAngle--;
                }

                if (MOVE_L_1 == true)
                {
                    Form1.d1PLC1Variables.LargeCarTravelDistance++;
                }
                if (MOVE_R_1 == true)
                {
                    Form1.d1PLC1Variables.LargeCarTravelDistance--;
                }

                if (ROTATE_L_2 == true)
                {
                    Form1.d2PLC1Variables.RotaryAngle_2++;
                }
                if (ROTATE_R_2 == true)
                {
                    Form1.d2PLC1Variables.RotaryAngle_2--;
                }

                if (ELEVATE_L_2 == true)
                {
                    Form1.d2PLC1Variables.VariableAmplitudeAngle_2++;
                }
                if (ELEVATE_R_2 == true)
                {
                    Form1.d2PLC1Variables.VariableAmplitudeAngle_2--;
                }

                if (MOVE_L_2 == true)
                {
                    Form1.d2PLC1Variables.LargeCarTravelDistance_2++;
                }
                if (MOVE_R_2 == true)
                {
                    Form1.d2PLC1Variables.LargeCarTravelDistance_2--;
                }



                //检查子系统连接状态
                //综合监控
                foreach (KeyValuePair<IWebSocketConnection, string> pair in connectClients)
                {
                    if (pair.Value == "MC")
                    {
                        Form1.systemVariables.MCCommunicationState = true;
                    }
                }
                //三维扫描
                foreach (KeyValuePair<IWebSocketConnection, string> pair in connectClients)
                {
                    if (pair.Value == "SCAN")
                    {
                        Form1.systemVariables.SCANCommunicationState = true;
                    }
                }
                //任务规划
                foreach (KeyValuePair<IWebSocketConnection, string> pair in connectClients)
                {
                    if (pair.Value == "PC")
                    {
                        Form1.systemVariables.PCCommunicationState = true;
                    }
                }


                count++;
                //MessageBox.Show(count.ToString());
                Thread.Sleep(2000);
            }
        }




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
                        connectClients.Add(socket , "UnKnown Client");
                        DisplayRichTextboxContentAndScroll("有客户端连接");
                    };

                    socket.OnError = (Exception ex) =>
                    {

                        string clientName = null;
                        // 从字典中拿到对应客户端名称
                        if (connectClients.ContainsKey(socket))
                        {
                            clientName = connectClients[socket];
                        }



                        if (clientName == "MC")
                        {
                            Form1.systemVariables.MCCommunicationState = false;
                        }
                        else if (clientName == "SCAN")
                        {
                            Form1.systemVariables.SCANCommunicationState = false;
                        }
                        else if (clientName == "PC")
                        {
                            Form1.systemVariables.PCCommunicationState = false;
                        }


                        connectClients.Remove(socket);


                        DisplayRichTextboxContentAndScroll("有客户端意外断开");
                    };

                    socket.OnClose = () =>
                    {

                        string clientName = null;
                        // 从字典中拿到对应客户端名称
                        if (connectClients.ContainsKey(socket))
                        {
                            clientName = connectClients[socket];
                        }



                        if(clientName == "MC")
                        {
                            Form1.systemVariables.MCCommunicationState = false;
                        }
                        else if(clientName == "SCAN")
                        {
                            Form1.systemVariables.SCANCommunicationState = false;
                        }
                        else if (clientName == "PC")
                        {
                            Form1.systemVariables.PCCommunicationState = false;
                        }


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

                        //检查远程驱动标识位
                        if (systemCommand.DATA_TYPE == 6) 
                        {
                            // 从消息中获取实际的客户端名称
                            string clientName = systemCommand.QUERY_SYSTEM;// "MC"

                            // 在字典中更新客户端名称
                            if (connectClients.ContainsKey(socket))
                            {
                                connectClients[socket] = clientName;
                                //当需要访问指定系统名的套接字时：IWebSocketConnection socket = connectClients[clientName];
                            }



                            //检查一帧数据标识位
                            if (systemCommand.QUERY_TYPE == 1)
                            {
                                string responseMessage = JsonConvert.SerializeObject(systemVariables, settings);

                                socket.Send(Compress(responseMessage));
                            }
                            //检查转发模式标识位
                            else if (systemCommand.QUERY_TYPE == 3)
                            {
                                string systemString = systemCommand.COMMAND_NAME;

                                if (connectClients.ContainsValue(systemString))
                                {
                                    IWebSocketConnection socket2 = connectClients.FirstOrDefault(x => x.Value == systemString).Key;

                                    string jsonString = systemCommand.DATA_STRING;

                                    socket2.Send(Compress(jsonString));


                                }


                            }
                            //检查命令模式标识位
                            else if (systemCommand.QUERY_TYPE == 2)
                            {
                                float number = systemCommand.DATA_FLOAT;

                                //选择器
                                switch (systemCommand.COMMAND_NAME)
                                {
                                    case "SUPPLYPOWER_ON": // 命令名为 "SUPPLYPOWER_ON"

                                        form1.PLCWrite(Form1.modbusHelper_D1PLC1, 0001, 0, "1");
                                        Thread.Sleep(700);
                                        form1.PLCWrite(Form1.modbusHelper_D1PLC1, 0001, 0, "0");

                                        break;

                                    case "ROTATE_LEFT_1": // 命令名为 "ROTATE_LEFT_1"

                                        ROTATE_L_1 = true;
                                        ROTATE_R_1 = false;

                                        break;

                                    case "ROTATE_RIGHT_1":

                                        ROTATE_L_1 = false;
                                        ROTATE_R_1 = true;

                                        break;

                                    case "ROTATE_STOP_1":

                                        ROTATE_L_1 = false;
                                        ROTATE_R_1 = false;

                                        break;

                                    case "ELEVATE_UP_1":

                                        ELEVATE_L_1 = true;
                                        ELEVATE_R_1 = false;

                                        break;

                                    case "ELEVATE_DOWN_1":

                                        ELEVATE_L_1 = false;
                                        ELEVATE_R_1 = true;

                                        break;

                                    case "ELEVATE_STOP_1":

                                        ELEVATE_L_1 = false;
                                        ELEVATE_R_1 = false;

                                        break;

                                    case "MOVE_FORWARD_1":

                                        MOVE_L_1 = true;
                                        MOVE_R_1 = false;

                                        break;

                                    case "MOVE_BACKWARD_1":

                                        MOVE_L_1 = false;
                                        MOVE_R_1 = true;

                                        break;

                                    case "MOVE_STOP_1":

                                        MOVE_L_1 = false;
                                        MOVE_R_1 = false;

                                        break;

                                    case "ROTATE_LEFT_2": // 命令名为 "ROTATE_LEFT_2"

                                        ROTATE_L_2 = true;
                                        ROTATE_R_2 = false;

                                        break;

                                    case "ROTATE_RIGHT_2":

                                        ROTATE_L_2 = false;
                                        ROTATE_R_2 = true;

                                        break;

                                    case "ROTATE_STOP_2":

                                        ROTATE_L_2 = false;
                                        ROTATE_R_2 = false;

                                        break;

                                    case "ELEVATE_UP_2":

                                        ELEVATE_L_2 = true;
                                        ELEVATE_R_2 = false;

                                        break;

                                    case "ELEVATE_DOWN_2":

                                        ELEVATE_L_2 = false;
                                        ELEVATE_R_2 = true;

                                        break;

                                    case "ELEVATE_STOP_2":

                                        ELEVATE_L_2 = false;
                                        ELEVATE_R_2 = false;

                                        break;

                                    case "MOVE_FORWARD_2":

                                        MOVE_L_2 = true;
                                        MOVE_R_2 = false;

                                        break;

                                    case "MOVE_BACKWARD_2":

                                        MOVE_L_2 = false;
                                        MOVE_R_2 = true;

                                        break;

                                    case "MOVE_STOP_2":

                                        MOVE_L_2 = false;
                                        MOVE_R_2 = false;

                                        break;

                                    default:
                                        // 处理其他值或未赋值的情况
                                        break;
                                }
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
        //解压字符串
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
