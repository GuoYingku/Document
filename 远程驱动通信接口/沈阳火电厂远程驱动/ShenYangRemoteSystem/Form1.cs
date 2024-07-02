using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ShenYangRemoteSystem.用户控件;
using ShenYangRemoteSystem.Subclass;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Configuration;
using S7.Net;

namespace ShenYangRemoteSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 程序初始化

            siemensHelper = new SiemensHelper(this);
            siemensHelper2 = new SiemensHelper(this);

            //施耐德
            modbusHelper.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;
            modbusHelper2.DisplayRichTextboxContentAndScrollEvent += ModbusHelper_DisplayRichTextboxContentAndScrollEvent;

            systemCommand.QUERY_SYSTEM = "RC"; //本系统ID
            this.FormClosed += Form1_FormClosed; //主窗体退出事件
            //this.WindowState = FormWindowState.Minimized; //初始化窗体显示状态---最小化
            //this.ShowInTaskbar = false; //禁用（隐藏）任务栏图标
            

            Thread thread1 = new Thread(new ThreadStart(ConnectToDatabase));
            Thread thread2 = new Thread(new ThreadStart(Process_SiemensPLC_Main));
            Thread thread3 = new Thread(new ThreadStart(Process_SiemensPLC_DataGet1));
            Thread thread4 = new Thread(new ThreadStart(Process_SiemensPLC_DataGet2));
            Thread thread5 = new Thread(new ThreadStart(Process_SocketListening));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();


            UpdateClock(null, null); //初始化时间
            Page1Btn_Click(null, null); //初始化主界面


            #region UI处理业务
            // 将所有按钮添加到列表中
            buttons = new List<Button> { Page1Btn, Page2Btn, Page3Btn, Page4Btn, Page5Btn };

            // 为每个按钮添加单击事件处理程序
            foreach (Button button in buttons)
            {
                button.Click += ChangeColor;
                button.Tag = button.ForeColor;
            }

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 500 //Refresh rate 0.5s
            };
            timer.Tick += new EventHandler(UpdateClock);
            timer.Start();

            Page1Btn.ForeColor = Color.Yellow;
            Page1Btn.BackColor = Color.FromArgb(16, 78, 139);
            #endregion

            #region 键值对2
            plc2addresses.Add("AutoSamplingStopped", "10001");
            plc2addresses.Add("AutoMaterialStackingStopped", "10003");
            plc2addresses.Add("ScraperStopped", "10002");




            #endregion

            #endregion
        }

        #region 全局变量定义
        //对象实例化

        //各用户控件对象
        public static UserControl_Page1 uc1 = new UserControl_Page1();
        public static UserControl_Page2 uc2 = new UserControl_Page2();
        public static UserControl_Page3 uc3 = new UserControl_Page3();
        public static UserControl_Page4 uc4 = new UserControl_Page4();
        public static UserControl_Page5 uc5 = new UserControl_Page5();


        public static ModbusHelper modbusHelper = new ModbusHelper();//施耐德PLC1对象
        public static ModbusHelper modbusHelper2 = new ModbusHelper();//施耐德PLC2对象

        public static SiemensHelper siemensHelper;//西门子PLC1对象
        public static SiemensHelper siemensHelper2;//西门子PLC2对象

        public static PLC1Variables plc1Variables = new PLC1Variables(); //PLC1变量对象
        public static PLC2Variables plc2Variables = new PLC2Variables(); //PLC2变量对象
        public static SystemVariables systemVariables = new SystemVariables(); //PLC全体变量对象


        public static SystemCommand systemCommand = new SystemCommand(); //系统命令对象
        public static MySqlConnection mySqlConnection;//数据库对象

        // 字典定义
        Dictionary<string, string> plc1addresses = new Dictionary<string, string>(); //PLC1变量地址
        Dictionary<string, string> plc2addresses = new Dictionary<string, string>(); //PLC2变量地址
        Dictionary<string, Control> loadedControls = new Dictionary<string, Control>(); //用户控件

        private List<Button> buttons;



        private DateTime startTime;
        IPAddress hostIP;
        IPEndPoint port;
        int point;
        Socket socketWatcher;
        bool flag;
        /*  变量定义
         *  startTime:程序启动时刻
         *  hostIP:用于存放服务器（本机）IP地址
         *  port:服务器的网络终结点，即IP:端口号
         *  point:存放用户输入的端口号
         *  socketWatcher:监听子系统的套接字
         *  flag:循环控制旗
         */



        //时间显示
        private void UpdateClock(object sender, EventArgs e)
        {
            // 获取当前时间并在 Label 控件上显示
            DateTime currentTime = DateTime.Now;
            TimeLabel.Text = currentTime.ToString("F");
        }

        // 设置序列化选项
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };


        #endregion


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //线程模块


        #region 与数据库连接线程
        /// <summary>
        /// 与数据库连接线程
        /// </summary>
        private void ConnectToDatabase()
        {
            // 连接数据库
            string IP = "127.0.0.1";
            string Database = "csr";
            string Username = "root";
            string Password = "123456";
            string connetStr = $"server={IP};database={Database};username={Username};password={Password};Charset=utf8";
            mySqlConnection = new MySqlConnection(connetStr);

            while (true)
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    try
                    {
                        mySqlConnection.Open(); // 连接数据库

                        uc3.mySqlConnection = mySqlConnection;
                        uc4.mySqlConnection = mySqlConnection;


                        UpdateText(uc3.MySqlConnectionLabel, "已连接");
                        UpdateText(uc4.MySqlConnectionLabel, "已连接");
                        UpdateText(uc5.MySqlConnectionLabel, "已连接");

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK); // 显示错误信息

                        UpdateText(uc3.MySqlConnectionLabel, "已断开");
                        UpdateText(uc4.MySqlConnectionLabel, "已断开");
                        UpdateText(uc5.MySqlConnectionLabel, "已断开");
                    }
                }

                Thread.Sleep(5000);
            }
        }

        public void UpdateText(Label label, string text)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                label.Text = text;
            }));
        }

        #endregion

        #region 与PLC连接线程 （西门子）
        private void Process_SiemensPLC_Main()
        {
            try
            {
                siemensHelper.plc = new Plc(CpuType.S71500, "192.168.1.66", 0, 1);
                siemensHelper2.plc = new Plc(CpuType.S71500, "192.168.1.150", 0, 1);

                siemensHelper.plc.Open();
                siemensHelper2.plc.Open();
            }
            catch { }

            while (true)
            {
                //PLC1
                try
                {
                    //检查plc是否连接上
                    if (siemensHelper.plc.IsConnected == true)
                    {
                        if (uc5.PlcConnectionLabel != null)
                        {
                            UpdateText(uc5.PlcConnectionLabel, "PLC1已连接！");
                        }
                    }
                    else
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC1连接失败！");
                        DisplayRichTextboxContentAndScroll("PLC1连接失败！");

                        siemensHelper.plc.Open();//再次尝试
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                //PLC2
                try
                {
                    //检查plc是否连接上
                    if (siemensHelper2.plc.IsConnected)
                    {
                        if (uc5.PlcConnectionLabel != null)
                        {
                            UpdateText(uc5.PlcConnectionLabel, "PLC2已连接！");
                        }
                    }
                    else
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC2连接失败！");
                        DisplayRichTextboxContentAndScroll("PLC2连接失败！");

                        siemensHelper2.plc.Open();
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                Thread.Sleep(5000);
            }
        }
        #endregion

        #region 与PLC连接线程 （施耐德）
        /// <summary>
        /// 与PLC连接线程
        /// </summary>
        private void Process_PLC_Main()
        {
            while (true)
            {
                //PLC1
                try
                {
                    if (modbusHelper.isConnectPLC == false)
                    {
                        if (modbusHelper.ConnectPLC("84.3.243.106", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("PLC1连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("PLC1连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "PLC1连接失败！");
                        }
                    }

                    if (modbusHelper.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC1已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                //PLC2
                try
                {
                    if (modbusHelper2.isConnectPLC == false)
                    {
                        if (modbusHelper2.ConnectPLC("84.3.243.106", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("PLC2连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("PLC2连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "PLC2连接失败！");
                        }
                    }

                    if (modbusHelper2.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC2已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                Thread.Sleep(5000);
            }
        }
        #endregion

        #region PLC数据获取线程 （西门子）
        private void Process_SiemensPLC_DataGet1()
        {
            while (true)
            {
                if (siemensHelper.plc != null && siemensHelper.plc.IsConnected)
                {
                    plc1Variables.TimeStamp = DateTime.Now; // 记录程序启动时间

                    ReadPLC1();

                    //GlobalVariables.globalVariables = plc1Variables;//同步到全局变量
                }

                //复制变量
                //CopyProperties1(plc1Variables, systemVariables);

                Thread.Sleep(1000);
            }
        }
        private void Process_SiemensPLC_DataGet2()
        {
            while (true)
            {
                ShareRes.WaitMutex();//YCQ, 20231223

                if (siemensHelper2.plc != null && siemensHelper2.plc.IsConnected)
                {
                    ReadPLC2();

                    //GlobalVariables2.globalVariables2 = plc2Variables;
                }
                ShareRes.ReleaseMutex();//YCQ, 20231223

                #region 随机数

                systemVariables.TimeStamp = DateTime.Now;

                Random random = new Random();

                // 遍历所有属性，给它们赋随机数值
                foreach (var property in systemVariables.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(systemVariables, random.Next(2) == 0);
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(systemVariables, (float)random.NextDouble());
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(systemVariables, random.Next());
                    }
                    else if (property.PropertyType == typeof(ushort))
                    {
                        property.SetValue(systemVariables, (ushort)random.Next(500));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(systemVariables, (int)random.Next(500));
                    }
                }

                #endregion

                Thread.Sleep(500);

                //复制变量
                //CopyProperties2(plc2Variables, systemVariables);

                test = JsonConvert.SerializeObject(systemVariables, settings);

                MessageBox.Show(test);
            }
        }
        #endregion

        string test;

        #region PLC数据获取线程 （施耐德）
        /// <summary>
        /// PLC数据获取线程
        /// </summary>
        private void Process_PLC_DataGet1()
        {
            while (true)
            {
                ShareRes.WaitMutex();

                if (modbusHelper.socket != null && modbusHelper.isConnectPLC == true)
                {
                    ReadPLC1();
                }

                ShareRes.ReleaseMutex();

                //复制变量
                CopyProperties1(plc1Variables, systemVariables);


                Thread.Sleep(1000);
            }
        }

        private void Process_PLC_DataGet2()
        {
            while (true)
            {
                if (modbusHelper2.socket != null && modbusHelper2.isConnectPLC == true)
                {
                    ReadPLC2();
                }

                try
                {
                    //检测PLC连接状态
                    if (modbusHelper2.socket == null || modbusHelper2.isConnectPLC == false)
                    {
                        systemVariables.PLCSignal = false;
                    }
                    else if (modbusHelper2.socket != null && modbusHelper2.isConnectPLC == true)
                    {
                        systemVariables.PLCSignal = true;
                    }
                }
                catch { }

                #region 随机数
                //Random random = new Random();

                //// 遍历所有属性，给它们赋随机数值
                //foreach (var property in systemVariables.GetType().GetProperties())
                //{
                //    if (property.PropertyType == typeof(bool))
                //    {
                //        property.SetValue(systemVariables, random.Next(2) == 0);
                //    }
                //    else if (property.PropertyType == typeof(float))
                //    {
                //        property.SetValue(systemVariables, (float)random.NextDouble());
                //    }
                //    else if (property.PropertyType == typeof(long))
                //    {
                //        property.SetValue(systemVariables, random.Next());
                //    }
                //    else if (property.PropertyType == typeof(ushort))
                //    {
                //        property.SetValue(systemVariables, (ushort)random.Next(ushort.MaxValue + 1));
                //    }
                //}

                #endregion

                //复制变量
                CopyProperties2(plc2Variables, systemVariables);


                Thread.Sleep(500);
            }
        }
        #endregion

        #region WebSocket通信线程
        /// <summary>
        /// WebSocket通信线程
        /// </summary>
        public void Process_SocketListening()
        {
            Thread.Sleep(2000);


            WebSocketHelper websocket = new WebSocketHelper(this, systemVariables);
            
            websocket.Listen("127.0.0.1:11000");


        }

        #endregion


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //方法模块


        #region 对象属性映射方法
        public static void CopyProperties1(PLC1Variables a, SystemVariables c)
        {
            Type aType = typeof(PLC1Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] aProperties = aType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo aProp in aProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == aProp.Name && p.PropertyType == aProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, aProp.GetValue(a));
                }
            }
        }
        public static void CopyProperties2(PLC2Variables b, SystemVariables c)
        {
            Type bType = typeof(PLC2Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] bProperties = bType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo bProp in bProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == bProp.Name && p.PropertyType == bProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, bProp.GetValue(b));
                }
            }
        }
        #endregion

        #region PLC单个写入方法 （施耐德）
        public void PLCWrite(int startAddress, int datatype, string data)
        {
            try
            {
                switch (datatype)
                {
                    case 0: //Byte(Byte)
                        modbusHelper2.WriteValue<byte>(startAddress, byte.Parse(data));
                        break;

                    case 1: //"Int16(Int)"
                        modbusHelper2.WriteValue<short>(startAddress, short.Parse(data));
                        break;

                    case 2: //"UInt16(Word)"
                        modbusHelper2.WriteValue<ushort>(startAddress, ushort.Parse(data));
                        break;

                    case 3: //"Int32(DInt)"
                        modbusHelper2.WriteValue<int>(startAddress, int.Parse(data));
                        break;

                    case 4: //"UInt32(DWord)"
                        modbusHelper2.WriteValue<uint>(startAddress, uint.Parse(data));
                        break;

                    case 5: //"Float(Real)"
                        modbusHelper2.WriteValue<float>(startAddress, float.Parse(data));
                        break;

                    case 6: //"Int64"
                        modbusHelper2.WriteValue<long>(startAddress, long.Parse(data));
                        break;

                    case 7: //"UInt64"
                        modbusHelper2.WriteValue<ulong>(startAddress, ulong.Parse(data));
                        break;

                    case 8: //"Double"
                        modbusHelper2.WriteValue<double>(startAddress, double.Parse(data));
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

        #endregion

        #region PLC单个读取方法 （施耐德）
        public object PLCRead(int startAddress, int datatype)
        {
            object result;
            object data = null;

            try
            {
                switch (datatype)
                {
                    case 0: //Byte(Byte)
                        byte b;
                        modbusHelper2.ReadValue<byte>(startAddress, out b);

                        data = b;
                        break;

                    case 1: //"Int16(Int)"
                        short s;
                        modbusHelper2.ReadValue<short>(startAddress, out s);

                        data = s;
                        break;

                    case 2: //"UInt16(Word)"
                        ushort us;
                        modbusHelper2.ReadValue<ushort>(startAddress, out us);

                        data = us;
                        break;

                    case 3: //"Int32(DInt)"
                        int i;
                        modbusHelper2.ReadValue<int>(startAddress, out i);

                        data = i;
                        break;

                    case 4: //"UInt32(DWord)"
                        uint ui;
                        modbusHelper2.ReadValue<uint>(startAddress, out ui);

                        data = ui;
                        break;

                    case 5: //"Float(Real)"
                        float f;
                        modbusHelper2.ReadValue<float>(startAddress, out f);

                        data = f;
                        break;

                    case 6: //"Int64"
                        long l;
                        modbusHelper2.ReadValue<long>(startAddress, out l);

                        data = l;
                        break;

                    case 7: //"UInt64"
                        ulong ul;
                        modbusHelper2.ReadValue<ulong>(startAddress, out ul);

                        data = ul;
                        break;

                    case 8: //"Double"
                        double d;
                        modbusHelper2.ReadValue<double>(startAddress, out d);

                        data = d;
                        break;

                    default:
                        DisplayRichTextboxContentAndScroll("不支持的数据类型\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                DisplayRichTextboxContentAndScroll("写寄存器出现错误，请检查：\n" + ex.Message);
            }


            result = data;

            return result;
        }
        #endregion

        #region PLC1数据映射方法
        public void ReadPLC1()
        {
            object result1 = null;

            //遍历键值对1
            foreach (string address in plc1addresses.Values)
            {
                try
                {
                    // 读取地址并处理返回的数据
                    object result = siemensHelper.PLCRead(address);

                    string key = plc1addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(PLC1Variables).GetProperty(key);

                    if (property.PropertyType == typeof(float) && result.GetType() == typeof(uint))
                    {
                        float convertedValue = Convert.ToSingle(result);
                        property.SetValue(plc1Variables, convertedValue);
                    }
                    else
                    {
                        property.SetValue(plc1Variables, result);
                    }
                }
                catch (OverflowException e)
                {
                    MessageBox.Show(result1.ToString());
                    MessageBox.Show(e.ToString());
                }
            }
        }
        #endregion

        #region PLC2数据映射方法
        public void ReadPLC2()
        {
            object result2 = null;

            //遍历键值对2
            foreach (string address in plc2addresses.Values)
            {
                try
                {
                    // 读取地址并处理返回的数据
                    object result = siemensHelper2.PLCRead(address);

                    string key = plc2addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(PLC2Variables).GetProperty(key);

                    if (property.PropertyType == typeof(float) && result.GetType() == typeof(uint))
                    {
                        float convertedValue = Convert.ToSingle(result);
                        property.SetValue(plc2Variables, convertedValue);
                    }
                    else
                    {
                        property.SetValue(plc2Variables, result);
                    }
                }
                catch (OverflowException e)
                {
                    MessageBox.Show(result2.ToString());
                    MessageBox.Show(e.ToString());
                }
            }
        }
        #endregion

        #region 数据库方法

        //数据库操作日志添加方法
        public static void OperationLogs(string operationinfo)
        {
            //记录操作日志，将数据存入数据库
            string insertSql = String.Format("insert into debug0328 (time, info) values('{0}','{1}')", DateTime.Now, operationinfo);

            ExecuteSql(insertSql);


            //删除debug0328表三个月前的数据
            string deleteSql = String.Format("delete from debug0328 where time < '{0}' ;", DateTime.Now.AddMonths(-3));
            ExecuteSql(deleteSql);
        }

        public static string connstr = "server=" + "127.0.0.1" + ";database= " + "csr" + ";username=" + "root" + ";password=" + "123456" + ";Charset=utf8";

        //执行sql语句（MySQL短连接）
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


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //其他模块


        #region 滚动式RichTextBox日志显示事件（未使用，现在使用的是uc5对象的方法）
        public void ModbusHelper_DisplayRichTextboxContentAndScrollEvent(string addContent)
        {
            uc5.displaySequence++;
            if (uc5.displaySequence >= 100)
            {
                uc5.rtxtDisplay.Invoke(new MethodInvoker(() =>
                {
                    uc5.rtxtDisplay.Clear();
                }));
                uc5.displaySequence = 0;
            }
            if (uc5.rtxtDisplay.InvokeRequired)
            {
                uc5.rtxtDisplay.Invoke(new MethodInvoker(() =>
                {
                    uc5.rtxtDisplay.AppendText(addContent + "\n");
                    uc5.rtxtDisplay.ScrollToCaret();
                }));
            }
            else
            {
                uc5.rtxtDisplay.AppendText(addContent + "\n");
                uc5.rtxtDisplay.ScrollToCaret();
            }
        }

        /// <summary>
        /// 大于100时 就清空文本框
        /// </summary>
        public int displaySequence = 0;
        /// <summary>
        /// 异常日志展示
        /// </summary>
        /// <param name="addContent">异常文本</param>
        public void DisplayRichTextboxContentAndScroll(object addContent)
        {
            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (displaySequence >= 100)
                    {
                        uc5.rtxtDisplay.Clear();
                        displaySequence = 0;
                    }
                    uc5.rtxtDisplay.AppendText(addContent + "\n");
                    uc5.rtxtDisplay.ScrollToCaret();
                }));
            }
            catch (Exception)
            {
                //MessageBox.Show("Invok错误调用");
            }
        }
        #endregion

        #region 用户控件UI事件

        //页面显示方法
        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;

            // 获取控件的名称
            string controlName = c.Name;

            try
            {
                PagePanel.Controls.Clear();
                PagePanel.Controls.Add(c);
            }
            catch { }

        }

        //颜色改变UI事件
        private void ChangeColor(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // 遍历所有按钮，将被点击的按钮文本颜色设为红色，其他按钮文本颜色设为默认颜色
            foreach (Button button in buttons)
            {
                if (button == clickedButton)
                {
                    button.ForeColor = Color.Yellow;
                    button.BackColor = Color.FromArgb(16, 78, 139);
                }
                else
                {
                    button.ForeColor = (Color)button.Tag;
                    button.BackColor = Color.FromArgb(51, 63, 98);
                }
            }
        }

        #region 页面切换按钮
        private void Page1Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc1);
        }
        private void Page2Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2);
        }

        private void Page3Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc3);
        }

        private void Page4Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc4);
        }

        private void Page5Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc5);
        }
        #endregion

        #endregion

        #region 主窗体UI事件

        //退出确认
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //让用户选择点击
            DialogResult result = MessageBox.Show("是否确认关闭？", "正在关闭系统...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //判断是否取消事件
            if (result == DialogResult.No)
            {
                //取消退出
                e.Cancel = true;
            }
        }
        //按钮退出系统
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否确认关闭？", "正在关闭系统...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                //关闭TCP连接
                //modbusHelper2.CloseConnect();


                Environment.Exit(0);
            }
            else if (dr == DialogResult.No)
            {
            }
        }

        //按钮最小化系统
        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        //主窗体退出时占用资源销毁
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            modbusHelper2.CloseConnect();


            string processesName = "ShenYangRemoteSystem";
            System.Diagnostics.Process[] targetProcess = System.Diagnostics.Process.GetProcessesByName("ShenYangRemoteSystem");
            foreach (System.Diagnostics.Process process in targetProcess)
            {
                if (processesName == process.ProcessName)
                {
                    process.Kill();
                }
            }


            Environment.Exit(0);
        }
        #endregion

        #region 信号量
        public class ShareRes
        {
            public int count = 0;
            public static Mutex mutex = new Mutex();
            public static void WaitMutex()
            {
                mutex.WaitOne();

            }

            public static void ReleaseMutex()
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion


        //添加配置文件信息
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件

            cfa.AppSettings.Settings["Port"].Value = "COM1";
            cfa.Save();  //保存配置文件
            ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件

            MessageBox.Show(cfa.AppSettings.Settings["Port"].Value.ToString());
        }
    }
}
