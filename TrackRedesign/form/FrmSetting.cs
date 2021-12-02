using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackRedesign {
    public partial class FrmSetting : Form {
        private FrmMain receiveFm;
        public FrmSetting() {
            InitializeComponent();
        }
        /// <summary>
        /// 带一个参数的构造函数
        /// </summary>
        /// <param name="passFm">传递的父窗体</param>
        public FrmSetting(FrmMain passFm) {
            InitializeComponent();
            receiveFm = passFm;
        }
        /// <summary>
        /// 将参数添加到对应的datatable中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(txtElevation.Text)) {
                    receiveFm.parameter.Rows[0]["参数值"] = double.Parse(txtElevation.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtPP.Text)) {
                    receiveFm.parameter.Rows[1]["参数值"] = double.Parse(txtPP.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtGauge.Text)) {
                    receiveFm.parameter.Rows[2]["参数值"] = double.Parse(txtGauge.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtLevel.Text)) {
                    receiveFm.parameter.Rows[3]["参数值"] = double.Parse(txtLevel.Text.Trim());
                }
                if (!string.IsNullOrEmpty(cbojicha.Text)) {
                    if (cbojicha.Text == "无极差")
                        receiveFm.parameter.Rows[4]["参数值"] = 0;
                    else
                        receiveFm.parameter.Rows[4]["参数值"] = double.Parse(cbojicha.Text);
                }
                using (StreamWriter sw = new StreamWriter(Path.Combine(receiveFm.path, receiveFm.prjName), true, Encoding.Default)) {
                    for (int i = 0; i < receiveFm.parameter.Rows.Count; i++) {
                        sw.Write(receiveFm.parameter.Rows[i]["参数名"] + "=" + receiveFm.parameter.Rows[i]["参数值"] + "\r");
                    }
                    //sw.Write(receiveFm.parameter.Rows[1]["参数名"] + "=" + receiveFm.parameter.Rows[1]["参数值"] + "\r");
                    //sw.Write(receiveFm.parameter.Rows[2]["参数名"] + "=" + receiveFm.parameter.Rows[2]["参数值"] + "\r");
                    //sw.Write(receiveFm.parameter.Rows[3]["参数名"] + "=" + receiveFm.parameter.Rows[3]["参数值"] + "\r");
                    //sw.Write(receiveFm.parameter.Rows[4]["参数名"] + "=" + receiveFm.parameter.Rows[4]["参数值"] + "\r");
                }
                IOHelper.DataTableExportToTxt(receiveFm.parameter, receiveFm.path + @"预处理文件\参数文件.txt");
                MessageBox.Show("设置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception) {
                MessageBox.Show("输入有误，请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void FrmSetting_Load(object sender, EventArgs e) {
            txtElevation.Text = receiveFm.parameter.Rows[0]["参数值"].ToString();
            txtPP.Text = receiveFm.parameter.Rows[1]["参数值"].ToString();
            txtGauge.Text = receiveFm.parameter.Rows[2]["参数值"].ToString();
            txtLevel.Text = receiveFm.parameter.Rows[3]["参数值"].ToString();
            if (double.Parse(receiveFm.parameter.Rows[4]["参数值"].ToString()) == 0)
                cbojicha.SelectedIndex = 0;
            else if (double.Parse(receiveFm.parameter.Rows[4]["参数值"].ToString()) == 1)
                cbojicha.SelectedIndex = 1;
        }
    }
}
