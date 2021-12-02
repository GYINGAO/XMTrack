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
    public partial class FrmViewModelData : Form {
        private static string format = "0.0";
        private DataTable model;
        public FrmViewModelData() {
            InitializeComponent();
        }

        private void FrmViewModelData_Load(object sender, EventArgs e) {
            if (Tag != null)
                model = (DataTable)this.Tag;
            DataTable mod = new DataTable("型号数据");
            mod.Columns.Add("轨枕编号", Type.GetType("System.String"));
            mod.Columns.Add("里程", Type.GetType("System.Double"));
            mod.Columns.Add("左股外口轨距块", Type.GetType("System.Double"));
            mod.Columns.Add("左股里口轨距块", Type.GetType("System.Double"));
            mod.Columns.Add("左股胶垫厚度", Type.GetType("System.Double"));
            mod.Columns.Add("右股里口轨距块", Type.GetType("System.Double"));
            mod.Columns.Add("右股外口轨距块", Type.GetType("System.Double"));
            mod.Columns.Add("右股胶垫厚度", Type.GetType("System.Double"));
            mod.Columns.Add("CP3里程", Type.GetType("System.Double"));

            for (int i = 0; i < model.Rows.Count; i++) {
                DataRow dr = mod.NewRow();
                dr["轨枕编号"] = model.Rows[i]["轨枕编号"];
                dr["里程"] = model.Rows[i]["里程"];
                dr["左股外口轨距块"] = model.Rows[i]["左股外口轨距块"];
                dr["左股里口轨距块"] = model.Rows[i]["左股里口轨距块"];
                dr["左股胶垫厚度"] = model.Rows[i]["左股胶垫厚度"];
                dr["右股里口轨距块"] = model.Rows[i]["右股里口轨距块"];
                dr["右股外口轨距块"] = model.Rows[i]["右股外口轨距块"];
                dr["右股胶垫厚度"] = model.Rows[i]["右股胶垫厚度"];
                dr["CP3里程"] = model.Rows[i]["CP3里程"];
                mod.Rows.Add(dr);
            }
            dgvModelData.DataSource = mod;
            dgvModelData.SetDoubleBuffered(true);
            SetFormat(dgvModelData);
        }

        private void dgvModelData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            ChartHelper.AutoNumber(dgvModelData, e);
        }

        private static void SetFormat(DataGridView dgv) {
            dgv.Columns["里程"].DefaultCellStyle.Format = format;
            dgv.Columns["左股外口轨距块"].DefaultCellStyle.Format = format;
            dgv.Columns["左股里口轨距块"].DefaultCellStyle.Format = format;
            dgv.Columns["左股胶垫厚度"].DefaultCellStyle.Format = format;
            dgv.Columns["右股里口轨距块"].DefaultCellStyle.Format = format;
            dgv.Columns["右股外口轨距块"].DefaultCellStyle.Format = format;
            dgv.Columns["右股胶垫厚度"].DefaultCellStyle.Format = format;
            dgv.Columns["CP3里程"].DefaultCellStyle.Format = format;
        }
    }
}
