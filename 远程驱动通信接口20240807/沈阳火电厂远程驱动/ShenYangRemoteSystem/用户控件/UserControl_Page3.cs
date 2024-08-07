using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page3 : UserControl
    {
        public UserControl_Page3()
        {
            InitializeComponent();
        }
        private void UserControl_Page3_Load(object sender, EventArgs e)
        {
            #region UI处理业务
            // 将所有按钮添加到列表中
            buttons = new List<Button> { Page3_1Btn, Page3_2Btn, Page3_3Btn };

            // 为每个按钮添加单击事件处理程序
            foreach (Button button in buttons)
            {
                button.Click += ChangeColor;
                button.Tag = button.ForeColor;
            }

            Page3_1Btn.ForeColor = Color.Yellow;
            Page3_1Btn.BackColor = Color.FromArgb(16, 78, 139);
            #endregion


            uc3_1 = Form1.uc3_1;
            uc3_2 = Form1.uc3_2;
            uc3_3 = Form1.uc3_3;

            uc3_1.mySqlConnection = mySqlConnection;
            uc3_2.mySqlConnection = mySqlConnection;
            uc3_3.mySqlConnection = mySqlConnection;

            Page3_1Btn_Click(null, null); //初始化
        }


        public static UserControl_Page3_1 uc3_1;
        public static UserControl_Page3_2 uc3_2;
        public static UserControl_Page3_3 uc3_3;

        private List<Button> buttons;

        public MySqlConnection mySqlConnection = null;


        #region 用户控件UI事件

        //页面显示方法
        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;

            // 获取控件的名称
            string controlName = c.Name;

            try
            {
                Page3Panel.Controls.Clear();
                Page3Panel.Controls.Add(c);
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

        private void Page3_1Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc3_1);

            uc3_1.dateTimePickerEnd.Invoke(new MethodInvoker(() =>
            {
                uc3_1.dateTimePickerEnd.Value = DateTime.Now;
            }));
        }

        private void Page3_2Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc3_2);
        }

        private void Page3_3Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc3_3);
        }

        #endregion

    }
}
