using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackRedesign {
    public partial class FrmEditPPPoint : Form {
        readonly int index;
        FrmAddAdjustLine fm = null;
        private double tag;
        public FrmEditPPPoint() {
            InitializeComponent();
        }
        public FrmEditPPPoint(int passIndex,FrmAddAdjustLine fmPass) {
            InitializeComponent();
            index = passIndex;
            fm = fmPass;
        }
        private void FrmEditPoint_Load(object sender, EventArgs e) {
            txtMiles.Text = fm.dgvPPAdjustLine.Rows[index].Cells[0].Value.ToString();
            txtBias.Text = fm.dgvPPAdjustLine.Rows[index].Cells[1].Value.ToString();
            tag = Math.Round(double.Parse(txtMiles.Text), 1);
        }

        private void btnConfirm_Click(object sender, EventArgs e) {
            this.Close();
        }


        private void FrmEditPPPoint_FormClosing(object sender, FormClosingEventArgs e) {
            if (!IOHelper.IsIntOrDouble(txtMiles.Text.Trim()) || !IOHelper.IsIntOrDouble(txtBias.Text.Trim())) {
                MessageBox.Show("输入有误，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            for (int i = 0; i < fm.dgvPPAdjustLine.Rows.Count; i++) {
                if (Math.Round(double.Parse(fm.dgvPPAdjustLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(txtMiles.Text.Trim()), 1) && Math.Round(double.Parse(txtMiles.Text.Trim()), 1) != tag) {
                    MessageBox.Show("里程重复，无法修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
            }
            fm.dgvPPAdjustLine.Rows[index].Cells["PPMiles"].Value = double.Parse(txtMiles.Text.Trim());
            fm.dgvPPAdjustLine.Rows[index].Cells["PPBias"].Value = double.Parse(txtBias.Text.Trim());
            fm.dgvPPAdjustLine.Sort(fm.dgvPPAdjustLine.Columns[0], ListSortDirection.Ascending);
            FrmAddAdjustLine.GetBend(fm.dgvPPAdjustLine, fm.chordLength);

            ChartHelper.LogChecked(fm.ctclPPAdjLine, fm.PPChecked);
            fm.PaintFromDgv(fm.dgvPPAdjustLine, fm.ctclPPAdjLine, "PPMiles", "PPBias");
            //计算调整量
            fm.CalcPP();
            //画调整量图像
            fm.PaintFromCalcPPResult();
            ChartHelper.SetSeriesColor(fm.ctclPPAdjLine, fm.ppColor, "调前平面", "调整量", "右界", "左界", "调整中线");
            ChartHelper.SetPPCheckBoxes(fm.ctclPPAdjLine, fm.PPChecked);
        }
    }
}
