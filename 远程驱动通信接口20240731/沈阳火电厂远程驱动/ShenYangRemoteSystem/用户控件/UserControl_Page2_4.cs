using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page2_4 : UserControl
    {
        public UserControl_Page2_4()
        {
            InitializeComponent();
        }

        private void UserControl_Page2_4_Load(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(Uc2_4LifeProcess));

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

            controlHelper = new ControlHelper(this);// 初始化控件帮助类对象
        }



        public ControlHelper controlHelper;


        List<Thread> connectedThreads = new List<Thread>(); // 用于存储已连接的线程


        //图片更新进程
        private void Uc2_4LifeProcess()
        {
            while (true)
            {
                SystemVariables systemVariables = Form1.systemVariables;//此处没有使用局部变量表示的全局变量类对象，而是使用了ControlHelper类中实例化的全局变量类对象，这里先观察以下可不可行，如果可行的话以后都用这种方案。

                Thread.Sleep(1000);


                //系统状态
                controlHelper.UpdateRedByVariable("pictureBox72", "AutoMaterialStackingStopped");

            }
        }
    }
}
