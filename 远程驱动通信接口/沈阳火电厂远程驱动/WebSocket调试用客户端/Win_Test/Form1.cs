using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Reflection;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using WebSocketSharp;

namespace Win_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += MainForm_FormClosed;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "127.0.0.1";
            this.textBox4.Text = "11000";
            this.textBox3.Text = "6";
            this.textBox5.Text = "1";
            this.comboBox2.Text = "1";

            systemCommand.QUERY_SYSTEM = "TEST";//子系统ID

            webSocket = new WebSocketHelper(this);
        }

        #region 声明变量
        /*  变量定义
         *  hostIP:用于存放服务器（本机）IP地址
         *  port:存放用户输入的端口号
         */

        IPAddress hostIP;
        int port;
        #endregion

        #region 实例化对象
        //实例化样机（以后有别的类型的机器就添加相应的类到解决方案中，这里是把斗轮堆取料机作为样机）
        private ServerCommand systemCommand = new ServerCommand();

        //实例化webSocket对象
        WebSocketHelper webSocket;

        // 设置序列化选项
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };
        #endregion

        #region 发送按钮事件
        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
                hostIP = IPAddress.Parse(textBox1.Text.Trim());
                port = int.Parse(textBox4.Text.Trim());

                systemCommand.DATA_TYPE = int.Parse(textBox3.Text.Trim());//数据类型
                systemCommand.QUERY_TYPE = int.Parse(textBox5.Text.Trim());//命令类型
                systemCommand.COMMAND_NAME = comboBox1.Text;//命令名
                systemCommand.DATA_INT = int.Parse(comboBox2.Text.Trim());//命令内容

                string json = JsonConvert.SerializeObject(systemCommand, settings);

                string param1 = $"{hostIP}:{port}";
                string param2 = json;

                Thread thread = new Thread(() => webSocket.Send(param1, param2)); // Lambda表达式，匿名方法

                thread.Start();
            //}
            //catch(Exception ey)
            //{
                //MessageBox.Show("发送失败"+ey.ToString());
                //button3_Click(null, null);
            //}
        }
        #endregion

        #region 断开连接按钮事件
        private void button3_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
