using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WinForms;

namespace TrackRedesign {
    public delegate void TransfDelegate(string title, DateTime time, string maker);
    public partial class FrmInfo : Form {

        public string path, title, maker;
        public DataTable parameter, export, overrunPP, overrunEle;
        public int datumTrack;
        public DateTime time;
        public event TransfDelegate TransfEvent;
        public FrmInfo() {
            InitializeComponent();
        }
        public FrmInfo(string path, DataTable parameter, DataTable export, int datumTrack, string title, DateTime time, string maker, DataTable overrunPP, DataTable overrunEle) {
            InitializeComponent();
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "yyyy年MM月dd日";
            this.path = path;
            this.parameter = parameter;
            this.export = export;
            this.datumTrack = datumTrack;
            this.title = title;
            this.time = time;
            this.maker = maker;
            this.overrunPP = overrunPP;
            this.overrunEle = overrunEle;
        }
        private void FrmInfo_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(title)) {
                txtTitle.Text = title;
            }
            if (time.Year != 0001) {
                dtpTime.Value = time;
            }
            if (!string.IsNullOrEmpty(maker)) {
                txtMaker.Text = maker;
            }
            txtTitle.Focus();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtTitle.Text)) {
                MessageBox.Show("请输入标题", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtMaker.Text)) {
                MessageBox.Show("请输入方案制定人", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            title = txtTitle.Text.Trim();
            time = dtpTime.Value;
            maker = txtMaker.Text.Trim();
            TransfEvent(title, time, maker);
            //fm.title = txtTitle.Text.Trim();
            //fm.Time = dtpTime.Value;
            //fm.maker = txtMaker.Text.Trim();
            string timeFormat = $"{time.Year}年{time.Month}月{time.Day}日";

            bool tag = true;
            DirectoryInfo root = new DirectoryInfo(path + "调整方案");
            foreach (FileInfo name in root.GetFiles()) {
                //if (Regex.IsMatch(name.FullName, $"调整方案(级差为{parameter.Rows[4][1]})")) {
                //    tag = false;
                //    break;
                //}
                if (name.Name.Contains($"{title}(级差为{parameter.Rows[4][1]})")) {
                    tag = false;
                    break;
                }
            }
            string dirExcel = path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]}).xls";
            string dirTxt = path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]}).txt";
            string template = string.Empty;
            if (datumTrack == 1)
                template = ExceNPOIlHelep.bootTemplate + "ExcelTemplate/右股基准股.xls";
            else if (datumTrack == 0)
                template = ExceNPOIlHelep.bootTemplate + "ExcelTemplate/左股基准股.xls";
            if (tag == false) {
                if (MessageBox.Show($"已有{title}(级差为{parameter.Rows[4][1]})，是否重新导出覆盖？", "导出提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) {
                    //IOHelper.DataTableExportToExcel(export, dir);
                    splashScreenManager1.ShowWaitForm();
                    bool booExcel = ExceNPOIlHelep.ReportExcelForTemplate(export, dirExcel, template, title, timeFormat, maker);
                    //bool booTxt = IOHelper.ExportAdj(export, dirTxt);
                    IOHelper.DataTableExportToTxt(overrunPP, path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]})_平面超限报表.txt");
                    IOHelper.DataTableExportToTxt(overrunEle, path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]})_高程超限报表.txt");
                    splashScreenManager1.CloseWaitForm();
                    if (booExcel) {
                        if (MessageBox.Show("导出成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                            System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                        }
                    }
                    else {
                        MessageBox.Show("导出失败", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else {
                    if (MessageBox.Show("是否查看调整方案？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    }
                }
            }
            else {
                //IOHelper.ExportAdj(export, path + $"调整方案\\调整方案(级差为{parameter.Rows[4][1]}).txt");
                //IOHelper.DataTableExportToExcel(export, path + $"调整方案\\调整方案(级差为{parameter.Rows[4][1]}).xls");
                splashScreenManager1.ShowWaitForm();
                bool booExcel = ExceNPOIlHelep.ReportExcelForTemplate(export, dirExcel, template, title, timeFormat, maker);
                //bool booTxt = IOHelper.ExportAdj(export, dirTxt);
                IOHelper.DataTableExportToTxt(overrunPP, path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]})_平面超限报表.txt");
                IOHelper.DataTableExportToTxt(overrunEle, path + $"调整方案\\{title}(级差为{parameter.Rows[4][1]})_高程超限报表.txt");
                splashScreenManager1.CloseWaitForm();
                if (booExcel) {
                    if (MessageBox.Show("导出成功，是否查看？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        System.Diagnostics.Process.Start("Explorer", path + "调整方案");
                    }
                }
                else {
                    MessageBox.Show("导出失败", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }


    }
}
