using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using NH;
using TrackRedesign.utils;
using Point = TrackRedesign.utils.Point;

namespace TrackRedesign {
    public partial class FrmAddAdjustLine : Form {
        private int maxIndex;
        private double maxOverrun;
        //定义弦长
        public readonly double chordLength = 70;
        //定义挠度限差
        public static readonly double bendTol = 3;
        //定义点的查找范围
        readonly double tol = 30;
        //定义折线颜色数组
        public Color[] ppColor = new Color[5] {Color.FromArgb(255, 156, 0), Color.FromArgb(0, 139, 222),
            Color.FromArgb(1, 191, 191), Color.FromArgb(1, 191, 191), Color.FromArgb(237, 3, 2), };
        public Color[] eleColor = new Color[5] {Color.FromArgb(255, 156, 0), Color.FromArgb(0, 139, 222),
            Color.FromArgb(1, 191, 191), Color.FromArgb(1, 191, 191), Color.FromArgb(237, 3, 2), };

        //定义图例复选框状态数组
        public bool[] PPChecked = new bool[4];
        public bool[] EleChecked = new bool[4];
        private FrmMain frmMain;
        public FrmAddAdjustLine() {
            InitializeComponent();
        }
        public FrmAddAdjustLine(FrmMain passFrm) {
            InitializeComponent();
            frmMain = passFrm;
            this.DoubleBuffered = true;
        }


        /// <summary>
        /// 窗体载入，载入图像和数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAddAdjustLine_Load(object sender, EventArgs e) {
            if (frmMain.AdjustPlanPP.Rows.Count == 0) {
                //ChartHelper.AddOneSeries(frmMain.averageValue, ctclPPAdjLine, "调前平面", "调前平面");
                string[] columnPPName = { "里程", "调前平面" };
                //添加默认起终点
                dgvPPAdjustLine.DataSource = null;
                for (int i = 0; i < 2; i++) {
                    int index = dgvPPAdjustLine.Rows.Add();
                    if (index == 0) {
                        dgvPPAdjustLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[0][0];
                        dgvPPAdjustLine.Rows[index].Cells[1].Value = 0;
                    }
                    else if (index == 1) {
                        dgvPPAdjustLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[frmMain.averageValue.Rows.Count - 1][0];
                        dgvPPAdjustLine.Rows[index].Cells[1].Value = 0;
                    }
                }
                CalcPP();
                //画模拟中线
                ChartHelper.AddOneSeries(frmMain.tendLinePP, ctclPPAdjLine, "调整中线", "偏差值");
                //画调整量图像
                PaintFromCalcPPResult();
                ChartHelper.SetLegendAndXY(ctclPPAdjLine);
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                //设置图例复选框状态
                //ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
                ChartHelper.AddDtToDgv(frmMain.averageValue, dgvPPData, columnPPName);
            }
            else {
                InitPP();
            }
            if (frmMain.AdjustPlanElevation.Rows.Count == 0) {
                //高程
                //ChartHelper.AddOneSeries(frmMain.averageValue, ctclElevation, "调前高程", "调前高程");

                string[] columnElevationName = { "里程", "调前高程" };
                //添加默认起终点
                dgvElevationLine.DataSource = null;
                for (int i = 0; i < 2; i++) {
                    int index = dgvElevationLine.Rows.Add();
                    if (index == 0) {
                        dgvElevationLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[0][0];
                        dgvElevationLine.Rows[index].Cells[1].Value = 0;
                    }
                    else if (index == 1) {
                        dgvElevationLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[frmMain.averageValue.Rows.Count - 1][0];
                        dgvElevationLine.Rows[index].Cells[1].Value = 0;
                    }
                }
                //计算调整量
                CalcElevation();
                //画模拟中线
                ChartHelper.AddOneSeries(frmMain.tendLineElevation, ctclElevation, "调整中线", "偏差值");
                //画调整量图像
                PaintFromCalcElevationResult();
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                ChartHelper.SetLegendAndXY(ctclElevation);
                ChartHelper.AddDtToDgv(frmMain.averageValue, dgvElevationData, columnElevationName);
            }
            else {
                InitElevation();
            }
        }

        private void dgvPPData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            //自动编号，与数据无关
            ChartHelper.AutoNumber(dgvPPData, e);
        }


        /// <summary>
        /// 单个添加平面调整中线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPPAdd_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(txtPPMiles.Text)) {
                MessageBox.Show("请输入里程！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (String.IsNullOrEmpty(txtPPBias.Text)) {
                MessageBox.Show("请输入偏差值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //记录当前图例复选框的状态
            ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);

            //dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);

            //txt添加进datagridview
            try {
                for (int i = 0; i < dgvPPAdjustLine.Rows.Count; i++) {
                    if (Math.Round(double.Parse(dgvPPAdjustLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(txtPPMiles.Text.Trim()), 1)) {
                        MessageBox.Show("里程重复，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                int index = this.dgvPPAdjustLine.Rows.Add();
                dgvPPAdjustLine.Rows[index].Cells[0].Value = Convert.ToDouble(txtPPMiles.Text.Trim());
                dgvPPAdjustLine.Rows[index].Cells[1].Value = Convert.ToDouble(txtPPBias.Text.Trim());
                dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvPPAdjustLine, chordLength);
                //画趋势折线
                PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                txtPPMiles.Clear();
                txtPPBias.Clear();
                frmMain.AdjustPlanPP.Rows.Clear();
            }
            catch (Exception) {
                MessageBox.Show("输入有误，请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //计算调整量
            CalcPP();
            //画调整量图像
            PaintFromCalcPPResult();
            ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            //设置图例复选框状态
            ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
        }

        /// <summary>
        /// 清空平面调整中线表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPPClear_Click(object sender, EventArgs e) {
            if (MessageBox.Show("您确定要清空列表吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                return;
            }
            ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
            dgvPPAdjustLine.Rows.Clear();
            ctclPPAdjLine.Series.Clear();
            frmMain.tendLinePP.Rows.Clear();
            frmMain.AdjustPlanPP.Rows.Clear();
            //ChartHelper.AddOneSeries(frmMain.averageValue, ctclPPAdjLine, "调前平面", "调前平面");
            for (int i = 0; i < 2; i++) {
                int index = dgvPPAdjustLine.Rows.Add();
                if (index == 0) {
                    dgvPPAdjustLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[0][0];
                    dgvPPAdjustLine.Rows[index].Cells[1].Value = 0;
                }
                else if (index == 1) {
                    dgvPPAdjustLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[frmMain.averageValue.Rows.Count - 1][0];
                    dgvPPAdjustLine.Rows[index].Cells[1].Value = 0;
                }
            }
            //计算调整量
            CalcPP();
            //画模拟中线
            ChartHelper.AddOneSeries(frmMain.tendLinePP, ctclPPAdjLine, "调整中线", "偏差值");
            //画调整量图像
            PaintFromCalcPPResult();
            ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
        }
        /// <summary>
        /// 平面调整中线修改或删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPPAdjustLine_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex != -1) {
                int rowIndex = e.RowIndex;
                DataGridViewCell cell = dgvPPAdjustLine.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "修改") {
                    FrmEditPPPoint frm = new FrmEditPPPoint(rowIndex, this);
                    //frm.MdiParent = this.MdiParent;
                    frm.TopMost = true;
                    frm.Show();
                    frmMain.AdjustPlanPP.Rows.Clear();
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "删除") {
                    if (e.RowIndex == 0) {
                        MessageBox.Show("起点无法删除", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (e.RowIndex == dgvPPAdjustLine.Rows.Count - 1) {
                        MessageBox.Show("终点无法删除", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (MessageBox.Show("确定删除该点？", "删除提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes) {
                        DataGridViewRow row = dgvPPAdjustLine.Rows[e.RowIndex];
                        dgvPPAdjustLine.Rows.Remove(row);//删除行  
                        frmMain.AdjustPlanPP.Rows.Clear();
                        dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
                        ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                        PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                        //计算调整量
                        CalcPP();
                        //画调整量图像
                        PaintFromCalcPPResult();
                        ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                        ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
                    }
                }

            }
        }
        /// <summary>
        /// 将表格中的数据画图
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ctcl"></param>
        /// <param name="xName"></param>
        /// <param name="yName"></param>
        public void PaintFromDgv(DataGridView dgv, ChartControl ctcl, string xName, string yName) {
            DataTable dt = ChartHelper.GetDgvToTable(dgv, xName, yName);
            ctcl.Series.Clear();
            ChartHelper.AddOneSeries(dt, ctcl, "调整中线", xName, yName, DevExpress.XtraCharts.ViewType.Line);
            //ChartHelper.AddOneSeries(frmMain.averageValue, ctcl, "调前平面", "调前平面");
        }

        private void dgvPPAdjustLine_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvPPAdjustLine, e);
        }

        private void dgvElevationLine_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvElevationLine, e);
        }

        private void dgvElevationData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvElevationData, e);
        }
        /// <summary>
        /// 单个添加高程调整中线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevationAdd_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(txtElevationMiles.Text)) {
                MessageBox.Show("请输入里程！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (String.IsNullOrEmpty(txtElevationBias.Text)) {
                MessageBox.Show("请输入偏差值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
            //txt添加进datagridview
            try {
                for (int i = 0; i < dgvElevationLine.Rows.Count; i++) {
                    if (Math.Round(double.Parse(dgvElevationLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(txtElevationMiles.Text.Trim()), 1)) {
                        MessageBox.Show("里程重复，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                frmMain.AdjustPlanElevation.Rows.Clear();
                int index = this.dgvElevationLine.Rows.Add();
                dgvElevationLine.Rows[index].Cells[0].Value = Convert.ToDouble(txtElevationMiles.Text.Trim());
                dgvElevationLine.Rows[index].Cells[1].Value = Convert.ToDouble(txtElevationBias.Text.Trim());
                //画图
                dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvElevationLine, chordLength);
            }
            catch (Exception) {
                MessageBox.Show("输入有误，请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ChartHelper.LogEleChecked(ctclElevation, EleChecked);
            PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
            txtElevationMiles.Clear();
            txtElevationBias.Clear();
            //计算调整量
            CalcElevation();
            //画调整量图像
            PaintFromCalcElevationResult();
            ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
        }

        /// <summary>
        /// 清空高程调整中线表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevationClear_Click(object sender, EventArgs e) {
            if (MessageBox.Show("您确定要清空列表吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                return;
            }
            ChartHelper.LogEleChecked(ctclElevation, EleChecked);
            dgvElevationLine.Rows.Clear();
            ctclElevation.Series.Clear();
            frmMain.tendLineElevation.Rows.Clear();
            frmMain.AdjustPlanElevation.Rows.Clear();
            //ChartHelper.AddOneSeries(frmMain.averageValue, ctclElevation, "调前高程", "调前高程");
            //添加默认起终点
            dgvElevationLine.DataSource = null;
            for (int i = 0; i < 2; i++) {
                int index = dgvElevationLine.Rows.Add();
                if (index == 0) {
                    dgvElevationLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[0][0];
                    dgvElevationLine.Rows[index].Cells[1].Value = 0;
                }
                else if (index == 1) {
                    dgvElevationLine.Rows[index].Cells[0].Value = frmMain.averageValue.Rows[frmMain.averageValue.Rows.Count - 1][0];
                    dgvElevationLine.Rows[index].Cells[1].Value = 0;
                }
            }
            //计算调整量
            CalcElevation();
            //画模拟中线
            ChartHelper.AddOneSeries(frmMain.tendLineElevation, ctclElevation, "调整中线", "偏差值");
            //画调整量图像
            PaintFromCalcElevationResult();
            ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
        }
        /// <summary>
        /// 高程调整中线修改或删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvElevationLine_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex != -1) {
                int rowIndex = e.RowIndex;
                DataGridViewCell cell = dgvElevationLine.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "修改") {
                    FrmEditEvelationPoint frm = new FrmEditEvelationPoint(rowIndex, this);
                    //frm.MdiParent = this.MdiParent;
                    //frm.Parent = this.Parent;
                    //frm.StartPosition = FormStartPosition.Manual;
                    frm.Show();
                    frmMain.AdjustPlanElevation.Rows.Clear();
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "删除") {
                    if (e.RowIndex == 0) {
                        MessageBox.Show("起点无法删除", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (e.RowIndex == dgvElevationLine.Rows.Count - 1) {
                        MessageBox.Show("终点无法删除", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (MessageBox.Show("确定删除该点？", "删除提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes) {
                        frmMain.AdjustPlanElevation.Rows.Clear();
                        DataGridViewRow row = dgvElevationLine.Rows[e.RowIndex];
                        dgvElevationLine.Rows.Remove(row);//删除行  
                        dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
                        ChartHelper.LogEleChecked(ctclElevation, EleChecked);
                        PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                        //计算调整量
                        CalcElevation();
                        //画调整量图像
                        PaintFromCalcElevationResult();
                        ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                        ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
                    }
                }
            }
        }




        /// <summary>
        /// 计算平面调整量
        /// </summary>
        public void CalcPP() {
            //frmMain.tendLinePP.Rows.Clear();
            //frmMain.slopePP.Rows.Clear();
            //frmMain.simulationAdjustmentPP.Rows.Clear();
            //frmMain.actualAdjustmentPP.Rows.Clear();
            //frmMain.basicTrackAdjustablePP.Rows.Clear();
            CalcHelper calc = new CalcHelper();
            string[] colNamePP = { "PPMiles", "PPBias" };
            calc.CalaTendLine(dgvPPAdjustLine, frmMain.tendLinePP, colNamePP);
            calc.CalcSlope(frmMain.tendLinePP, frmMain.slopePP);
            calc.CalcPPSimulationAdjustment(frmMain.averageValue,
                frmMain.simulationAdjustmentPP, frmMain.slopePP, frmMain.tendLinePP, frmMain.datumTrack);
            calc.CalcPPActualAdjustment(frmMain.actualAdjustmentPP, frmMain.averageValue,
                frmMain.simulationAdjustmentPP, frmMain.datumTrack, double.Parse(frmMain.parameter.Rows[4][1].ToString()));
            calc.CalcPPBasicTrackAdjustable(frmMain.averageValue, frmMain.railFastening,
                frmMain.simulationAdjustmentPP, frmMain.basicTrackAdjustablePP, frmMain.datumTrack);
            Tuple<DataTable, int, double> tuple = CalcHelper.CalcOverrunResult(frmMain.basicTrackAdjustablePP, frmMain.actualAdjustmentPP, frmMain.datumTrack);
            frmMain.overrunPP = tuple.Item1;
            maxIndex = tuple.Item2;
            maxOverrun = tuple.Item3;
            dgvOvernum_PP.Rows.Clear();
            for (int i = 0; i < frmMain.overrunPP.Rows.Count; i++) {
                int index = dgvOvernum_PP.Rows.Add();
                for (int j = 1; j < frmMain.overrunPP.Columns.Count; j++) {
                    dgvOvernum_PP.Rows[index].Cells[j - 1].Value = frmMain.overrunPP.Rows[i][j];
                }
            }
            txtovernum_pp.Text = "数量：" + dgvOvernum_PP.Rows.Count.ToString();
        }

        /// <summary>
        /// 把平面调整量计算结果画出来
        /// </summary>
        /// <param name="ctcl"></param>
        public void PaintFromCalcPPResult() {
            string ppAdj;
            if (frmMain.datumTrack == 0) ppAdj = "调左平面";
            else ppAdj = "调右平面";
            ChartHelper.AddOneSeries(frmMain.averageValue, ctclPPAdjLine, "调前平面", "调前平面");
            ChartHelper.AddOneSeries(frmMain.actualAdjustmentPP, ctclPPAdjLine, "调整量", ppAdj);
            ChartHelper.AddOneSeries(frmMain.basicTrackAdjustablePP, ctclPPAdjLine, "右界", "基准股右调");
            ChartHelper.AddOneSeries(frmMain.basicTrackAdjustablePP, ctclPPAdjLine, "左界", "基准股左调");
            try {
                int m = frmMain.datumTrack == 0 ? 1 : 2;
                for (int i = 0; i < ctclPPAdjLine.Series["调整量"].Points.Count; i++) {
                    double positive = Convert.ToDouble(ctclPPAdjLine.Series["右界"].Points[i].Values[0]);
                    double negative = Convert.ToDouble(ctclPPAdjLine.Series["左界"].Points[i].Values[0]);
                    double num = Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i][m]);
                    if (num > positive || num < negative) {
                        ctclPPAdjLine.Series["调整量"].Points[i].Color = Color.Red;
                    }
                }
            }
            catch (Exception) {
                return;
            }
        }

        /// <summary>
        /// 计算高程平面调整量
        /// </summary>
        public void CalcElevation() {
            //frmMain.tendLineElevation.Rows.Clear();
            //frmMain.slopeElevation.Rows.Clear();
            //frmMain.simulationAdjustmentElevation.Rows.Clear();
            //frmMain.actualAdjustmentElevation.Rows.Clear();
            //frmMain.basicTrackAdjustableElevation.Rows.Clear();
            CalcHelper calc = new CalcHelper();
            string[] colNameElevation = { "ElevationMiles", "ElevationBias" };
            calc.CalaTendLine(dgvElevationLine, frmMain.tendLineElevation, colNameElevation);
            calc.CalcSlope(frmMain.tendLineElevation, frmMain.slopeElevation);
            calc.CalcElevationSimulationAdjustment(frmMain.averageValue,
                frmMain.simulationAdjustmentElevation, frmMain.slopeElevation,
                frmMain.tendLineElevation, frmMain.datumTrack);
            calc.CalcElevationActualAdjustment(frmMain.actualAdjustmentElevation, frmMain.averageValue,
                frmMain.simulationAdjustmentElevation, frmMain.datumTrack, double.Parse(frmMain.parameter.Rows[4][1].ToString()));
            calc.CalcElevationBasicTrackAdjustable(frmMain.averageValue, frmMain.railFastening,
                frmMain.simulationAdjustmentElevation, frmMain.basicTrackAdjustableElevation, frmMain.datumTrack);
            Tuple<DataTable, int, double> tuple = CalcHelper.CalcOverrunResult(frmMain.basicTrackAdjustableElevation, frmMain.actualAdjustmentElevation, frmMain.datumTrack);
            frmMain.overrunEle = tuple.Item1;
            dgvovernum_Ele.Rows.Clear();
            for (int i = 0; i < frmMain.overrunEle.Rows.Count; i++) {
                int index = dgvovernum_Ele.Rows.Add();
                for (int j = 1; j < frmMain.overrunEle.Columns.Count; j++) {
                    dgvovernum_Ele.Rows[index].Cells[j - 1].Value = frmMain.overrunEle.Rows[i][j];
                }
            }
            txtovernum_ele.Text = "数量：" + dgvovernum_Ele.Rows.Count.ToString();
        }
        /// <summary>
        /// 把高程调整量计算结果画出来
        /// </summary>
        /// <param name="ctcl"></param>
        public void PaintFromCalcElevationResult() {
            string elevationAdj = null;
            if (frmMain.datumTrack == 0) elevationAdj = "调左高程";
            else elevationAdj = "调右高程";
            ChartHelper.AddOneSeries(frmMain.averageValue, ctclElevation, "调前高程", "调前高程");
            ChartHelper.AddOneSeries(frmMain.actualAdjustmentElevation, ctclElevation, "调整量", elevationAdj);
            ChartHelper.AddOneSeries(frmMain.basicTrackAdjustableElevation, ctclElevation, "下界", "基准股下调");
            ChartHelper.AddOneSeries(frmMain.basicTrackAdjustableElevation, ctclElevation, "上界", "基准股上调");
            try {
                int m = frmMain.datumTrack == 0 ? 1 : 2;
                for (int i = 0; i < ctclElevation.Series["调整量"].Points.Count; i++) {
                    double positive = Convert.ToDouble(ctclElevation.Series["下界"].Points[i].Values[0]);
                    double negative = Convert.ToDouble(ctclElevation.Series["上界"].Points[i].Values[0]);
                    double num = Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i][m]);
                    if (num > positive || num < negative) {
                        ctclElevation.Series["调整量"].Points[i].Color = Color.Red;
                    }
                }
            }
            catch (Exception) {
                return;
            }
        }

        /// <summary>
        /// 保存平面调整方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPPSave_Click(object sender, EventArgs e) {
            if (frmMain.tendLinePP.Rows.Count == 0) {
                MessageBox.Show("没有检测到数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmMain.AdjustPlanPP.Rows.Clear();
            for (int i = 0; i < frmMain.averageValue.Rows.Count; i++) {
                DataRow dr = frmMain.AdjustPlanPP.NewRow();
                if (i < frmMain.railFastening.Rows.Count) {
                    dr["测量点"] = frmMain.railFastening.Rows[i]["轨枕数"];
                }
                else {
                    dr["测量点"] = -1;
                }
                dr["里程"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["里程"].ToString());
                if (i < frmMain.railFastening.Rows.Count)
                    dr["轨枕编号"] = frmMain.railFastening.Rows[i]["轨枕编号"].ToString();
                else
                    dr["轨枕编号"] = "";
                dr["调前平面"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前平面"].ToString());
                dr["调前轨距"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前轨距"].ToString());
                dr["调左平面"] = Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调左平面"].ToString());
                dr["调右平面"] = Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调右平面"].ToString());
                switch (frmMain.datumTrack) {
                    case 0: {
                            dr["调后平面"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前平面"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调左平面"].ToString());
                            dr["调后轨距"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前轨距"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调左平面"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调右平面"].ToString());
                            dr["基准股"] = "左";
                            break;
                        }
                    case 1: {
                            dr["调后平面"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前平面"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调右平面"].ToString());
                            dr["调后轨距"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前轨距"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调左平面"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentPP.Rows[i]["调右平面"].ToString());
                            dr["基准股"] = "右";
                            break;
                        }
                    default:
                        break;
                }
                frmMain.AdjustPlanPP.Rows.Add(dr);
            }
            Tuple<DataTable, int, double> tuple = CalcHelper.CalcOverrunResult(frmMain.basicTrackAdjustablePP, frmMain.AdjustPlanPP, frmMain.datumTrack);
            frmMain.overrunPP = tuple.Item1;
            IOHelper.DataTableExportToTxt(frmMain.tendLinePP, frmMain.path + @"调整文件\平面调整中线.txt");
            IOHelper.Slope2Txt(frmMain.slopePP, frmMain.path + @"调整文件\平面调整中线斜率.txt");
            IOHelper.DataTableExportToTxt(frmMain.simulationAdjustmentPP, frmMain.path + @"调整文件\平面模拟调整量.txt");
            IOHelper.DataTableExportToTxt(frmMain.actualAdjustmentPP, frmMain.path + @"调整文件\平面模拟调整量取整.txt");
            IOHelper.DataTableExportToTxt(frmMain.basicTrackAdjustablePP, frmMain.path + @"调整文件\平面基准股可调量.txt");
            IOHelper.Export2Adj(frmMain.AdjustPlanPP, frmMain.path + @"调整文件\平面调整方案.txt");
            MessageBox.Show("保存成功！", "保存提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 保存高程调整方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevationSave_Click(object sender, EventArgs e) {
            if (frmMain.tendLineElevation.Rows.Count == 0) {
                MessageBox.Show("没有检测到数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmMain.AdjustPlanElevation.Rows.Clear();
            for (int i = 0; i < frmMain.averageValue.Rows.Count; i++) {
                DataRow dr = frmMain.AdjustPlanElevation.NewRow();
                if (i < frmMain.railFastening.Rows.Count) {
                    dr["测量点"] = frmMain.railFastening.Rows[i]["轨枕数"];
                }
                else {
                    dr["测量点"] = -1;
                }
                dr["里程"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["里程"].ToString());
                if (i < frmMain.railFastening.Rows.Count)
                    dr["轨枕编号"] = frmMain.railFastening.Rows[i]["轨枕编号"].ToString();
                else
                    dr["轨枕编号"] = " ";
                dr["调前高程"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前高程"].ToString());
                dr["调前水平"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前水平"].ToString());
                dr["调左高程"] = Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调左高程"].ToString());
                dr["调右高程"] = Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调右高程"].ToString());
                switch (frmMain.datumTrack) {
                    case 0: {
                            dr["调后高程"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前高程"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调左高程"].ToString());
                            dr["调后水平"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前水平"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调左高程"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调右高程"].ToString());
                            dr["基准股"] = "左";
                            break;
                        }
                    case 1: {
                            dr["调后高程"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前高程"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调右高程"].ToString());
                            dr["调后水平"] = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前水平"].ToString()) -
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调左高程"].ToString()) +
                                Convert.ToDouble(frmMain.actualAdjustmentElevation.Rows[i]["调右高程"].ToString());
                            dr["基准股"] = "右";
                            break;
                        }
                    default:
                        break;
                }
                frmMain.AdjustPlanElevation.Rows.Add(dr);
            }
            Tuple<DataTable, int, double> tuple = CalcHelper.CalcOverrunResult(frmMain.basicTrackAdjustableElevation, frmMain.AdjustPlanElevation, frmMain.datumTrack);
            frmMain.overrunEle = tuple.Item1;
            IOHelper.DataTableExportToTxt(frmMain.tendLineElevation, frmMain.path + @"调整文件\高程调整中线.txt");
            IOHelper.Slope2Txt(frmMain.slopeElevation, frmMain.path + @"调整文件\高程调整中线斜率.txt");
            IOHelper.DataTableExportToTxt(frmMain.actualAdjustmentElevation, frmMain.path + @"调整文件\高程模拟调整量取整.txt");
            IOHelper.DataTableExportToTxt(frmMain.basicTrackAdjustableElevation, frmMain.path + @"调整文件\高程基准股可调量.txt");
            IOHelper.Export2Adj(frmMain.AdjustPlanElevation, frmMain.path + @"调整文件\高程调整方案.txt");
            IOHelper.DataTableExportToTxt(frmMain.simulationAdjustmentElevation, frmMain.path + @"调整文件\高程模拟调整量.txt");
            MessageBox.Show("保存成功！", "保存提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FrmAddAdjustLine_FormClosing(object sender, FormClosingEventArgs e) {
            if (frmMain.AdjustPlanPP.Rows.Count == 0 && frmMain.tendLinePP.Rows.Count > 0) {
                DialogResult result = MessageBox.Show("平面调整结果尚未保存，是否保存？", "关闭提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    btnPPSave_Click(this, e);
                    this.Close();
                }
                else if (result == DialogResult.Cancel) {
                    e.Cancel = true;
                }
            }

            if (frmMain.AdjustPlanElevation.Rows.Count == 0 && frmMain.tendLineElevation.Rows.Count > 0) {
                DialogResult result = MessageBox.Show("高程调整结果尚未保存，是否保存？", "关闭提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    btnElevationSave_Click(this, e);
                    this.Close();
                }
                else if (result == DialogResult.Cancel) {
                    e.Cancel = true;
                }
            }
        }




        private void cbxX_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xy = ctclElevation.Diagram as XYDiagram;
            if (cbxElevationX.Checked) {
                xy.EnableAxisXZooming = true;
            }
            else {
                xy.EnableAxisXZooming = false;
            }
        }

        private void cbxY_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xy = ctclElevation.Diagram as XYDiagram;
            if (cbxElevationY.Checked) {
                xy.EnableAxisYZooming = true;
            }
            else {
                xy.EnableAxisYZooming = false;
            }
        }

        private void cbxPPX_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xy = ctclPPAdjLine.Diagram as XYDiagram;
            if (cbxPPX.Checked) {
                xy.EnableAxisXZooming = true;
            }
            else {
                xy.EnableAxisXZooming = false;
            }
        }

        private void cbxPPY_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xy = ctclPPAdjLine.Diagram as XYDiagram;
            if (cbxPPY.Checked) {
                xy.EnableAxisYZooming = true;
            }
            else {
                xy.EnableAxisYZooming = false;
            }
        }

        private void InitPP() {
            dgvPPAdjustLine.DataSource = null;
            for (int i = 0; i < frmMain.tendLinePP.Rows.Count; i++) {
                int index = dgvPPAdjustLine.Rows.Add();
                for (int j = 0; j < frmMain.tendLinePP.Columns.Count; j++) {
                    dgvPPAdjustLine.Rows[index].Cells[j].Value = frmMain.tendLinePP.Rows[index][j];
                }
            }
            GetBend(dgvPPAdjustLine, chordLength);
            CalcPP();
            ChartHelper.AddOneSeries(frmMain.tendLinePP, ctclPPAdjLine, "调整中线", "偏差值");
            PaintFromCalcPPResult();
            ChartHelper.SetLegendAndXY(ctclPPAdjLine);
            ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            //ChartHelper.AddOneSeries(frmMain.averageValue, ctclPPAdjLine, "调前平面", "调前平面");
            string[] columnPPName = { "里程", "调前平面" };
            ChartHelper.AddDtToDgv(frmMain.averageValue, dgvPPData, columnPPName);

        }

        private void InitElevation() {
            dgvElevationLine.DataSource = null;
            for (int i = 0; i < frmMain.tendLineElevation.Rows.Count; i++) {
                int index = dgvElevationLine.Rows.Add();
                for (int j = 0; j < frmMain.tendLineElevation.Columns.Count; j++) {
                    dgvElevationLine.Rows[index].Cells[j].Value = frmMain.tendLineElevation.Rows[index][j];
                }
            }
            GetBend(dgvElevationLine, chordLength);
            CalcElevation();
            ChartHelper.AddOneSeries(frmMain.tendLineElevation, ctclElevation, "调整中线", "偏差值");
            PaintFromCalcElevationResult();
            ChartHelper.SetLegendAndXY(ctclElevation);
            ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            //ChartHelper.AddOneSeries(frmMain.averageValue, ctclElevation, "调前高程", "调前高程");
            string[] columnElevationName = { "里程", "调前高程" };
            ChartHelper.AddDtToDgv(frmMain.averageValue, dgvElevationData, columnElevationName);

        }

        private void 调整中线ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                ppColor[4] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            }
        }

        private void 调前平面ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                ppColor[0] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            }
        }

        private void 调整量ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                ppColor[1] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            }
        }

        private void 右界ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                ppColor[2] = colorDia.Color;
                ppColor[3] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            }
        }

        private void 调前高程ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                eleColor[0] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            }
        }

        private void 调整量ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                eleColor[1] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            }
        }

        private void 上下界ToolStripMenuItem_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                eleColor[2] = colorDia.Color;
                eleColor[3] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            }
        }

        private void 调整中线ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ColorDialog colorDia = new ColorDialog();
            if (colorDia.ShowDialog() == DialogResult.OK) {
                //获取所选择的颜色
                eleColor[4] = colorDia.Color;
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            }
        }

        private void 恢复默认ToolStripMenuItem_Click(object sender, EventArgs e) {
            ppColor[0] = Color.FromArgb(255, 156, 0);
            ppColor[1] = Color.FromArgb(0, 139, 222);
            ppColor[2] = Color.FromArgb(1, 191, 191);
            ppColor[3] = Color.FromArgb(1, 191, 191);
            ppColor[4] = Color.FromArgb(237, 3, 2);
            ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
        }

        private void 恢复默认ToolStripMenuItem1_Click(object sender, EventArgs e) {
            eleColor[0] = Color.FromArgb(255, 156, 0);
            eleColor[1] = Color.FromArgb(0, 139, 222);
            eleColor[2] = Color.FromArgb(1, 191, 191);
            eleColor[3] = Color.FromArgb(1, 191, 191);
            eleColor[4] = Color.FromArgb(237, 3, 2);
            ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
        }

        private void btnPPImport_Click(object sender, EventArgs e) {
            string file = IOHelper.OpenFileDia("请选择平面调整中线文件");
            if (!string.IsNullOrEmpty(file)) {
                IOHelper.ImportAdjLine(file, dgvPPAdjustLine);
                frmMain.AdjustPlanPP.Rows.Clear();
                dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvPPAdjustLine, chordLength);
                //ChartHelper.DelDgvDuplicateLine(dgvPPAdjustLine);//删除重复行
                //记录当前图例复选框的状态
                ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                //画图
                PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                //计算调整量
                CalcPP();
                //画调整量图像
                PaintFromCalcPPResult();
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                //设置图例复选框状态
                ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
            }
        }

        /// <summary>
        /// 导入调整中线文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEleImport_Click(object sender, EventArgs e) {
            string file = IOHelper.OpenFileDia("请选择高程调整中线文件");
            if (!string.IsNullOrEmpty(file)) {
                frmMain.AdjustPlanElevation.Rows.Clear();
                IOHelper.ImportAdjLine(file, dgvElevationLine);
                //记录当前图例复选框的状态
                ChartHelper.LogEleChecked(ctclElevation, EleChecked);
                //画图
                dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvElevationLine, chordLength);
                PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                //计算调整量
                CalcElevation();
                //画调整量图像
                PaintFromCalcElevationResult();
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
            }
        }

        static double p_x, p_y, e_x, e_y, coordp_x_down, coordp_x_up, coordp_y_up, coorde_x_down, coorde_x_up, coorde_y_up;

        private void btnCalc_Click(object sender, EventArgs e) {
            Console.WriteLine(maxIndex);
            Console.WriteLine(maxOverrun);


            /*List<Point> pointList = new List<Point>();
            for (int i = 0; i < frmMain.averageValue.Rows.Count; i++) {
                Point point = new Point();
                point.Lng = Convert.ToDouble(frmMain.averageValue.Rows[i]["调前平面"]);
                point.Lat = Convert.ToDouble(frmMain.averageValue.Rows[i]["里程"]);
                pointList.Add(point);
            }
            DouglasPeucker douglas = new DouglasPeucker(pointList, 8);
            var zhedian = douglas.Compress();
            dgvPPAdjustLine.Rows.Clear();
            foreach (var item in zhedian) {
                var index = dgvPPAdjustLine.Rows.Add();
                dgvPPAdjustLine.Rows[index].Cells[0].Value = item.Lat;
                dgvPPAdjustLine.Rows[index].Cells[1].Value = item.Lng;
            }
            dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
            GetBend(dgvPPAdjustLine, chordLength);
            //ChartHelper.DelDgvDuplicateLine(dgvPPAdjustLine);//删除重复行
            //记录当前图例复选框的状态
            ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
            //画图
            PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
            //计算调整量
            CalcPP();
            //画调整量图像
            PaintFromCalcPPResult();
            ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            //设置图例复选框状态
            ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);*/
        }

        private void dgvovernum_Ele_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvovernum_Ele, e);
        }

        private void dgvOvernum_PP_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvOvernum_PP, e);
        }

        private void dgvElevationLine_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex <= 0 || e.RowIndex <= 0) {
                return;
            }
            DataGridViewCell cell = dgvElevationLine.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.ColumnIndex == 2 && !string.IsNullOrEmpty(cell.ErrorText)) {
                if (MessageBox.Show($"该点70m弦挠度为{cell.Value}mm,挠度限差为{bendTol}mm,是否修正该点?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    CalcHelper.Point ps = new CalcHelper.Point() {
                        x = double.Parse(dgvElevationLine.Rows[e.RowIndex - 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgvElevationLine.Rows[e.RowIndex - 1].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pm = new CalcHelper.Point() {
                        x = double.Parse(dgvElevationLine.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        y = double.Parse(dgvElevationLine.Rows[e.RowIndex].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pe = new CalcHelper.Point() {
                        x = double.Parse(dgvElevationLine.Rows[e.RowIndex + 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgvElevationLine.Rows[e.RowIndex + 1].Cells[1].Value.ToString())
                    };
                    double[] l = CalcHelper.CalcLine(ps, pe);
                    double miX = Math.Round((ps.x + pe.x) / 2, 3);
                    double miY = Math.Round((ps.y + pe.y) / 2, 3);
                    double yMaxOrMin = pm.y;
                    double angle, bend;

                    double yMin, yMax, xMin, xMax;
                    if (pm.x > miX) {
                        xMin = miX;
                        xMax = pm.x;
                    }
                    else {
                        xMin = pm.x;
                        xMax = miX;
                    }
                    while (true) {
                        if (pm.x > miX) {
                            pm.x--;
                            if (pm.x <= xMin) {
                                MessageBox.Show("调整失败，请手动调整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else {
                            pm.x++;
                            if (pm.x >= xMax) {
                                MessageBox.Show("调整失败，请手动调整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        if (pm.y > ps.y) {
                            yMin = -(l[0] * pm.x + l[2]) / l[1];
                            yMax = yMaxOrMin;
                            pm.y = yMax;
                            while (pm.y > yMin) {
                                pm.y -= 0.1;
                                angle = CalcHelper.CalcAngle(ps, pm, pe);
                                bend = CalcHelper.CalcBend(angle, chordLength);
                                if (bend < bendTol) {
                                    dgvElevationLine.Rows[e.RowIndex].Cells[0].Value = "";
                                    dgvElevationLine.Rows[e.RowIndex].Cells[1].Value = "";

                                    dgvElevationLine.Rows[e.RowIndex].Cells[0].Value = Math.Round(pm.x, 3);
                                    dgvElevationLine.Rows[e.RowIndex].Cells[1].Value = Math.Round(pm.y, 3);
                                    GetBend(dgvElevationLine, chordLength);
                                    //记录当前图例复选框的状态
                                    ChartHelper.LogChecked(ctclElevation, EleChecked);
                                    //画图
                                    PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                                    //计算调整量
                                    CalcElevation();
                                    //画调整量图像
                                    PaintFromCalcElevationResult();
                                    ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                                    //设置图例复选框状态
                                    ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
                                    return;
                                }
                            }
                        }
                        else {
                            yMin = yMaxOrMin;
                            yMax = -(l[0] * pm.x + l[2]) / l[1];
                            pm.y = yMin;
                            while (pm.y < yMax) {
                                pm.y += 0.1;
                                angle = CalcHelper.CalcAngle(ps, pm, pe);
                                bend = CalcHelper.CalcBend(angle, chordLength);
                                if (bend < bendTol) {
                                    dgvElevationLine.Rows[e.RowIndex].Cells[0].Value = "";
                                    dgvElevationLine.Rows[e.RowIndex].Cells[1].Value = "";

                                    dgvElevationLine.Rows[e.RowIndex].Cells[0].Value = Math.Round(pm.x, 3);
                                    dgvElevationLine.Rows[e.RowIndex].Cells[1].Value = Math.Round(pm.y, 3);
                                    GetBend(dgvElevationLine, chordLength);
                                    //记录当前图例复选框的状态
                                    ChartHelper.LogChecked(ctclElevation, EleChecked);
                                    //画图
                                    PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                                    //计算调整量
                                    CalcElevation();
                                    //画调整量图像
                                    PaintFromCalcElevationResult();
                                    ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                                    //设置图例复选框状态
                                    ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
                                    return;
                                }
                            }
                        }
                    }
                }
            }


            //if (e.ColumnIndex < 0) {
            //    return;
            //}
            //DataGridViewCell cell = dgvElevationLine.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //if (e.ColumnIndex == 2 && !string.IsNullOrEmpty(cell.ErrorText)) {
            //    int index = dgvElevationLine.CurrentCell.RowIndex;
            //    Series series = ctclElevation.Series["调整中线"];
            //    if (series != null && series.Points != null) {
            //        //ctclElevation.Series["调整中线"].Points[index]
            //        SeriesPointCollection seriesPoint = series.Points;
            //        SeriesPoint point = seriesPoint[index];
            //        MessageBox.Show(index + "\nx:" + point.Argument + "\ny:" + point.ValuesSerializable);
            //    }
            //}
        }

        private void dgvPPAdjustLine_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex <= 0 || e.RowIndex <= 0) {
                return;
            }
            DataGridViewCell cell = dgvPPAdjustLine.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.ColumnIndex == 2 && !string.IsNullOrEmpty(cell.ErrorText)) {
                if (MessageBox.Show($"该点70m弦挠度为{cell.Value}mm,挠度限差为{bendTol}mm,是否修正该点?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    CalcHelper.Point ps = new CalcHelper.Point() {
                        x = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex - 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex - 1].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pm = new CalcHelper.Point() {
                        x = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        y = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pe = new CalcHelper.Point() {
                        x = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex + 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgvPPAdjustLine.Rows[e.RowIndex + 1].Cells[1].Value.ToString())
                    };
                    double[] l = CalcHelper.CalcLine(ps, pe);
                    double miX = Math.Round((ps.x + pe.x) / 2, 3);
                    double miY = Math.Round((ps.y + pe.y) / 2, 3);
                    double yMaxOrMin = pm.y;
                    double angle, bend;

                    double yMin, yMax, xMin, xMax;
                    if (pm.x > miX) {
                        xMin = miX;
                        xMax = pm.x;
                    }
                    else {
                        xMin = pm.x;
                        xMax = miX;
                    }
                    while (true) {
                        if (pm.x > miX) {
                            pm.x--;
                            if (pm.x <= xMin) {
                                MessageBox.Show("调整失败，请手动调整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else {
                            pm.x++;
                            if (pm.x >= xMax) {
                                MessageBox.Show("调整失败，请手动调整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        if (pm.y > ps.y) {
                            yMin = -(l[0] * pm.x + l[2]) / l[1];
                            yMax = yMaxOrMin;
                            pm.y = yMax;
                            while (pm.y > yMin) {
                                pm.y -= 0.1;
                                angle = CalcHelper.CalcAngle(ps, pm, pe);
                                bend = CalcHelper.CalcBend(angle, chordLength);
                                if (bend < bendTol) {
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[0].Value = "";
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[1].Value = "";

                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[0].Value = Math.Round(pm.x, 3);
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[1].Value = Math.Round(pm.y, 3);
                                    GetBend(dgvPPAdjustLine, chordLength);
                                    //记录当前图例复选框的状态
                                    ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                                    //画图
                                    PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                                    //计算调整量
                                    CalcPP();
                                    //画调整量图像
                                    PaintFromCalcPPResult();
                                    ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                                    //设置图例复选框状态
                                    ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
                                    return;
                                }
                            }
                        }
                        else {
                            yMin = yMaxOrMin;
                            yMax = -(l[0] * pm.x + l[2]) / l[1];
                            pm.y = yMin;
                            while (pm.y < yMax) {
                                pm.y += 0.1;
                                angle = CalcHelper.CalcAngle(ps, pm, pe);
                                bend = CalcHelper.CalcBend(angle, chordLength);
                                if (bend < bendTol) {
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[0].Value = "";
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[1].Value = "";

                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[0].Value = Math.Round(pm.x, 3);
                                    dgvPPAdjustLine.Rows[e.RowIndex].Cells[1].Value = Math.Round(pm.y, 3);
                                    GetBend(dgvPPAdjustLine, chordLength);
                                    //记录当前图例复选框的状态
                                    ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                                    //画图
                                    PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                                    //计算调整量
                                    CalcPP();
                                    //画调整量图像
                                    PaintFromCalcPPResult();
                                    ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                                    //设置图例复选框状态
                                    ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 计算挠度
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="chordLength"></param>
        public static void GetBend(DataGridView dgv, double chordLength) {
            int length = dgv.Rows.Count;
            if (length > 2) {
                for (int i = 0; i < length; i++) {
                    dgv.Rows[i].Cells[2].Value = string.Empty;
                    dgv.Rows[i].Cells[2].ErrorText = null;
                }
                for (int i = 1; i < length - 1; i++) {
                    CalcHelper.Point ps = new CalcHelper.Point() {
                        x = double.Parse(dgv.Rows[i - 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgv.Rows[i - 1].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pm = new CalcHelper.Point() {
                        x = double.Parse(dgv.Rows[i].Cells[0].Value.ToString()),
                        y = double.Parse(dgv.Rows[i].Cells[1].Value.ToString())
                    };
                    CalcHelper.Point pe = new CalcHelper.Point() {
                        x = double.Parse(dgv.Rows[i + 1].Cells[0].Value.ToString()),
                        y = double.Parse(dgv.Rows[i + 1].Cells[1].Value.ToString())
                    };
                    double angle = CalcHelper.CalcAngle(ps, pm, pe);
                    double bend = CalcHelper.CalcBend(angle, chordLength);
                    dgv.Rows[i].Cells[2].Value = bend;
                    if (Convert.ToDouble(dgv.Rows[i].Cells[2].Value) > bendTol) {
                        dgv.Rows[i].Cells[2].ErrorText = "挠度超限";
                    }
                }
            }
        }


        /// <summary>
        /// 平面鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctclPPAdjLine_MouseDown(object sender, MouseEventArgs e) {
            p_x = e.X;
            p_y = e.Y;
            DiagramCoordinates coords = ((XYDiagram2D)ctclPPAdjLine.Diagram).PointToDiagram(ctclPPAdjLine.PointToClient(Cursor.Position));
            coordp_x_down = Math.Round(coords.NumericalArgument, 3);
        }

        /// <summary>
        /// 高程鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctclElevation_MouseDown(object sender, MouseEventArgs e) {
            e_x = e.X;
            e_y = e.Y;
            DiagramCoordinates coords = ((XYDiagram2D)ctclElevation.Diagram).PointToDiagram(ctclElevation.PointToClient(Cursor.Position));
            coorde_x_down = Math.Round(coords.NumericalArgument, 3);
        }

        /// <summary>
        /// 高程点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctclElevation_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                return;
            }
            DiagramCoordinates coords = ((XYDiagram2D)ctclElevation.Diagram).PointToDiagram(ctclElevation.PointToClient(Cursor.Position));
            coorde_x_up = Math.Round(coords.NumericalArgument, 3);
            coorde_y_up = Math.Round(coords.NumericalValue, 3);
            if (coorde_x_up == 0) {
                return;
            }
            int count = dgvElevationLine.Rows.Count;
            if (Math.Round(coorde_x_up, 1) <= Math.Round(double.Parse(dgvElevationLine.Rows[0].Cells[0].Value.ToString()), 1) - 100 ||
                Math.Round(coorde_x_up, 1) >= Math.Round(double.Parse(dgvElevationLine.Rows[count - 1].Cells[0].Value.ToString()), 1) + 100) {
                MessageBox.Show("超出范围，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (e_x == e.X && e_y == e.Y && (Control.ModifierKeys != Keys.Control)) {

                for (int i = 0; i < count; i++) {
                    if (Math.Round(double.Parse(dgvElevationLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(coorde_x_up, 1)) {
                        MessageBox.Show("里程重复，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                frmMain.AdjustPlanElevation.Rows.Clear();
                int index2 = this.dgvElevationLine.Rows.Add();
                dgvElevationLine.Rows[index2].Cells[0].Value = coorde_x_up;
                dgvElevationLine.Rows[index2].Cells[1].Value = coorde_y_up;
                //画图
                dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvElevationLine, chordLength);
                ChartHelper.LogEleChecked(ctclElevation, EleChecked);
                PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                //计算调整量
                CalcElevation();
                //画调整量图像
                PaintFromCalcElevationResult();
                ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
            }
            else {
                if ((ModifierKeys & Keys.Control) == Keys.Control) {
                    bool isAdd = false;
                    for (int i = 0; i < dgvElevationLine.Rows.Count; i++) {
                        if (Math.Abs(Math.Round(double.Parse(dgvElevationLine.Rows[i].Cells[0].Value.ToString()), 1) - Math.Round(coorde_x_down, 1)) <= tol) {
                            DataGridViewRow row = dgvElevationLine.Rows[i];
                            dgvElevationLine.Rows.Remove(row);//删除行  
                            isAdd = true;
                            break;
                        }
                    }
                    if (isAdd) {
                        frmMain.AdjustPlanElevation.Rows.Clear();
                        int index2 = this.dgvElevationLine.Rows.Add();
                        dgvElevationLine.Rows[index2].Cells[0].Value = coorde_x_up;
                        dgvElevationLine.Rows[index2].Cells[1].Value = coorde_y_up;
                        //画图
                        dgvElevationLine.Sort(dgvElevationLine.Columns[0], ListSortDirection.Ascending);
                        GetBend(dgvElevationLine, chordLength);
                        ChartHelper.LogEleChecked(ctclElevation, EleChecked);
                        PaintFromDgv(dgvElevationLine, ctclElevation, "ElevationMiles", "ElevationBias");
                        //计算调整量
                        CalcElevation();
                        //画调整量图像
                        PaintFromCalcElevationResult();
                        ChartHelper.SetSeriesColor(ctclElevation, eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
                        ChartHelper.SetEleCheckBoxes(ctclElevation, EleChecked);
                    }
                }
            }
        }

        /// <summary>
        /// 平面点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctclPPAdjLine_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                return;
            }
            DiagramCoordinates coords = ((XYDiagram2D)ctclPPAdjLine.Diagram).PointToDiagram(ctclPPAdjLine.PointToClient(Cursor.Position));
            coordp_x_up = Math.Round(coords.NumericalArgument, 3);
            coordp_y_up = Math.Round(coords.NumericalValue, 3);
            if (coordp_x_up == 0) {
                return;
            }
            int count = dgvPPAdjustLine.Rows.Count;
            if (Math.Round(coordp_x_up, 1) <= Math.Round(double.Parse(dgvPPAdjustLine.Rows[0].Cells[0].Value.ToString()), 1) - 100 ||
                Math.Round(coordp_x_up, 1) >= Math.Round(double.Parse(dgvPPAdjustLine.Rows[count - 1].Cells[0].Value.ToString()), 1) + 100) {
                MessageBox.Show("超出范围，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (p_x == e.X && p_y == e.Y && (Control.ModifierKeys != Keys.Control)) {
                for (int i = 0; i < count; i++) {
                    if (Math.Round(double.Parse(dgvPPAdjustLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(coordp_x_up, 1)) {
                        MessageBox.Show("里程重复，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                //记录当前图例复选框的状态
                ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                int index = this.dgvPPAdjustLine.Rows.Add();
                dgvPPAdjustLine.Rows[index].Cells[0].Value = coordp_x_up;
                dgvPPAdjustLine.Rows[index].Cells[1].Value = coordp_y_up;
                //画趋势折线
                dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
                GetBend(dgvPPAdjustLine, chordLength);
                PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                frmMain.AdjustPlanPP.Rows.Clear();
                //计算调整量
                CalcPP();
                //画调整量图像
                PaintFromCalcPPResult();
                ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                //设置图例复选框状态
                ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
            }
            else {
                if ((ModifierKeys & Keys.Control) == Keys.Control) {
                    bool isAdd = false;
                    for (int i = 0; i < dgvPPAdjustLine.Rows.Count; i++) {
                        if (Math.Abs(Math.Round(double.Parse(dgvPPAdjustLine.Rows[i].Cells[0].Value.ToString()), 1) - Math.Round(coordp_x_down, 1)) <= tol) {
                            DataGridViewRow row = dgvPPAdjustLine.Rows[i];
                            dgvPPAdjustLine.Rows.Remove(row);//删除行  
                            isAdd = true;
                            break;
                        }
                    }
                    if (isAdd) {
                        //记录当前图例复选框的状态
                        ChartHelper.LogChecked(ctclPPAdjLine, PPChecked);
                        int index = this.dgvPPAdjustLine.Rows.Add();
                        dgvPPAdjustLine.Rows[index].Cells[0].Value = coordp_x_up;
                        dgvPPAdjustLine.Rows[index].Cells[1].Value = coordp_y_up;
                        //画趋势折线
                        dgvPPAdjustLine.Sort(dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
                        GetBend(dgvPPAdjustLine, chordLength);
                        PaintFromDgv(dgvPPAdjustLine, ctclPPAdjLine, "PPMiles", "PPBias");
                        frmMain.AdjustPlanPP.Rows.Clear();
                        //计算调整量
                        CalcPP();
                        //画调整量图像
                        PaintFromCalcPPResult();
                        ChartHelper.SetSeriesColor(ctclPPAdjLine, ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
                        //设置图例复选框状态
                        ChartHelper.SetPPCheckBoxes(ctclPPAdjLine, PPChecked);
                    }
                }
            }
        }
    }


}

