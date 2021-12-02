using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace TrackRedesign {
    public partial class FrmCalcMaterial : Form {

        private static double derta;

        //定义二维数组(四行)，第一行表示型号，第二行表示调前。第三行表示调后，第四行表示调后-调前
        static double[,] pp;
        static double[,] ele;
        private DataTable dtAdjPP;
        private DataTable dtAdjEle;
        private DataTable dtFasten;
        private DataTable para;
        public DataTable dtAfterModel = new DataTable("调整后材料型号");
        public string path = string.Empty;
        public FrmCalcMaterial() {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void FrmCalcMaterial_Load(object sender, EventArgs e) {
            if (this.Tag != null) {
                TagObject tag = (TagObject)this.Tag;
                dtAdjPP = tag.PPAdjustPlan;
                dtAdjEle = tag.ElevationAdjustPlan;
                dtFasten = tag.Model;
                path = tag.Path;
                para = tag.Para;
            }
            derta = double.Parse(para.Rows[4][1].ToString());
            //初始化调后型号
            dtAfterModel.Columns.Add("里程", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("轨枕编号", Type.GetType("System.String"));
            dtAfterModel.Columns.Add("左股外口轨距块", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("左股里口轨距块", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("左股胶垫厚度", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("右股里口轨距块", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("右股外口轨距块", Type.GetType("System.Double"));
            dtAfterModel.Columns.Add("右股胶垫厚度", Type.GetType("System.Double"));
            ShowVariation();

            DirectoryInfo root = new DirectoryInfo(path + "调整方案");
            foreach (FileInfo file in root.GetFiles()) {
                if (file.Name.Contains($"轨距垫数量(极差为{para.Rows[4][1]})")) {
                    DataTable dtPP = new DataTable();
                    dtPP.Columns.Add("轨距垫型号", typeof(double));
                    dtPP.Columns.Add("对应厚度", typeof(string));
                    dtPP.Columns.Add("置换", typeof(int));
                    dtPP.Columns.Add("用量", typeof(int));
                    dtPP.Columns.Add("作差", typeof(int));

                    //读入文件  
                    using StreamReader sr = new StreamReader(path + $"调整方案\\轨距垫数量(极差为{para.Rows[4][1]}).txt", Encoding.UTF8);
                    string dataLine;
                    string[] dataArray;
                    while (sr.Peek() > -1) {
                        dataLine = sr.ReadLine();
                        if (!string.IsNullOrEmpty(dataLine) && IOHelper.IsNumberic(dataLine)) {
                            dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                            DataRow dr = dtPP.NewRow();
                            dr[0] = dataArray[0];
                            dr[1] = dataArray[1];
                            dr[2] = dataArray[2];
                            dr[3] = dataArray[3];
                            dr[4] = dataArray[4];
                            dtPP.Rows.Add(dr);
                        }
                    }
                    for (int i = 0; i < dtPP.Rows.Count; i++) {
                        int index = dgvPPNum.Rows.Add();
                        for (int j = 0; j < dtPP.Columns.Count; j++) {
                            dgvPPNum.Rows[index].Cells[j].Value = dtPP.Rows[index][j];
                        }
                    }
                }

                if (file.Name.Contains($"调高垫板数量(极差为{para.Rows[4][1]})")) {
                    DataTable dtEle = new DataTable();
                    dtEle.Columns.Add("调高垫板型号", typeof(double));
                    dtEle.Columns.Add("对应厚度", typeof(string));
                    dtEle.Columns.Add("置换", typeof(int));
                    dtEle.Columns.Add("用量", typeof(int));
                    dtEle.Columns.Add("作差", typeof(int));

                    //读入文件  
                    using StreamReader sr = new StreamReader(path + $"调整方案\\调高垫板数量(极差为{para.Rows[4][1]}).txt", Encoding.UTF8);
                    string dataLine;
                    string[] dataArray;
                    while (sr.Peek() > -1) {
                        dataLine = sr.ReadLine();
                        if (!string.IsNullOrEmpty(dataLine) && IOHelper.IsNumberic(dataLine)) {
                            dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                            DataRow dr = dtEle.NewRow();
                            dr[0] = dataArray[0];
                            dr[1] = dataArray[1];
                            dr[2] = dataArray[2];
                            dr[3] = dataArray[3];
                            dr[4] = dataArray[4];
                            dtEle.Rows.Add(dr);
                        }
                    }
                    for (int i = 0; i < dtEle.Rows.Count; i++) {
                        int index = dgvElevationNum.Rows.Add();
                        for (int j = 0; j < dtEle.Columns.Count; j++) {
                            dgvElevationNum.Rows[index].Cells[j].Value = dtEle.Rows[index][j];
                        }
                    }
                }
            }
        }

        private void dgvMetarial_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvMetarial, e);
        }

        /// <summary>
        /// 计算精调后材料厚度
        /// </summary>
        /// <param name="dtBefore"></param>
        /// <param name="dtAfter"></param>
        /// <param name="dgvDetar"></param>
        /// <returns></returns>
        private void CalcAfterMetarial() {
            for (int i = 0; i < dtFasten.Rows.Count; i++) {
                if (i<dgvMetarial.Rows.Count) {
                    DataRow dr = dtAfterModel.NewRow();
                    dr["里程"] = dtFasten.Rows[i]["里程"];
                    dr["轨枕编号"] = dtFasten.Rows[i]["轨枕编号"];
                    if (dtAdjPP.Rows.Count != 0) {
                        dr["左股外口轨距块"] = double.Parse(dtFasten.Rows[i]["左股外口轨距块"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[2].Value.ToString());
                        dr["左股里口轨距块"] = double.Parse(dtFasten.Rows[i]["左股里口轨距块"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[3].Value.ToString());
                        dr["右股里口轨距块"] = double.Parse(dtFasten.Rows[i]["右股里口轨距块"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[5].Value.ToString());
                        dr["右股外口轨距块"] = double.Parse(dtFasten.Rows[i]["右股外口轨距块"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[6].Value.ToString());
                    }
                    if (dtAdjEle.Rows.Count != 0) {
                        dr["左股胶垫厚度"] = double.Parse(dtFasten.Rows[i]["左股胶垫厚度"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[4].Value.ToString());
                        dr["右股胶垫厚度"] = double.Parse(dtFasten.Rows[i]["右股胶垫厚度"].ToString()) + double.Parse(dgvMetarial.Rows[i].Cells[7].Value.ToString());
                    }
                    dtAfterModel.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 计算调高垫板数量
        /// </summary>
        /// <returns></returns>
        private static void GetEleNum(DataTable dtBefore, DataTable dtAfter) {
            //计算精调前调高垫板数量
            for (int i = 5; i <= 8; i += 3) {
                for (int j = 0; j < dtBefore.Rows.Count; j++) {
                    if (double.Parse(dtBefore.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtBefore.Rows[j][i]) / derta) - 1;
                        ele[1,index]++;
                    }
                }
            }
            //计算精调后调高垫板数量
            for (int i = 4; i < 8; i += 3) {
                for (int j = 0; j < dtAfter.Rows.Count; j++) {
                    if (double.Parse(dtAfter.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtAfter.Rows[j][i]) / derta) - 1;
                        ele[2,index]++;
                    }
                }
            }
            //计算调高垫板数量
            for (int m = 0; m < ele.GetLength(1); m++) {
                ele[3, m] = ele[2, m] - ele[1, m];
            }
        }

        /// <summary>
        /// 计算轨距垫数量
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="dtModel"></param>
        /// <param name="dtAfterModel"></param>
        /// <returns></returns>
        private static void GetPPNum(DataTable dtBefore, DataTable dtAfter) {
            //计算精调前左股轨距垫数量
            for (int i = 3; i <= 4; i++) {
                for (int j = 0; j < dtBefore.Rows.Count; j++) {
                    if (double.Parse(dtBefore.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtBefore.Rows[j][i]) / derta) - 1;
                        pp[1, index]++;
                    }
                }
            }
            //计算精调前右股轨距垫数量
            for (int i = 6; i <= 7; i++) {
                for (int j = 0; j < dtBefore.Rows.Count; j++) {
                    if (double.Parse(dtBefore.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtBefore.Rows[j][i]) / derta) - 1;
                        pp[1, index]++;
                    }
                }
            }
            //计算精调后左股轨距垫数量
            for (int i = 2; i <= 3; i++) {
                for (int j = 0; j < dtAfter.Rows.Count; j++) {
                    if (double.Parse(dtAfter.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtAfter.Rows[j][i]) / derta) - 1;
                        pp[2, index]++;
                    }
                }
            }
            //计算精调后右股轨距垫数量
            for (int i = 5; i <= 6; i++) {
                for (int j = 0; j < dtAfter.Rows.Count; j++) {
                    if (double.Parse(dtAfter.Rows[j][i].ToString()) > 0) {
                        int index = Convert.ToInt32(Convert.ToDouble(dtAfter.Rows[j][i]) / derta) - 1;
                        pp[2, index]++;
                    }
                }
            }
            //计算轨距垫数量
            for (int i = 0; i < pp.GetLength(1); i++) {
                pp[3, i] = pp[2, i] - pp[1, i];
            }
        }

        /// <summary>
        ///显示变化量
        /// </summary>
        private void ShowVariation() {
            int count = dtAdjPP.Rows.Count >= dtAdjEle.Rows.Count ? dtAdjPP.Rows.Count : dtAdjEle.Rows.Count;
            //显示厚度变化量
            for (int i = 0; i < count; i++) {
                int index = dgvMetarial.Rows.Add();
                if (dtAdjPP.Rows.Count!=0) {
                    dgvMetarial.Rows[index].Cells[0].Value = dtAdjPP.Rows[index]["里程"];
                    dgvMetarial.Rows[index].Cells[1].Value = dtAdjPP.Rows[index]["轨枕编号"];
                }
                else if (dtAdjEle.Rows.Count != 0) {
                    dgvMetarial.Rows[index].Cells[0].Value = dtAdjEle.Rows[index]["里程"];
                    dgvMetarial.Rows[index].Cells[1].Value = dtAdjEle.Rows[index]["轨枕编号"];
                }
                else {
                    dgvMetarial.Rows[index].Cells[0].Value = "";
                    dgvMetarial.Rows[index].Cells[1].Value = "";
                }
                if (dtAdjPP.Rows.Count != 0) {
                    dgvMetarial.Rows[index].Cells[2].Value = dtAdjPP.Rows[index]["调左平面"];
                    dgvMetarial.Rows[index].Cells[3].Value = -double.Parse(dtAdjPP.Rows[index]["调左平面"].ToString());
                    dgvMetarial.Rows[index].Cells[5].Value = dtAdjPP.Rows[index]["调右平面"];
                    dgvMetarial.Rows[index].Cells[6].Value = -double.Parse(dtAdjPP.Rows[index]["调右平面"].ToString());
                }
                else {
                    dgvMetarial.Rows[index].Cells[2].Value = "";
                    dgvMetarial.Rows[index].Cells[3].Value = "";
                    dgvMetarial.Rows[index].Cells[5].Value = "";
                    dgvMetarial.Rows[index].Cells[6].Value = "";
                }
                if (dtAdjEle.Rows.Count != 0) {
                    dgvMetarial.Rows[index].Cells[4].Value = dtAdjEle.Rows[index]["调左高程"];
                    dgvMetarial.Rows[index].Cells[7].Value = dtAdjEle.Rows[index]["调右高程"];
                }
                else {
                    dgvMetarial.Rows[index].Cells[4].Value = "";
                    dgvMetarial.Rows[index].Cells[7].Value = "";
                }                                          
            }
            dgvMetarial.SetDoubleBuffered(true);
        }
        /// <summary>
        /// 显示轨距垫数量
        /// </summary>
        private void ShowPP() {
            for (int p = 0; p < pp.GetLength(1); p++) {
                int index = dgvPPNum.Rows.Add();
                dgvPPNum.Rows[index].Cells[0].Value = pp[0,p];
                dgvPPNum.Rows[index].Cells[2].Value = pp[1,p];
                dgvPPNum.Rows[index].Cells[3].Value = pp[2,p];
                dgvPPNum.Rows[index].Cells[4].Value = pp[3,p];
                dgvPPNum.Rows[index].Cells[1].Value = dgvPPNum.Rows[index].Cells[0].Value + "mm";
            }
            dgvPPNum.SetDoubleBuffered(true);
        }
        /// <summary>
        /// 显示调高垫板数量
        /// </summary>
        private void ShowEle() {
            for (int p = 0; p < ele.GetLength(1); p++) {
                int index = dgvElevationNum.Rows.Add();
                dgvElevationNum.Rows[index].Cells[0].Value = ele[0,p];
                dgvElevationNum.Rows[index].Cells[2].Value = ele[1,p];
                dgvElevationNum.Rows[index].Cells[3].Value = ele[2,p];
                dgvElevationNum.Rows[index].Cells[4].Value = ele[3,p];
                dgvElevationNum.Rows[index].Cells[1].Value = dgvElevationNum.Rows[index].Cells[0].Value + "mm";
            }
            dgvElevationNum.SetDoubleBuffered(true);
        }

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalc_Click(object sender, EventArgs e) {
            if (derta == 0) {
                MessageBox.Show("扣件无极差，无法计算！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //for (int i = 1; i < 3; i++) {
            //    for (int j = 0; j < pp.GetLength(1); j++) {
            //        pp[i, j] = 0;
            //    }
            //}
            //for (int i = 1; i < 3; i++) {
            //    for (int j = 0; j < ele.GetLength(1); j++) {
            //        ele[i, j] = 0;
            //    }
            //}


            if (dgvPPNum.Rows.Count != 0 && dgvElevationNum.Rows.Count != 0) {
                if (MessageBox.Show("是否重新计算？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    dgvPPNum.Rows.Clear();
                    dgvElevationNum.Rows.Clear();
                    dtAfterModel.Rows.Clear();
                    splashScreenManager1.ShowWaitForm();
                    //计算调后厚度
                    CalcAfterMetarial();
                    if (dtAdjPP.Rows.Count!=0) {
                        double[] p = new double[8];
                        p[0] = (double)dtFasten.Compute("Max(左股外口轨距块)", "true");
                        p[1] = (double)dtFasten.Compute("Max(左股里口轨距块)", "true");
                        p[2] = (double)dtFasten.Compute("Max(右股外口轨距块)", "true");
                        p[3] = (double)dtFasten.Compute("Max(右股里口轨距块)", "true");
                        p[4] = (double)dtAfterModel.Compute("Max(左股外口轨距块)", "true");
                        p[5] = (double)dtAfterModel.Compute("Max(左股里口轨距块)", "true");
                        p[6] = (double)dtAfterModel.Compute("Max(右股外口轨距块)", "true");
                        p[7] = (double)dtAfterModel.Compute("Max(右股里口轨距块)", "true");
                        pp = GetArr(derta, p.Max());
                        //计算轨距垫数量
                        GetPPNum(dtFasten, dtAfterModel);
                        ShowPP();
                    }
                    if (dtAdjEle.Rows.Count!=0) {
                        double[] ee = new double[4];
                        ee[0] = (double)dtFasten.Compute("Max(左股胶垫厚度)", "true");
                        ee[1] = (double)dtFasten.Compute("Max(右股胶垫厚度)", "true");
                        ee[2] = (double)dtAfterModel.Compute("Max(左股胶垫厚度)", "true");
                        ee[3] = (double)dtAfterModel.Compute("Max(右股胶垫厚度)", "true");
                        ele = GetArr(derta, ee.Max());
                        //计算调高垫板数量
                        GetEleNum(dtFasten, dtAfterModel); 
                        ShowEle();
                    }
                    splashScreenManager1.CloseWaitForm();
                    MessageBox.Show("计算完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else {
                dgvPPNum.Rows.Clear();
                dgvElevationNum.Rows.Clear();
                dtAfterModel.Rows.Clear();
                splashScreenManager1.ShowWaitForm();
                //计算调后厚度
                CalcAfterMetarial();
                if (dtAdjPP.Rows.Count != 0) {
                    double[] p = new double[8];
                    p[0] = (double)dtFasten.Compute("Max(左股外口轨距块)", "true");
                    p[1] = (double)dtFasten.Compute("Max(左股里口轨距块)", "true");
                    p[2] = (double)dtFasten.Compute("Max(右股外口轨距块)", "true");
                    p[3] = (double)dtFasten.Compute("Max(右股里口轨距块)", "true");
                    p[4] = (double)dtAfterModel.Compute("Max(左股外口轨距块)", "true");
                    p[5] = (double)dtAfterModel.Compute("Max(左股里口轨距块)", "true");
                    p[6] = (double)dtAfterModel.Compute("Max(右股外口轨距块)", "true");
                    p[7] = (double)dtAfterModel.Compute("Max(右股里口轨距块)", "true");
                    pp = GetArr(derta, p.Max());
                    //计算轨距垫数量
                    GetPPNum(dtFasten, dtAfterModel);
                    ShowPP();
                }
                if (dtAdjEle.Rows.Count != 0) {
                    double[] ee = new double[4];
                    ee[0] = (double)dtFasten.Compute("Max(左股胶垫厚度)", "true");
                    ee[1] = (double)dtFasten.Compute("Max(右股胶垫厚度)", "true");
                    ee[2] = (double)dtAfterModel.Compute("Max(左股胶垫厚度)", "true");
                    ee[3] = (double)dtAfterModel.Compute("Max(右股胶垫厚度)", "true");
                    ele = GetArr(derta, ee.Max());
                    //计算调高垫板数量
                    GetEleNum(dtFasten, dtAfterModel);
                    ShowEle();
                }
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show("计算完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e) {
            if (dgvPPNum.Rows.Count == 0 && dgvElevationNum.Rows.Count == 0) {
                MessageBox.Show("没有数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dtAfterModel.Rows.Count != 0) {
                IOHelper.DataTableExportToTxt(dtAfterModel, path + $"调整方案\\调后扣件型号(极差为{para.Rows[4][1]}).txt");
            }
            DirectoryInfo root = new DirectoryInfo(path + "调整方案");
            if (dgvPPNum.Rows.Count != 0) {
                bool tag = false;
                foreach (FileInfo file in root.GetFiles()) {
                    if (file.Name.Contains($"轨距垫数量(极差为{para.Rows[4][1]})")) {
                        tag = true;
                        break;
                    }
                }
                string fileName = path + $"调整方案\\轨距垫数量(极差为{para.Rows[4][1]}).txt";
                if (tag == true) {
                    if (MessageBox.Show("已有轨距垫数量文件，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        // 如果存在要保存的文件,则删除
                        if (File.Exists(fileName)) {
                            File.Delete(fileName);
                        }
                        IOHelper.DataGridViewToTxt(dgvPPNum, fileName, '\t');

                        //if (MessageBox.Show("保存成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        //    System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                        //}
                    }
                    //else {
                    //    if (MessageBox.Show("是否查看轨距垫数量文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    //        System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    //    }
                    //}
                }
                else {
                    // 如果存在要保存的文件,则删除
                    if (File.Exists(fileName)) {
                        File.Delete(fileName);
                    }
                    IOHelper.DataGridViewToTxt(dgvPPNum, fileName, '\t');
                    //if (MessageBox.Show("保存成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    //    System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    //}
                }
            }
            if (dgvElevationNum.Rows.Count != 0) {
                bool tag = false;
                foreach (FileInfo file in root.GetFiles()) {
                    if (file.Name.Contains($"调高垫板数量(极差为{para.Rows[4][1]})")) {
                        tag = true;
                        break;
                    }
                }
                string fileName = path + $"调整方案\\调高垫板数量(极差为{para.Rows[4][1]}).txt";
                if (tag == true) {
                    if (MessageBox.Show("已有调高垫板数量文件，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        // 如果存在要保存的文件,则删除
                        if (File.Exists(fileName)) {
                            File.Delete(fileName);
                        }
                        IOHelper.DataGridViewToTxt(dgvElevationNum, fileName, '\t');
                        //if (MessageBox.Show("保存成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        //    System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                        //}
                    }
                    //else {
                    //    if (MessageBox.Show("是否查看原文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    //        System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    //    }
                    //}
                }
                else {
                    // 如果存在要保存的文件,则删除
                    if (File.Exists(fileName)) {
                        File.Delete(fileName);
                    }
                    IOHelper.DataGridViewToTxt(dgvElevationNum, fileName, '\t');
                    //if (MessageBox.Show("保存成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    //    System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    //}
                }
            }
            if (MessageBox.Show("保存成功,是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                System.Diagnostics.Process.Start("Explorer", path + "调整方案");
            }
        }

        /// <summary>
        /// 获取调整量，存入数组
        /// </summary>
        /// <param name="derta"></param>
        /// <returns></returns>
        public static double[] GetModel(double derta, DataTable dt, string colName1, string colName2) {           
            double max1 = (double)dt.Compute($"Max({colName1})", "true");
            double min1 = (double)dt.Compute($"Min({colName1})", "true");
            double max11 = Math.Abs(max1) >= Math.Abs(min1) ? Math.Abs(max1) : Math.Abs(min1);
            double max2 = (double)dt.Compute($"Max({colName2})", "true");
            double min2 = (double)dt.Compute($"Min({colName2})", "true");
            double max22 = Math.Abs(max2) >= Math.Abs(min2) ? Math.Abs(max2) : Math.Abs(min2);
            double max = max11 >= max22 ? max11 : max22;
            Console.WriteLine(max);
            int count =Convert.ToInt32(max / derta);
            Console.WriteLine(max);//查看数据
            double[,] result = new double[2,count];
            for (int i = 0; i < result.GetLength(1); i++) {
                result[0, i] = derta * (i + 1);
            }
            for (int k = 0; k < result.GetLength(1); k++) {
                bool exitLoop = false;
                for (int i = dt.Columns.IndexOf(colName1); i <= dt.Columns.IndexOf(colName2); i++) {
                    if (exitLoop == true)
                        break;
                    for (int j = 0; j < dt.Rows.Count; j++) {
                        if (result[0, k] == Convert.ToDouble(dt.Rows[j][i])) {
                            result[1, k] = 1;
                            exitLoop = true;
                            break;
                        }
                    }
                }
            }
            int arrLenth = 0;
            for (int i = 0; i < result.GetLength(1); i++) {
                if (result[1,i]==1) {
                    arrLenth++;
                }
            }
            double[] arr = new double[arrLenth];
            int index = 0;
            for (int i = 0; i < result.GetLength(1); i++) {
                if (result[1, i] == 1) {
                    arr[index] = result[0, i];
                    index++;
                }
            }
            return arr;
        }

        /// <summary>
        /// 根据极差和最大型号生成型号数组
        /// </summary>
        /// <param name="derta"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double[,] GetArr(double derta,double max) {
            int length = Convert.ToInt32(max / derta);
            double[,] arr = new double[4, length];
            for (int i = 0; i < length; i++) {
                arr[0,i] = (i + 1) * derta;
            }
            return arr;
        }

        private void dgvMetarial_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvMetarial, e);
        }
    }
}
