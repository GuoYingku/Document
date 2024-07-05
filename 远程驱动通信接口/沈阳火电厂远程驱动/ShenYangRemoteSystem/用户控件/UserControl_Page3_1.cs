using MySql.Data.MySqlClient;
using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page3_1 : UserControl
    {
        public UserControl_Page3_1()
        {
            InitializeComponent();

            //ConnectToDatabase();
        }
        private void UserControl_Page3_1_Load(object sender, EventArgs e)
        {
            #region Chart图表控件初始化

            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            //Chart UI设置
            // 设置 X 轴的间隔
            //chart1.ChartAreas[0].AxisX.Interval = 20;
            //chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;

            
            #endregion
        }

        public MySqlConnection mySqlConnection = null;

        public static string tablename = "criticalparameter";

        public static string variable1name = "M_ScraperMo";

        

        //int series = 1;


        #region 图表显示按钮
        // 根据按钮的点击事件，显示对应数据在图表中
        private void InsertBtn_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePickerStart.Value;
            DateTime endTime = dateTimePickerEnd.Value;


            DisplayChart(startTime, endTime);


            //ShowChart(startTime, endTime, series.ToString());
            //series++;
        }

        private void DisplayChart(DateTime startTime, DateTime endTime)
        {
            try
            {
                int maxnumber = int.Parse(comboBox1.Text);

                string query = $"select time, {variable1name} from {tablename} where time >= '{startTime.ToString("yyyy-MM-dd HH:mm:ss")}' and time <= '{endTime.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY time DESC LIMIT {maxnumber}";//给出表名称'

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, mySqlConnection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // 清空现有的 Series 和 ChartAreas
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                // 创建一个新的 ChartArea
                ChartArea chartArea = new ChartArea();
                chart1.ChartAreas.Add(chartArea);

                // 创建一个新的 Series（曲线）
                Series series = new Series();

                // 设置曲线样式
                series.Color = Color.FromArgb(80, 240, 250);
                series.BorderWidth = 4;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;

                // 设置采样点样式
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.MarkerColor = Color.Red;


                // 添加数据到 Series 中
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime xValue = Convert.ToDateTime(row["time"]);
                    double yValue = Convert.ToDouble(row[variable1name]);
                    series.Points.AddXY(xValue, yValue);
                }

                // 将 Series 添加到 Chart 控件中
                chart1.Series.Add(series);

                // 设置 X 轴标签显示格式为精确到秒的时间
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";


                // 设置坐标轴
                chart1.ChartAreas[0].AxisX.LineWidth = 5;
                chart1.ChartAreas[0].AxisY.LineWidth = 5;
                chart1.ChartAreas[0].AxisX.LineColor = Color.White;
                chart1.ChartAreas[0].AxisY.LineColor = Color.White;
                chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
                chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;


                // 设置图表背景颜色
                chart1.ChartAreas[0].BackColor = Color.FromArgb(16, 78, 139);
                chart1.ChartAreas[0].BorderColor = Color.FromArgb(16, 78, 139);
                chart1.BackColor = Color.FromArgb(16, 78, 139);


                // 刷新图表
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        #endregion

    }
}
