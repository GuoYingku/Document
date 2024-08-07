using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page2 : UserControl
    {
        public UserControl_Page2()
        {
            InitializeComponent();
        }

        private void UserControl_Page2_Load(object sender, EventArgs e)
        {
            #region UI处理业务
            // 将所有按钮添加到列表中
            buttons = new List<Button> { Page2_1Btn, Page2_2Btn, Page2_3Btn, Page2_4Btn, Page2_5Btn, Page2_6Btn };

            // 为每个按钮添加单击事件处理程序
            foreach (Button button in buttons)
            {
                button.Click += ChangeColor;
                button.Tag = button.ForeColor;
            }

            Page2_1Btn.ForeColor = Color.Yellow;
            Page2_1Btn.BackColor = Color.FromArgb(16, 78, 139);
            #endregion


            uc2_1 = Form1.uc2_1;
            uc2_2 = Form1.uc2_2;
            uc2_3 = Form1.uc2_3;
            uc2_4 = Form1.uc2_4;
            uc2_5 = Form1.uc2_5;
            uc2_6 = Form1.uc2_6;


            Page2_1Btn_Click(null, null); //初始化
        }


        public static UserControl_Page2_1 uc2_1;
        public static UserControl_Page2_2 uc2_2;
        public static UserControl_Page2_3 uc2_3;
        public static UserControl_Page2_4 uc2_4;
        public static UserControl_Page2_5 uc2_5;
        public static UserControl_Page2_6 uc2_6;

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
                Page2Panel.Controls.Clear();
                Page2Panel.Controls.Add(c);
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

        private void Page2_1Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_1);
        }

        private void Page2_2Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_2);
        }

        private void Page2_3Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_3);
        }

        private void Page2_4Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_4);
        }

        private void Page2_5Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_5);
        }

        private void Page2_6Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2_6);
        }
        #endregion


    }
}
