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
    public partial class UserControl_Page3 : UserControl
    {
        public UserControl_Page3()
        {
            InitializeComponent();

            //ConnectToDatabase();
        }
        private void UserControl_Page3_Load(object sender, EventArgs e)
        {
            #region Chart图表控件初始化

            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";



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
            #endregion
        }

        public MySqlConnection mySqlConnection = null;

        public static string tablename = "criticalparameter";

        public static string variable1name = "M_ScraperMotor1Current";


        int series = 1;


        #region 图表显示按钮
        // 根据按钮的点击事件，显示对应数据在图表中
        private void InsertBtn_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePickerStart.Value;
            DateTime endTime = dateTimePickerEnd.Value;
            

            ShowChart(startTime, endTime, series.ToString());

            series++;
        }
        private void ShowChart(DateTime startTime, DateTime endTime, string seriesName)
        {
            //try
            //{
                string query = $"select time, {variable1name} from {tablename} where time >= '{startTime.ToString("yyyy-MM-dd HH:mm:ss")}' and time <= '{endTime.ToString("yyyy-MM-dd HH:mm:ss")}'";//给出表名称

                chart1.Series.Clear();

                Series series = new Series(seriesName);

                //Chart UI设置
                // 设置 X 轴的间隔
                chart1.ChartAreas[0].AxisX.Interval = 20;
                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;

                // 设置曲线样式
                series.Color = Color.FromArgb(80, 240, 250);
                series.BorderWidth = 4;
                series.ChartType = SeriesChartType.Spline;
                series.XValueType = ChartValueType.DateTime;

                // 设置采样点样式
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 8;
                series.MarkerColor = Color.Red;



                MySqlDataAdapter adapter = new MySqlDataAdapter(query, mySqlConnection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "dataChart");


                //DateTime currentTime = DateTime.Now;
                int startIndex = 0;
            
                //找到时间范围内最晚的采样点
                for (int i = 0; i < dataSet.Tables["dataChart"].Rows.Count; i++)
                {
                    DataRow row = dataSet.Tables["dataChart"].Rows[i];
                    DateTime timestamp = Convert.ToDateTime(row["time"]);

                    if (timestamp <= endTime)
                    {
                        startIndex = i;
                    }
                }


                //从这个采样点开始向前遍历指定数量的采样点个数，将时间戳和对应数值添加到series的数据点中
            for (int i = startIndex; i >= Math.Max(startIndex - 19, 0); i--)
            {
                DataRow row = dataSet.Tables["dataChart"].Rows[i];
                DateTime timestamp = Convert.ToDateTime(row["time"]);
                double value = Convert.ToDouble(row[variable1name]);
                series.Points.AddXY(timestamp, value);
            }

            MessageBox.Show(series.ToString());

                chart1.Series.Add(series);

                // 设置 X 轴标签显示格式为精确到秒的时间
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        #endregion

    }
}
