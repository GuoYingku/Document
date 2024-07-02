using S7.Net;
using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShenYangRemoteSystem.Form1;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page5 : UserControl
    {
        public UserControl_Page5()
        {
            InitializeComponent();
        }

        private void UserControl_Page5_Load(object sender, EventArgs e)
        {
            siemens1 = siemensHelper;
            siemens2 = siemensHelper;

            Thread thread1 = new Thread(new ThreadStart(Process));

            thread1.Start();

            connectedThreads.Add(thread1); // 将新连接的线程添加到列表中

            // 如果线程数超过一个，则依据FIFO关闭线程
            if (connectedThreads.Count > 1)
            {
                Thread earliestThread = connectedThreads[0];
                connectedThreads.RemoveAt(0);
                try
                {
                    earliestThread.Abort();
                }
                catch { }
            }

            //施耐德
            //modbusHelper = Form1.modbusHelper;
            //modbusHelper2 = Form1.modbusHelper2;


            //modbusHelper.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;
            //modbusHelper2.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;
        }


        public SiemensHelper siemens1;
        public SiemensHelper siemens2;
        List<Thread> connectedThreads = new List<Thread>(); // 用于存储已连接的线程

        //public ModbusHelper modbusHelper;
        //public ModbusHelper modbusHelper2;

        #region PLC对象映射线程 （西门子）
        private void Process()
        {
            while (true)
            {
                //PLC1
                try
                {
                    if (siemensHelper.plc != null)
                    {
                        siemens1 = siemensHelper;

                        if (siemens1.plc.IsConnected != true)
                        {
                            //siemens1.plc.Open();
                        }
                    }
                }
                catch { }

                //PLC2
                try
                {
                    if (siemensHelper2.plc != null)
                    {
                        siemens2 = siemensHelper2;

                        if (siemens2.plc.IsConnected != true)
                        {
                            //siemens2.plc.Open();
                        }
                    }
                }
                catch { }

                Thread.Sleep(5000);
            }
        }
        #endregion

        #region 手动连接按钮 （西门子）
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //string TypeName = txtPLCType.Text;//PLC型号名称
            string TypeName = "1500";//PLC型号名称
            string Ip = txtIP.Text;//plcIp地址

            if (label1.Text != "PLC1已连接！")
            {
                try
                {
                    if (siemensHelper.plc == null)
                    {
                        switch (TypeName)
                        {
                            case "200":
                            case "200s":
                                /*S7.Net.dll类库中没有S7-200Smart类型，想跟S7-200Smart通讯选择S71200即可*/
                                siemensHelper.plc = new Plc(CpuType.S7200Smart, Ip, 0, 1);//创建plc实例
                                break;
                            case "300":
                                siemensHelper.plc = new Plc(CpuType.S7300, Ip, 0, 1);//创建plc实例
                                break;
                            case "400":
                                siemensHelper.plc = new Plc(CpuType.S7400, Ip, 0, 1);//创建plc实例
                                break;
                            case "1500":
                                siemensHelper.plc = new Plc(CpuType.S71500, Ip, 0, 1);//创建plc实例
                                break;
                        }

                        siemensHelper.plc = new Plc(CpuType.S71500, "192.168.1.66", 0, 1);
                        siemensHelper.plc.Open();

                        //检查plc是否连接上
                        if (siemensHelper.plc.IsConnected)
                        {
                            try
                            {
                                if (uc5.PlcConnectionLabel != null)
                                {
                                    UpdateText(uc5.PlcConnectionLabel, "PLC1已连接！");
                                }
                            }

                            catch (Exception ex)
                            {
                                DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                            }
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("PLC1连接失败！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }
            }
        }
        #endregion

        #region 手动单个读取 （西门子）
        private void btnRead_Click(object sender, EventArgs e)
        {
            //try
            //{
            string address = txtStartAddress.Text;//寄存地址

            //检查plc的可用性，检查此属性时,回向plc发送ping命令，如果plc响应ping则返回true，否则返回false.
            if (siemensHelper.plc.IsConnected)
            {
                object bytes = siemensHelper.plc.Read(address);
                //入参：读取数据地址

                Type dataType = bytes.GetType();

                switch (dataType.ToString())
                {
                    case "System.Bool":
                        txtValue.Text = Convert.ToBoolean(bytes).ToString();
                        break;
                    case "System.UInt32":
                        //取浮点数（电流）
                        txtValue.Text = ((uint)bytes).ConvertToFloat().ToString();
                        break;
                    default:
                        //未涉及到的数据类型
                        MessageBox.Show(dataType.ToString());

                        txtValue.Text = bytes.ToString();
                        break;
                }
            }
            else
            {
                DisplayRichTextboxContentAndScroll("PLC1手动单个读取失败！");
            }
        }
        #endregion

        #region 手动单个写入 （西门子）
        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                string address = txtStartAddress2.Text;//寄存地址
                string writeData = txtValue2.Text;//写入数据
                try
                {
                    switch (cboDataType2.SelectedIndex)
                    {
                        //Bit
                        case 0:
                            siemens1.plc.Write(address, Convert.ToBoolean(writeData));
                            break;
                        //Byte
                        case 1:
                            siemens1.plc.Write(address, Convert.ToByte(writeData));

                            break;
                        //Word
                        case 2:
                            siemens1.plc.Write(address, Convert.ToUInt16(writeData));

                            //OutValue = ((ushort)Value).ConvertToShort().ToString();
                            break;
                        //DWord
                        case 3:
                            siemens1.plc.Write(address, Convert.ToUInt32(writeData));

                            //OutValue = ((uint)Value).ConvertToInt().ToString();
                            break;
                        //Int
                        case 4:
                            siemens1.plc.Write(address, Convert.ToUInt16(writeData));

                            //OutValue = ((ushort)Value).ConvertToShort().ToString();
                            break;
                        //DInt
                        case 5:
                            siemens1.plc.Write(address, Convert.ToUInt32(writeData));

                            //OutValue = ((uint)Value).ConvertToInt().ToString();
                            break;
                        //Real
                        case 6:
                            //写小数
                            siemens1.plc.Write(address, Convert.ToSingle(writeData));

                            //OutValue = ((uint)Value).ConvertToDouble().ToString();
                            break;

                        default:
                            DisplayRichTextboxContentAndScroll("未选择数据类型\n");
                            break;


                            //case "Byte(Byte)":
                            //    modbusHelper2.WriteValue<byte>(startAddress, byte.Parse(txtValue2.Text));
                            //    break;
                            //case "Int16(Int)":
                            //    modbusHelper2.WriteValue<short>(startAddress, short.Parse(txtValue2.Text));
                            //    break;
                            //case "UInt16(Word)":
                            //    modbusHelper2.WriteValue<ushort>(startAddress, ushort.Parse(txtValue2.Text));
                            //    break;
                            //case "Int32(DInt)":
                            //    modbusHelper2.WriteValue<int>(startAddress, int.Parse(txtValue2.Text));
                            //    break;
                            //case "UInt32(DWord)":
                            //    modbusHelper2.WriteValue<uint>(startAddress, uint.Parse(txtValue2.Text));
                            //    break;
                            //case "Float(Real)":
                            //    modbusHelper2.WriteValue<float>(startAddress, float.Parse(txtValue2.Text));
                            //    break;
                            //case "Int64":
                            //    modbusHelper2.WriteValue<long>(startAddress, long.Parse(txtValue2.Text));
                            //    break;
                            //case "UInt64":
                            //    modbusHelper2.WriteValue<ulong>(startAddress, ulong.Parse(txtValue2.Text));
                            //    break;
                            //case "Double":
                            //    modbusHelper2.WriteValue<double>(startAddress, double.Parse(txtValue2.Text));
                            //    break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("写寄存器出现错误，请检查：\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                DisplayRichTextboxContentAndScroll(ex.Message);
            }
        }
        #endregion

        #region 调试区逻辑
        /// <summary>
        /// 显示序号 大于100时 就清空文本
        /// </summary>
        public int displaySequence = 0;
        /// <summary>
        /// 显示内容并滚动到当前位置
        /// </summary>
        /// <param name="addContent"></param>
        /// <param name="wd">为null时不添加新行，非空时添加一行</param>
        /// <param name="isNG">是否是有异常的数据航</param>
        public void DisplayRichTextboxContentAndScroll(string addContent)
        {
            try
            {
                displaySequence++;

                while (!this.IsHandleCreated)
                {
                    ;
                }
                this.Invoke(new MethodInvoker(() =>
                {
                    if (displaySequence >= 100)
                    {
                        rtxtDisplay.Clear();
                        displaySequence = 0;
                    }
                    rtxtDisplay.AppendText(addContent + "\n");
                    rtxtDisplay.ScrollToCaret();
                }
                ));
            }
            catch { }
        }
        #endregion

        #region 调试区方法
        public void ModbusHelper_DisplayRichTextboxContentAndScrollEvent(string addContent)
        {
            displaySequence++;
            if (displaySequence >= 100)
            {
                rtxtDisplay.Invoke(new MethodInvoker(() =>
                {
                    rtxtDisplay.Clear();
                }));
                displaySequence = 0;
            }
            rtxtDisplay.Invoke(new MethodInvoker(() =>
            {
                rtxtDisplay.AppendText(addContent + "\n");
                rtxtDisplay.ScrollToCaret();
            }));
        }
        #endregion

        #region 断开连接按钮
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            UserControl_Page3 uc3 = Form1.uc3;
            UserControl_Page4 uc4 = Form1.uc4;
            UserControl_Page5 uc5 = Form1.uc5;

            Form1.mySqlConnection.Close();

            UpdateText(uc3.MySqlConnectionLabel, "已断开");
            UpdateText(uc4.MySqlConnectionLabel, "已断开");
            UpdateText(uc5.MySqlConnectionLabel, "已断开");
        }

        public void UpdateText(Label label, string text)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                label.Text = text;
            }));
        }
        #endregion

    }



















}
