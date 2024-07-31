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
    public partial class UserControl_Page3_3 : UserControl
    {
        public UserControl_Page3_3()
        {
            InitializeComponent();
        }


        public MySqlConnection mySqlConnection = null;


        public static string tablename2 = "warning";


        private void SelectBtn_Click(object sender, EventArgs e)
        {
            #region 报警表
            //筛选日期
            string startTime2 = dateTimePicker2Start.Value.ToString("yyyy-MM-dd");
            string endTime2 = dateTimePicker2End.Value.ToString("yyyy-MM-dd");
            string searchStr2 = "select * from " + tablename2 + " where time >= '" + startTime2 + "' and time <= '" + endTime2 + "'";


            MySqlDataAdapter adapter2 = new MySqlDataAdapter(searchStr2, mySqlConnection);
            DataSet dataSet2 = new DataSet();
            adapter2.Fill(dataSet2, "table2");
            this.dataGridView2.DataSource = dataSet2.Tables["table2"];


            dataGridView2.Columns[0].HeaderText = "序号";
            dataGridView2.Columns[1].HeaderText = "时间";
            dataGridView2.Columns[2].HeaderText = "报警内容";
            dataGridView2.Columns[3].HeaderText = "操作员";


            // 手动对该列进行排序
            dataGridView2.Sort(dataGridView2.Columns["time"], ListSortDirection.Descending);


            #endregion
        }
    }
}
