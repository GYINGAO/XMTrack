using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TrackRedesign {
    public class ChartHelper {
        /// <summary>
        /// 向chartcontrol中添加一条曲线
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        public static void AddOneSeries(DataTable dtSource, ChartControl chart, string seriesName, string yName) {
            Series series = new Series(seriesName, ViewType.Line);
            SeriesPoint sp;
            for (int i = 0; i < dtSource.Rows.Count; i++) {
                sp = new SeriesPoint(Convert.ToDouble(dtSource.Rows[i]["里程"]), Convert.ToDouble(dtSource.Rows[i][yName]));
                series.Points.Add(sp);
            }
            chart.Series.Add(series);
        }
        public static void AddOneSeries(DataTable dtSource, ChartControl chart, string seriesName, string xName, string yName, ViewType lineType) {
            Series series = new Series(seriesName, lineType);
            ((LineSeriesView)series.View).MarkerVisibility = DefaultBoolean.True;
            SeriesPoint sp;
            for (int i = 0; i < dtSource.Rows.Count; i++) {
                sp = new SeriesPoint(Convert.ToDouble(dtSource.Rows[i][xName]), Convert.ToDouble(dtSource.Rows[i][yName]));
                series.Points.Add(sp);
            }
            chart.Series.Add(series);
            //series.CheckableInLegend = true;
            //series.CheckedInLegend = false;
        }

        /// <summary>
        /// 设置平面复选框是否为选中状态
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="b">调前平面、调整量、右界，左界）</param>
        public static void SetPPCheckBoxes(ChartControl targetChart, params bool[] b) {
            foreach (Series series in targetChart.Series) {
                if (series.Name == "调前平面") {
                    series.CheckedInLegend = b[0];
                }
                else if (series.Name == "调整量") {
                    series.CheckedInLegend = b[1];
                }
                else if (series.Name == "右界") {
                    series.CheckedInLegend = b[2];
                }
                else if (series.Name == "左界") {
                    series.CheckedInLegend = b[3];
                }
            }
        }


        /// <summary>
        /// 记录平面图例复选框的状态
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="b"></param>
        /// <param name="str"></param>
        public static void LogChecked(ChartControl targetChart, bool[] b) {
            foreach (Series series in targetChart.Series) {
                if (series.Name == "调前平面") {
                    b[0] = series.CheckedInLegend;
                }
                else if (series.Name == "调整量") {
                    b[1] = series.CheckedInLegend;
                }
                else if (series.Name == "右界") {
                    b[2] = series.CheckedInLegend;
                }
                else if (series.Name == "左界") {
                    b[3] = series.CheckedInLegend;
                }
            }
        }

        /// <summary>
        /// 记录高程图例复选框的状态
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="b"></param>
        public static void LogEleChecked(ChartControl targetChart, bool[] b) {
            foreach (Series series in targetChart.Series) {
                if (series.Name == "调前高程") {
                    b[0] = series.CheckedInLegend;
                }
                else if (series.Name == "调整量") {
                    b[1] = series.CheckedInLegend;
                }
                else if (series.Name == "下界") {
                    b[2] = series.CheckedInLegend;
                }
                else if (series.Name == "上界") {
                    b[3] = series.CheckedInLegend;
                }
            }
        }
        /// <summary>
        /// 设置高程复选框是否为选中状态
        /// </summary>
        /// <param name="targetChart"></param>
        /// <param name="b">调前高程、调整量、下界，上界）</param>
        public static void SetEleCheckBoxes(ChartControl targetChart, params bool[] b) {
            foreach (Series series in targetChart.Series) {
                if (series.Name == "调前高程") {
                    series.CheckedInLegend = b[0];
                }
                else if (series.Name == "调整量") {
                    series.CheckedInLegend = b[1];
                }
                else if (series.Name == "下界") {
                    series.CheckedInLegend = b[2];
                }
                else if (series.Name == "上界") {
                    series.CheckedInLegend = b[3];
                }
            }
        }
        /// <summary>
        /// 设置chart的图例属性和坐标轴属性
        /// </summary>
        /// <param name="targetChart"></param>
        public static void SetLegendAndXY(ChartControl targetChart) {
            //系列名在图表上的横向位置
            targetChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            targetChart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            targetChart.Legend.Direction = LegendDirection.LeftToRight;
            //打开复选框，可以任意选择系列
            targetChart.Legend.UseCheckBoxes = true;
            XYDiagram xyDiagram = (XYDiagram)targetChart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "里程(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;
            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "偏差值(mm)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            //xyDiagram.EnableAxisXZooming = true;
            //网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            //刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;
            //用自动刻度分隔线
            //xyDiagram.AxisY.AutoScaleBreaks.Enabled = true;
            //xyDiagram.AxisY.AutoScaleBreaks.MaxCount = 20;

            //xyDiagram.EnableAxisYZooming = true;
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="name"></param>
        public static void SetChartTitle(ChartControl chart, string name) {
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = name;
            chartTitle.TextColor = Color.Black;
            chartTitle.Font = new Font("Tahoma", 14, FontStyle.Bold);
            chart.Titles.Add(chartTitle);
        }
        /// <summary>
        /// 把datatable指定列添加到datagridview
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="targetDgv"></param>
        /// <param name="addColumn"></param>
        public static void AddDtToDgv(DataTable dtSource, DataGridView targetDgv, params string[] addColumn) {
            DataTable dt = CopyDt(dtSource, addColumn);
            targetDgv.DataSource = dt;
            foreach (string item in addColumn) {
                targetDgv.Columns[item].DefaultCellStyle.Format = "0.000";
            }
            targetDgv.SetDoubleBuffered(true);
        }
        /// <summary>
        /// 把datatable指定列复制到另一个datatable
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="addColumn"></param>
        /// <returns></returns>
        public static DataTable CopyDt(DataTable dtSource, params string[] addColumn) {
            DataTable dt = new DataTable();
            foreach (string str in addColumn) {
                dt.Columns.Add(str, Type.GetType("System.Double"));
            }
            for (int i = 0; i < dtSource.Rows.Count; i++) {
                DataRow dr = dt.NewRow();
                foreach (string columnName in addColumn) {
                    dr[columnName] = dtSource.Rows[i][columnName];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 将dataGridView数据转成DataTable,包含列名
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataTable GetDgvToTable(DataGridView dgv, params string[] colName) {
            DataTable dt = new DataTable();
            // 列强制转换
            foreach (string str in colName) {
                DataColumn dc = new DataColumn(str, Type.GetType("System.Double"));
                dt.Columns.Add(dc);
            }
            // 循环行
            for (int count = 0; count < dgv.Rows.Count; count++) {
                DataRow dr = dt.NewRow();
                foreach (string str in colName) {
                    dr[str] = Convert.ToDouble(dgv.Rows[count].Cells[str].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 自动添加编号
        /// </summary>
        /// <param name="dgv"></param>
        public static void AutoNumber(DataGridView dgv, DataGridViewRowPostPaintEventArgs e) {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y,
               dgv.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dgv.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dgv.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 判断X是否选中
        /// </summary>
        /// <param name="ctcl"></param>
        /// <param name="rbtnX"></param>
        public static void XZooming(ChartControl ctcl, RadioButton rbtnX) {
            XYDiagram xy = ctcl.Diagram as XYDiagram;
            if (rbtnX.Checked == true)
                xy.EnableAxisXZooming = true;
            else
                xy.EnableAxisXZooming = false;

        }
        public static void YZooming(ChartControl ctcl, RadioButton rbtnY) {
            XYDiagram xy = ctcl.Diagram as XYDiagram;
            if (rbtnY.Checked == true)
                xy.EnableAxisYZooming = true;
            else
                xy.EnableAxisYZooming = false;
        }

        /// <summary>
        /// 设置指定series的颜色
        /// </summary>
        /// <param name="ctcl">要修改的ChartControl</param>
        /// <param name="seriesName">要修改的series的名字，调前平面、调整量、右界，左界，调整中线</param>
        public static void SetSeriesColor(ChartControl ctcl, Color[] color, params string[] seriesName) {
            foreach (Series series in ctcl.Series) {
                if (series.Name == seriesName[0]) {
                    series.View.Color = color[0];
                    //series.View.Color = Color.FromArgb(255, 105, 180);
                }
                else if (series.Name == seriesName[1]) {
                    series.View.Color = color[1];
                    //series.View.Color = Color.FromArgb(0, 0, 0);
                }
                else if (series.Name == seriesName[2]) {
                    series.View.Color = color[2];
                    //series.View.Color = Color.FromArgb(0, 191, 255);
                }
                else if (series.Name == seriesName[3]) {
                    series.View.Color = color[3];
                    //series.View.Color = Color.FromArgb(0, 191, 255);
                }
                else if (series.Name == seriesName[4]) {
                    series.View.Color = color[4];
                    //series.View.Color = Color.FromArgb(0, 0, 205);
                }
            }
        }

        /// <summary>
        /// 删除dgv重复的行
        /// </summary>
        /// <param name="dgv"></param>
        public static void DelDgvDuplicateLine(DataGridView dgv) {
            for (int i = 0; i < dgv.Rows.Count - 1; i++) {
                for (int j = i + 1; j < dgv.Rows.Count; j++) {
                    if (dgv.Rows[i].Cells[0].Value == dgv.Rows[j].Cells[0].Value) {
                        dgv.Rows.RemoveAt(j);
                    }
                    else
                        break;
                }
            }
        }

    }
}
