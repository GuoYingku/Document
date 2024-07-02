using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;


namespace ShenYangRemoteSystem
{
    /// <summary>
    /// 通过Modbus协议与PLC进行通信，读写指定起始地址的PLC寄存器数据
    /// 兼容西门子PLC、施耐德PLC
    /// </summary>
    public class ModbusHelper
    {
        /// <summary>
        /// 与PLC通信的socket客户端
        /// </summary>
        public Socket socket;
        /// <summary>
        /// 是否已连接上PLC，true：已连接上PLC false：未连接
        /// </summary>
        public bool isConnectPLC = false;
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectPLC(string ip, int port)
        {
            isConnectPLC = false;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult asyncResult = socket.BeginConnect(ip, port, CallbackConnect, socket);
                asyncResult.AsyncWaitHandle.WaitOne(2000);
                socket.ReceiveTimeout = 2000;//2000ms无数据接收则超时
                return isConnectPLC;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("连接Modbus_TCP失败：" + ex.Message);
                System.Diagnostics.Debug.WriteLine("连接Modbus_TCP失败：" + ex.Message);

                DisplayRichTextboxContentAndScrollEvent?.Invoke("连接Modbus_TCP失败：" + ex.Message);

                isConnectPLC = false;
            }
            return isConnectPLC;
        }

        /// <summary>
        /// 异步连接PLC
        /// </summary>
        /// <param name="ar"></param>
        private void CallbackConnect(IAsyncResult ar)
        {
            isConnectPLC = false;
            try
            {
                Socket skt = ar.AsyncState as Socket;
                skt.EndConnect(ar);
                isConnectPLC = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("连接Modbus_TCP失败：" + ex.Message);
                System.Diagnostics.Debug.WriteLine("连接Modbus_TCP失败：" + ex.Message);

                DisplayRichTextboxContentAndScrollEvent?.Invoke("连接Modbus_TCP失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 关闭套接字连接
        /// </summary>
        public void CloseConnect()
        {
            if (this.socket != null)
            {
                try
                {
                    this.socket.Close(1000);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写一个指定起始地址的Modbus值。写入【sbyte,byte,short,ushort】使用【0x06指令码：写单个寄存器】。
        /// 写入【int，uint，long，ulong，float，double】使用【0x10指令码：写多个寄存器】
        /// </summary>
        /// <typeparam name="T">基本的数据类型，如short，int，double等</typeparam>
        /// <param name="startAddress">寄存器起始地址，范围：【0x0000~0xFFFF】</param>
        /// <param name="value">写入的值</param>
        /// <returns>true：写入成功 false：写入失败</returns>
        public bool WriteValue<T>(int startAddress, T value)
        {
            try
            {


                if (socket == null || !socket.Connected)
                {
                    System.Diagnostics.Debug.WriteLine("socket为空或者尚未建立与PLC_Modbus的连接...");

                    DisplayRichTextboxContentAndScrollEvent?.Invoke("socket为空或者尚未建立与PLC_Modbus的连接...");

                    return false;
                }
                if (startAddress < 0 || startAddress > 65535)
                {
                    System.Diagnostics.Debug.WriteLine("Modbus的起始地址必须在0~65535之间");

                    DisplayRichTextboxContentAndScrollEvent?.Invoke("Modbus的起始地址必须在0~65535之间");

                    return false;
                }
                byte[] addrArray = BitConverter.GetBytes((ushort)startAddress);
                //sbyte，byte，short，ushort 占用一个寄存器（Word）范围的可以使用功能码0x06：写单个寄存器
                //int,long,float,double 需要使用两个或两个以上寄存器，因此只能使用功能码0x10：写多个寄存器 其中int，uint，float占用两个寄存器 long，ulong，double占用四个寄存器
                byte[] buffer = new byte[12] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x06, addrArray[1], addrArray[0], 0x00, 0x00 };
                if (typeof(T) == typeof(sbyte))
                {
                    sbyte sb = Convert.ToSByte(value);
                    byte b = (byte)sb;
                    buffer[11] = b;
                }
                else if (typeof(T) == typeof(byte))
                {
                    byte b = Convert.ToByte(value);
                    buffer[11] = b;
                }
                else if (typeof(T) == typeof(short))
                {
                    short s = Convert.ToInt16(value);
                    byte[] writeValueArray = BitConverter.GetBytes(s);
                    buffer[10] = writeValueArray[1];
                    buffer[11] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(ushort))
                {
                    ushort us = Convert.ToUInt16(value);
                    byte[] writeValueArray = BitConverter.GetBytes(us);
                    buffer[10] = writeValueArray[1];
                    buffer[11] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(int))
                {
                    int i = Convert.ToInt32(value);
                    byte[] writeValueArray = BitConverter.GetBytes(i);
                    buffer = new byte[17] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0B, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[3];
                    buffer[14] = writeValueArray[2];
                    buffer[15] = writeValueArray[1];
                    buffer[16] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(uint))
                {
                    uint ui = Convert.ToUInt32(value);
                    byte[] writeValueArray = BitConverter.GetBytes(ui);
                    buffer = new byte[17] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0B, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[3];
                    buffer[14] = writeValueArray[2];
                    buffer[15] = writeValueArray[1];
                    buffer[16] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(long))
                {
                    long l = Convert.ToInt64(value);
                    byte[] writeValueArray = BitConverter.GetBytes(l);
                    buffer = new byte[21] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0F, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[7];
                    buffer[14] = writeValueArray[6];
                    buffer[15] = writeValueArray[5];
                    buffer[16] = writeValueArray[4];
                    buffer[17] = writeValueArray[3];
                    buffer[18] = writeValueArray[2];
                    buffer[19] = writeValueArray[1];
                    buffer[20] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(ulong))
                {
                    ulong ul = Convert.ToUInt64(value);
                    byte[] writeValueArray = BitConverter.GetBytes(ul);
                    buffer = new byte[21] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0F, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[7];
                    buffer[14] = writeValueArray[6];
                    buffer[15] = writeValueArray[5];
                    buffer[16] = writeValueArray[4];
                    buffer[17] = writeValueArray[3];
                    buffer[18] = writeValueArray[2];
                    buffer[19] = writeValueArray[1];
                    buffer[20] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(float))
                {
                    float f = Convert.ToSingle(value);
                    byte[] writeValueArray = BitConverter.GetBytes(f);
                    buffer = new byte[17] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0B, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[3];
                    buffer[14] = writeValueArray[2];
                    buffer[15] = writeValueArray[1];
                    buffer[16] = writeValueArray[0];
                }
                else if (typeof(T) == typeof(double))
                {
                    double d = Convert.ToDouble(value);
                    byte[] writeValueArray = BitConverter.GetBytes(d);
                    buffer = new byte[21] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x0F, 0x01, 0x10, addrArray[1], addrArray[0], 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    buffer[13] = writeValueArray[7];
                    buffer[14] = writeValueArray[6];
                    buffer[15] = writeValueArray[5];
                    buffer[16] = writeValueArray[4];
                    buffer[17] = writeValueArray[3];
                    buffer[18] = writeValueArray[2];
                    buffer[19] = writeValueArray[1];
                    buffer[20] = writeValueArray[0];
                }
                else
                {
                    //暂不考虑 char(就是ushort，两个字节)，decimal（十六个字节）等类型
                    System.Diagnostics.Debug.WriteLine("写Modbus数据暂不支持其他类型：" + value.GetType());

                    DisplayRichTextboxContentAndScrollEvent?.Invoke("写Modbus数据暂不支持其他类型：");

                    return false;
                }
                try
                {
                    socket.Send(buffer);
                    DisplayBuffer(buffer, buffer.Length, true);
                    Thread.Sleep(50);//等待50ms
                    byte[] receiveBuffer = new byte[1024];
                    int receiveCount = socket.Receive(receiveBuffer);
                    DisplayBuffer(receiveBuffer, receiveCount, false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                    DisplayRichTextboxContentAndScrollEvent?.Invoke("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                    return false;
                }
                return true;

            }
            catch (Exception e) { MessageBox.Show(e.ToString()); return false; }

        }

        /// <summary>
        /// 从一个指定的起始寄存器地址开始，顺次连续写入字节流【写任意多个寄存器】
        /// </summary>
        /// <param name="startAddress">起始地址</param>
        /// <param name="buffer">要写入的字节数组，buffer数组长度范围：【1~240（0x01~0xF0）】</param>
        /// <returns>true：写入成功 false：写入失败</returns>
        public bool WriteValue(int startAddress, byte[] buffer)
        {
            //分奇数个字节、偶数个字节
            if (socket == null || !socket.Connected)
            {
                System.Diagnostics.Debug.WriteLine("socket为空或者尚未建立与PLC_Modbus的连接...");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("socket为空或者尚未建立与PLC_Modbus的连接...");

                return false;
            }
            if (startAddress < 0 || startAddress > 65535)
            {
                System.Diagnostics.Debug.WriteLine("Modbus的起始地址必须在0~65535之间");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("Modbus的起始地址必须在0~65535之间");

                return false;
            }
            if (buffer == null || buffer.Length < 1 || buffer.Length > 240)
            {
                System.Diagnostics.Debug.WriteLine("写连续寄存器块范围：(1 至120 个寄存器)");//每个寄存器将数据分成两字节

                DisplayRichTextboxContentAndScrollEvent?.Invoke("写连续寄存器块范围：(1 至120 个寄存器)");

                return false;
            }
            byte[] addrArray = BitConverter.GetBytes((ushort)startAddress);
            //需要写入的寄存器个数
            byte registerCount = (byte)((buffer.Length + 1) / 2);
            //实际写入的字节个数：注意buffer数组的长度为奇数时 需要将最后一个寄存器的高位设置为0
            byte writeCount = (byte)(registerCount * 2);
            byte[] sendBuffer = new byte[13 + writeCount];
            sendBuffer[0] = 0x02;
            sendBuffer[1] = 0x01;
            sendBuffer[5] = (byte)(7 + writeCount);
            sendBuffer[6] = 0x01;
            sendBuffer[7] = 0x10;
            sendBuffer[8] = addrArray[1];
            sendBuffer[9] = addrArray[0];
            sendBuffer[11] = registerCount;
            sendBuffer[12] = writeCount;
            for (int i = 0; i < writeCount - 2; i++)
            {
                sendBuffer[13 + i] = buffer[i];
            }

            //最后两个元素[最后的一个寄存器]的处理
            if (buffer.Length % 2 == 1)
            {
                //如果是奇数个，需要将最后一个寄存器的高位设置为0
                sendBuffer[13 + writeCount - 2] = 0;
                sendBuffer[13 + writeCount - 1] = buffer[buffer.Length - 1];
            }
            else
            {
                //如果是偶数个，则一一对应
                sendBuffer[13 + writeCount - 2] = buffer[buffer.Length - 2];
                sendBuffer[13 + writeCount - 1] = buffer[buffer.Length - 1];
            }

            try
            {
                socket.Send(sendBuffer);
                DisplayBuffer(sendBuffer, sendBuffer.Length, true);
                Thread.Sleep(50);//等待50ms
                byte[] receiveBuffer = new byte[1024];
                int receiveCount = socket.Receive(receiveBuffer);
                DisplayBuffer(receiveBuffer, receiveCount, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                DisplayRichTextboxContentAndScrollEvent?.Invoke("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                return false;
            }
            return true;
        }



        #region 重要委托和事件

        //原代码
        //private void DisplayRichTextboxContentAndScroll(string addContent)
        //{
        //    this.Invoke(new MethodInvoker(() =>
        //    {
        //        rtxtDisplay.AppendText(addContent + "\n");
        //        rtxtDisplay.ScrollToCaret();
        //    }));
        //}

        //Form1 form1 = new Form1();

        public delegate void DisplayRichTextboxContentAndScrollHandler(string addContent);
        public event DisplayRichTextboxContentAndScrollHandler DisplayRichTextboxContentAndScrollEvent;

        #endregion



        /// <summary>
        /// 读取指定起始地址的值
        /// </summary>
        /// <typeparam name="T">基本的数据类型，如short，int，double等</typeparam>
        /// <param name="startAddress">起始地址</param>
        /// <param name="value">返回的具体指</param>
        /// <returns>true：读取成功 false：读取失败</returns>
        public bool ReadValue<T>(int startAddress, out T value)
        {
            value = default(T);
            if (socket == null || !socket.Connected)
            {
                System.Diagnostics.Debug.WriteLine("socket为空或者尚未建立与PLC_Modbus的连接...");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("socket为空或者尚未建立与PLC_Modbus的连接...");
                //DisplayRichTextboxContentAndScroll("socket为空或者尚未建立与PLC_Modbus的连接...");

                return false;
            }
            if (startAddress < 0 || startAddress > 65535)
            {
                System.Diagnostics.Debug.WriteLine("Modbus的起始地址必须在0~65535之间");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("Modbus的起始地址必须在0~65535之间");

                return false;
            }
            byte[] addrArray = BitConverter.GetBytes((ushort)startAddress);
            byte wordLength = 0;//读取的地址个数【多少个字Word】 int，float需要两个字 long,double需要四个字
            if (typeof(T) == typeof(sbyte) || typeof(T) == typeof(byte) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort))
            {
                wordLength = 1;
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(uint) || typeof(T) == typeof(float))
            {
                wordLength = 2;
            }
            else if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(double))
            {
                wordLength = 4;
            }
            else
            {
                //暂不考虑 char(就是ushort，两个字节)，decimal（十六个字节）等类型
                System.Diagnostics.Debug.WriteLine("读Modbus数据暂不支持其他类型：" + value.GetType());
                return false;
            }
            byte[] sendBuffer = new byte[12] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, addrArray[1], addrArray[0], 0x00, wordLength };
            //try
            //{
            socket.Send(sendBuffer);
            //}
            //catch (Exception e)
            //{
            //DisplayRichTextboxContentAndScrollEvent?.Invoke(e.ToString());
            //DisplayRichTextboxContentAndScrollEvent?.Invoke("1");
            //}

            DisplayBuffer(sendBuffer, sendBuffer.Length, true);
            Thread.Sleep(50);//等待50ms

            byte[] receiveBuffer = new byte[1024];
            try
            {
                //协议错误时 Receive函数将发生异常
                int receiveCount = socket.Receive(receiveBuffer);
                DisplayBuffer(receiveBuffer, receiveCount, false);
                //receiveBuffer[8] : 真实数据的字节流总个数
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                DisplayRichTextboxContentAndScrollEvent?.Invoke("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                return false;
            }

            if (typeof(T) == typeof(sbyte))
            {
                byte b = receiveBuffer[10];
                sbyte sb = (sbyte)b;
                value = (T)(object)sb;
            }
            else if (typeof(T) == typeof(byte))
            {
                byte b = receiveBuffer[10];
                value = (T)(object)b;
            }
            else if (typeof(T) == typeof(short))
            {
                short s = BitConverter.ToInt16(new byte[] { receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)s;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ushort us = BitConverter.ToUInt16(new byte[] { receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)us;
            }
            else if (typeof(T) == typeof(int))
            {
                int i = BitConverter.ToInt32(new byte[] { receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)i;
            }
            else if (typeof(T) == typeof(uint))
            {
                uint ui = BitConverter.ToUInt32(new byte[] { receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)ui;
            }
            else if (typeof(T) == typeof(long))
            {
                long l = BitConverter.ToInt64(new byte[] { receiveBuffer[16], receiveBuffer[15], receiveBuffer[14], receiveBuffer[13], receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)l;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ulong ul = BitConverter.ToUInt64(new byte[] { receiveBuffer[16], receiveBuffer[15], receiveBuffer[14], receiveBuffer[13], receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)ul;
            }
            else if (typeof(T) == typeof(float))
            {
                float f = BitConverter.ToSingle(new byte[] { receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)f;
            }
            else if (typeof(T) == typeof(double))
            {
                double d = BitConverter.ToDouble(new byte[] { receiveBuffer[16], receiveBuffer[15], receiveBuffer[14], receiveBuffer[13], receiveBuffer[12], receiveBuffer[11], receiveBuffer[10], receiveBuffer[9] }, 0);
                value = (T)(object)d;
            }
            return true;
        }

        /// <summary>
        /// 从起始地址开始，读取一段字节流
        /// </summary>
        /// <param name="startAddress">起始寄存器地址</param>
        /// <param name="length">读取的字节个数</param>
        /// <param name="value">返回的字节流数据</param>
        /// <returns>true：读取成功 false：读取失败</returns>
        public bool ReadValue(int startAddress, int length, out byte[] value)
        {
            value = new byte[length];
            if (socket == null || !socket.Connected)
            {
                System.Diagnostics.Debug.WriteLine("socket为空或者尚未建立与PLC_Modbus的连接...");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("socket为空或者尚未建立与PLC_Modbus的连接...");

                return false;
            }
            //读保持寄存器0x03读取的寄存器数量的范围为 1~125。因一个寄存器【一个Word】存放两个字节，因此 字节数组的长度范围 为 1~250
            if (length < 1 || length > 250)
            {
                System.Diagnostics.Debug.WriteLine("返回的字节数组的长度范围为 1~250");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("返回的字节数组的长度范围为 1~250");

                return false;
            }
            if (startAddress < 0 || startAddress > 65535)
            {
                System.Diagnostics.Debug.WriteLine("Modbus的起始地址必须在0~65535之间");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("Modbus的起始地址必须在0~65535之间");

                return false;
            }
            byte[] addrArray = BitConverter.GetBytes((ushort)startAddress);
            //读取的寄存器个数： 如果length为偶数 则为 length/2 如果length为奇数，则为(length+1)/2。因整数相除，结果不考虑余数，所以如下通用：
            byte registerCount = (byte)((length + 1) / 2);
            byte[] sendBuffer = new byte[12] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, addrArray[1], addrArray[0], 0x00, registerCount };
            socket.Send(sendBuffer);

            DisplayBuffer(sendBuffer, sendBuffer.Length, true);
            Thread.Sleep(50);//等待50ms

            byte[] receiveBuffer = new byte[1024];
            try
            {
                int receiveCount = socket.Receive(receiveBuffer);
                DisplayBuffer(receiveBuffer, receiveCount, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                DisplayRichTextboxContentAndScrollEvent?.Invoke("接收Modbus的响应数据异常，请查看发送的报文格式是否有误：" + ex.Message);

                return false;
            }
            //接收到的实际数据字节个数
            byte receiveLength = receiveBuffer[8];
            if (receiveLength != registerCount * 2)
            {
                System.Diagnostics.Debug.WriteLine("解析接收数据非法，接收的实际数据长度【不是】读取寄存器数量的2倍");

                DisplayRichTextboxContentAndScrollEvent?.Invoke("解析接收数据非法，接收的实际数据长度【不是】读取寄存器数量的2倍");

                return false;
            }
            value = new byte[receiveLength];
            for (int i = 0; i < receiveLength; i++)
            {
                value[i] = receiveBuffer[9 + i];
            }
            return true;
        }

        /// <summary>
        /// 读取起始地址开始存储的条码，默认读取最大长度为100的条码字符串
        /// </summary>
        /// <param name="startAddress">起始地址</param>
        /// <param name="barcode">返回的条码字符串</param>
        /// <returns>true：读取成功 false：读取失败</returns>
        public bool ReadBarcode(int startAddress, out string barcode)
        {
            barcode = string.Empty;
            byte[] dataBuffer = new byte[100];
            bool result = ReadValue(startAddress, 100, out dataBuffer);
            if (!result)
            {
                return false;
            }
            List<byte> list = new List<byte>();
            for (int i = 0; i < dataBuffer.Length; i += 2)
            {
                //因一个寄存器存储的数据 是一个字Word，分成两个字节Byte【高位字节、低位字节】，存储的条码是低位在前，因此每隔两个需要交换顺序
                list.Add(dataBuffer[i + 1]);
                list.Add(dataBuffer[i]);
                //遇到'\0'后面的数据无效
                if (dataBuffer[i] == 0 || dataBuffer[i + 1] == 0)
                {
                    break;
                }
            }
            byte[] actualBuffer = list.ToArray();
            barcode = Encoding.ASCII.GetString(actualBuffer).Trim('\0').Trim();
            return result;
        }

        /// <summary>
        /// 打印Debug发送或接收字节数组信息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <param name="isSend"></param>
        public void DisplayBuffer(byte[] buffer, int count, bool isSend)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((isSend ? "发送" : "接收到") + "的字节流：\n");
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(buffer[i].ToString("X2"));
            }
            string content = sb.ToString();
            System.Diagnostics.Debug.WriteLine(content);

            DisplayRichTextboxContentAndScrollEvent?.Invoke(content.ToString());
        }
    }
}
/*

Modbus报文协议格式【只考虑读写保持寄存器 0x03：读多个保持寄存器 0x06：写单个保持寄存器 0x10：写多个保持寄存器】

PLC中，每个寄存器将数据分成两个字节，即一个寄存器【一个字Word】地址占用两个字节Byte，寄存器地址范围：【0x0000~0xFFFF】

注意：PLC存储数据是高位在前，如十进制的1000，在PLC中存储字节顺序是：0x03 0xE8。而在C#中存储字节顺序是低位在前：如 BitConverter.GetBytes((short)1000)，是 0xE8 0x03
①发送的请求【读多个保持寄存器 0x03】Modbus字节流说明：
* byte[0] byte[1] 随便指定，PLC返回的前两个字节完全一致
* byte[2]=0 byte[3]=0 固定为0 代表Modbus标识
* byte[4] byte[5] 排在byte[5]后面所有字节的个数，也就是总长度6
* byte[6] 站号，随便指定，00–FF都可以,PLC返回的保持一致
* byte[7] 功能码，读保持寄存器 0x03：
* byte[8] byte[9] 起始地址，如起始地址为整数20 则为 0x00 0x14，再如起始地址为整数1000，则为 0x03 0xE8
* byte[10] byte[11] 寄存器个数【Word】，读取的数据长度【以字Word为单位】，读取Int32或Float就是两个字，读取byte或short就是一个字 范围【1~125 即 0x0001~0x007D】
*
* 接收的内容解析：【读多个保持寄存器 0x03】【Modbus响应】
* byte[0] byte[1] 与发送的一致
* byte[2]=0 byte[3]=0 固定为0 代表Modbus标识
* byte[4] byte[5] 排在byte[5]后面所有字节的个数
* byte[6] 站号，与发送的一致
* byte[7] 功能码，与发送的一致
* byte[8] 表示byte[8]后面跟随的字节数【发送的寄存器个数 * 2】
* byte[9] byte[10] byte[11] byte[12] byte[…] 真实数据的字节流，字节流的总个数就是byte[8]

②发送的请求【写单个保持寄存器 0x06】Modbus字节流说明： 【0x06只能写入一个16位【short】数据,无法写入Int32或Float】
* byte[0] byte[1] 随便指定，PLC返回的前两个字节完全一致
* byte[2]=0 byte[3]=0 固定为0 代表Modbus标识
* byte[4] byte[5] 排在byte[5]后面所有字节的个数
* byte[6] 站号，随便指定，00–FF都可以,PLC返回的保持一致
* byte[7] 功能码，0x06：写单个保持寄存器
* byte[8] byte[9] 起始地址，如起始地址为整数20 则为 0x00 0x14，再如起始地址为整数1000，则为 0x03 0xE8
* byte[10] byte[11] 写入的值【只能写入一个字Word】
*
* 接收的内容【写单个保持寄存器的响应内容 0x06】与发送的内容完全一致

③发送的请求【写多个保持寄存器 0x10】Modbus字节流说明：【写Int32，UInt32，Float需要两个寄存器 写Int64，UInt64，Double需要四个寄存器】
* byte[0] byte[1] 随便指定，PLC返回的前两个字节完全一致
* byte[2]=0 byte[3]=0 固定为0 代表Modbus标识 随便指定也可以
* byte[4] byte[5] 排在byte[5]后面所有字节的个数
* byte[6] 站号，随便指定，00–FF都可以,PLC返回的保持一致
* byte[7] 功能码，0x10：写多个保持寄存器
* byte[8] byte[9] 起始地址，如起始地址为整数20 则为 0x00 0x14，再如起始地址为整数1000，则为 0x03 0xE8
* byte[10] byte[11] 寄存器数量【设置长度】，范围1~120【0x78】，因此byte[10]=0, byte[11]为寄存器数量
* byte[12] 字节个数 也就是【寄存器数量*2】，范围【2~240】
* byte[13] byte[14] byte[15] byte[16] byte[…] 具体的数据内容 对应 数据一高位 数据一低位 数据二高位 数据二低位
*
* 【写多个保持寄存器 0x10】Modbus响应结果
* byte[0] byte[1] 随便指定，PLC返回的前两个字节完全一致
* byte[2]=0 byte[3]=0 固定为0 代表Modbus标识
* byte[4] byte[5] 排在byte[5]后面所有字节的个数
* byte[6] 站号，随便指定，00–FF都可以,PLC返回的保持一致
* byte[7] 功能码，0x10：写多个保持寄存器
* byte[8] byte[9] 起始地址
* byte[10] byte[11] 寄存器数量【设置长度】
*/
