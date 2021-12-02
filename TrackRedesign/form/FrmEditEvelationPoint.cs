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
    public partial class FrmEditEvelationPoint : Form {
        private double tag;
        readonly int index;
        FrmAddAdjustLine fm = null;
        public FrmEditEvelationPoint() {
            InitializeComponent();
        }
        public FrmEditEvelationPoint(int passIndex,FrmAddAdjustLine fmPass) {
            InitializeComponent();
            index = passIndex;
            fm = fmPass;
        }
        private void FrmEditPoint_Load(object sender, EventArgs e) {
            txtMiles.Text = fm.dgvElevationLine.Rows[index].Cells[0].Value.ToString();
            txtBias.Text = fm.dgvElevationLine.Rows[index].Cells[1].Value.ToString();
            tag = Math.Round(double.Parse(txtMiles.Text), 1);
        }


        private void btnConfirm_Click(object sender, EventArgs e) {
            this.Close();
        }


        private void FrmEditEvelationPoint_FormClosing(object sender, FormClosingEventArgs e) {
            if (!IOHelper.IsIntOrDouble(txtMiles.Text.Trim()) || !IOHelper.IsIntOrDouble(txtBias.Text.Trim())) {
                MessageBox.Show("输入有误，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            for (int i = 0; i < fm.dgvElevationLine.Rows.Count; i++) {
                if (Math.Round(double.Parse(fm.dgvElevationLine.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(txtMiles.Text.Trim()), 1) && Math.Round(double.Parse(txtMiles.Text.Trim()), 1) != tag) {
                    MessageBox.Show("里程重复，无法修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
            }
            fm.dgvElevationLine.Rows[index].Cells["ElevationMiles"].Value = double.Parse(txtMiles.Text.Trim());
            fm.dgvElevationLine.Rows[index].Cells["ElevationBias"].Value = double.Parse(txtBias.Text.Trim());

            fm.dgvElevationLine.Sort(fm.dgvElevationLine.Columns[0], ListSortDirection.Ascending);
            FrmAddAdjustLine.GetBend(fm.dgvElevationLine, fm.chordLength);
            ChartHelper.LogEleChecked(fm.ctclElevation, fm.EleChecked);
            fm.PaintFromDgv(fm.dgvElevationLine, fm.ctclElevation, "ElevationMiles", "ElevationBias");
            //计算调整量
            fm.CalcElevation();
            //画调整量图像
            fm.PaintFromCalcElevationResult();
            ChartHelper.SetSeriesColor(fm.ctclElevation,fm.eleColor, "调前高程", "调整量", "下界", "上界", "调整中线");
            ChartHelper.SetEleCheckBoxes(fm.ctclElevation, fm.EleChecked);
        }
    }
}
