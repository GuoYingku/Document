using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace ShenYangRemoteSystem.Subclass
{
    /// <summary>
    /// 通过S7.Net库与西门子PLC进行通信，读写指定地址的PLC寄存器数据
    /// 仅支持西门子PLC
    /// </summary>
    public class SiemensHelper
    {
        /// <summary>
        /// plc连接对象
        /// </summary>
        public Plc plc;

        /// <summary>
        /// 主窗体对象
        /// </summary>
        Form1 form1;

        public SiemensHelper(){}

        /// <summary>
        /// SiemensHelper类的有参构造函数
        /// </summary>
        /// /// <param name="form">Form1的对象</param>
        public SiemensHelper(Form1 form)
        {
            form1 = form;


            Thread thread1 = new Thread(new ThreadStart(Process));

            thread1.Start();
        }


        #region PLC连接状态检测线程
        private void Process()
        {
            //while (true)
            //{
            //    try
            //    {
            //        if (plc != null)
            //        {
            //            if (plc.IsConnected == true)
            //            {
            //                //不在UI线程中，需要使用委托
            //                if (Form1.uc5.PlcConnectionLabel != null)
            //                    Form1.uc5.PlcConnectionLabel.Invoke(new MethodInvoker(() =>
            //                    {
            //                        form1.UpdateText(Form1.uc5.PlcConnectionLabel, "PLC1已连接！");
            //                    }));
            //            }
            //            else
            //            {
            //                plc.Open();
            //            }
            //        }
            //    }
            //    catch { }

            //    Thread.Sleep(5000);
            //}
        }
        #endregion

        #region PLC读取方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public object PLCRead(string address)
        {
            object result = null;
            object data;

            //检查plc的可用性，检查此属性时,回向plc发送ping命令，如果plc响应ping则返回true，否则返回false.
            if (plc.IsConnected)
            {
                object bytes = plc.Read(address);

                Type dataType = bytes.GetType();

                switch (dataType.ToString())
                {
                    case "System.Bool":
                        data = Convert.ToBoolean(bytes);
                        break;
                    default:
                        data = bytes;
                        break;
                }
                result = data;
            }
            else
            {
                MessageBox.Show("PLC未连接");
            }
            return result;
        }
        #endregion

        #region PLC写入方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="datatype"></param>
        /// <param name="data"></param>
        public void PLCWrite(string address, int datatype, string data)
        {
            try
            {
                if (plc.IsConnected)
                {
                    switch (datatype)
                    {
                        //Bit
                        case 0:
                            plc.Write(address, Convert.ToBoolean(data));
                            break;
                        //Byte
                        case 1:
                            plc.Write(address, Convert.ToByte(data));
                            break;
                        //Word
                        case 2:
                            plc.Write(address, Convert.ToUInt16(data));
                            break;
                        //DWord
                        case 3:
                            plc.Write(address, Convert.ToUInt32(data));
                            break;
                        //Int
                        case 4:
                            plc.Write(address, Convert.ToUInt16(data));
                            break;
                        //DInt
                        case 5:
                            plc.Write(address, Convert.ToUInt32(data));
                            break;
                        //Real
                        case 6:
                            plc.Write(address, Convert.ToSingle(data));
                            break;
                        default:
                            MessageBox.Show("暂时未涉及数据类型。");
                            break;
                    }
                }
                else { MessageBox.Show("连接失败：未成功建立连接"); }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion


    }
}