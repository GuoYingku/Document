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
            Thread thread1 = new Thread(new ThreadStart(Uc5LifeProcess));

            //thread1.Start();

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
            //modbusHelper_D1PLC1 = Form1.modbusHelper_D1PLC1;
            //modbusHelper_D1PLC2 = Form1.modbusHelper_D1PLC2;
            //modbusHelper_D2PLC1 = Form1.modbusHelper_D2PLC1;
            //modbusHelper_D2PLC2 = Form1.modbusHelper_D2PLC2;


            modbusHelper_D1PLC1 = new ModbusHelper(this);
            modbusHelper_D1PLC2 = new ModbusHelper(this);
            modbusHelper_D2PLC1 = new ModbusHelper(this);
            modbusHelper_D2PLC2 = new ModbusHelper(this);


            //modbusHelper.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;
            //modbusHelper2.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;
        }


        List<Thread> connectedThreads = new List<Thread>(); // 用于存储已连接的线程


        public ModbusHelper modbusHelper_D1PLC1;//施耐德D1PLC1对象
        public ModbusHelper modbusHelper_D1PLC2;//施耐德D1PLC2对象
        public ModbusHelper modbusHelper_D2PLC1;//施耐德D2PLC1对象
        public ModbusHelper modbusHelper_D2PLC2;//施耐德D2PLC2对象


        #region PLC对象映射线程 （施耐德）
        private void Uc5LifeProcess()
        {
            while (true)
            {
                //D1PLC1
                try
                {
                    if (Form1.modbusHelper_D1PLC1.socket != null)
                    {
                        modbusHelper_D1PLC1 = Form1.modbusHelper_D1PLC1;

                        if (modbusHelper_D1PLC1.isConnectPLC != true)
                        {
                            //modbusHelper_D1PLC1.ConnectPLC();
                        }
                    }
                }
                catch { }
                Thread.Sleep(500);

                //D1PLC2
                try
                {
                    if (Form1.modbusHelper_D1PLC2.socket != null)
                    {
                        modbusHelper_D1PLC2 = Form1.modbusHelper_D1PLC2;

                        if (modbusHelper_D1PLC2.isConnectPLC != true)
                        {
                            //modbusHelper_D1PLC2.ConnectPLC();
                        }
                    }
                }
                catch { }
                Thread.Sleep(500);

                //D2PLC1
                try
                {
                    if (Form1.modbusHelper_D2PLC1.socket != null)
                    {
                        modbusHelper_D2PLC1 = Form1.modbusHelper_D2PLC1;

                        if (modbusHelper_D2PLC1.isConnectPLC != true)
                        {
                            //modbusHelper_D2PLC1.ConnectPLC();
                        }
                    }
                }
                catch { }
                Thread.Sleep(500);

                //D2PLC2
                try
                {
                    if (Form1.modbusHelper_D2PLC2.socket != null)
                    {
                        modbusHelper_D2PLC2 = Form1.modbusHelper_D2PLC2;

                        if (modbusHelper_D2PLC2.isConnectPLC != true)
                        {
                            //modbusHelper_D2PLC2.ConnectPLC();
                        }
                    }
                }
                catch { }
                Thread.Sleep(500);
            }
        }
        #endregion

        #region 手动连接按钮 （施耐德）
        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;

            string ip = txtIP.Text;
            int port = int.Parse(txtPort.Text);

            if (modbusHelper_D1PLC1.ConnectPLC(ip, port))
            {
                DisplayRichTextboxContentAndScroll("连接Modbus_TCP成功");
            }
            else
            {
                DisplayRichTextboxContentAndScroll("连接Modbus_TCP失败,请检查网络以及IP设置");

                btnConnect.Enabled = true;
            }
        }
        #endregion

        #region 手动单个读取 （施耐德）
        private void btnRead_Click(object sender, EventArgs e)
        {
            //try
            //{
            int startAddress = int.Parse(txtStartAddress.Text);
            string curType = cboDataType.Text;
            //try
            //{
            switch (curType)
            {
                case "Byte(Byte)":
                    byte b;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<byte>(startAddress, out b);
                    txtValue.Text = b.ToString();
                    break;
                case "Int16(Int)":
                    short s;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<short>(startAddress, out s);
                    txtValue.Text = s.ToString();
                    break;
                case "UInt16(Word)":
                    ushort us;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<ushort>(startAddress, out us);
                    txtValue.Text = us.ToString();
                    break;
                case "Int32(DInt)":
                    int i;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<int>(startAddress, out i);
                    txtValue.Text = i.ToString();
                    break;
                case "UInt32(DWord)":
                    uint ui;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<uint>(startAddress, out ui);
                    txtValue.Text = ui.ToString();
                    break;
                case "Float(Real)":
                    float f;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<float>(startAddress, out f);
                    txtValue.Text = f.ToString();
                    break;
                case "Int64":
                    long l;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<long>(startAddress, out l);
                    txtValue.Text = l.ToString();
                    break;
                case "UInt64":
                    ulong ul;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<ulong>(startAddress, out ul);
                    txtValue.Text = ul.ToString();
                    break;
                case "Double":
                    double d;
                    modbusHelper_D1PLC1.ReadSingleHoldingRegisterValue<double>(startAddress, out d);
                    txtValue.Text = d.ToString();
                    break;

                default:
                    DisplayRichTextboxContentAndScroll("未选择数据类型\n");
                    break;
            }
            //}
            //    catch (Exception ex)
            //    {
            //        DisplayRichTextboxContentAndScroll("写寄存器出现错误，请检查：\n" + ex.Message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DisplayRichTextboxContentAndScroll(ex.Message);
            //}
        }
        #endregion

        #region 手动单个写入 （施耐德）
        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                int startAddress = int.Parse(txtStartAddress2.Text);
                string curType = cboDataType2.Text;
                try
                {
                    switch (curType)
                    {
                        case "Byte(Byte)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<byte>(startAddress, byte.Parse(txtValue2.Text));
                            break;
                        case "Int16(Int)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<short>(startAddress, short.Parse(txtValue2.Text));
                            break;
                        case "UInt16(Word)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<ushort>(startAddress, ushort.Parse(txtValue2.Text));
                            break;
                        case "Int32(DInt)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<int>(startAddress, int.Parse(txtValue2.Text));
                            break;
                        case "UInt32(DWord)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<uint>(startAddress, uint.Parse(txtValue2.Text));
                            break;
                        case "Float(Real)":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<float>(startAddress, float.Parse(txtValue2.Text));
                            break;
                        case "Int64":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<long>(startAddress, long.Parse(txtValue2.Text));
                            break;
                        case "UInt64":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<ulong>(startAddress, ulong.Parse(txtValue2.Text));
                            break;
                        case "Double":
                            modbusHelper_D1PLC1.WriteSingleHoldingRegisterValue<double>(startAddress, double.Parse(txtValue2.Text));
                            break;

                        default:
                            DisplayRichTextboxContentAndScroll("未选择数据类型\n");
                            break;
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

            UserControl_Page5 uc5 = Form1.uc5;

            Form1.mySqlConnection.Close();

            UpdateText(uc3.MySqlConnectionLabel, "已断开");

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
