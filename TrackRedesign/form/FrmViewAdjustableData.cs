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
    public partial class FrmViewAdjustableData : Form {
        private static string format = "0.0";
        private DataTable dt;
        public FrmViewAdjustableData() {
            InitializeComponent();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            //自动编号，与数据无关
            ChartHelper.AutoNumber(dgvAdjustableData, e);
        }

        private void FrmViewAdjustableData_Load(object sender, EventArgs e) {
            if (this.Tag != null)
                dt = (DataTable)this.Tag;
            DataTable adj = new DataTable("可调量数据");
            adj.Columns.Add("轨枕编号", Type.GetType("System.String"));
            adj.Columns.Add("里程", Type.GetType("System.Double"));
            adj.Columns.Add("左股左调量", Type.GetType("System.Double"));
            adj.Columns.Add("左股右调量", Type.GetType("System.Double"));
            adj.Columns.Add("左股上调量", Type.GetType("System.Double"));
            adj.Columns.Add("左股下调量", Type.GetType("System.Double"));
            adj.Columns.Add("右股左调量", Type.GetType("System.Double"));
            adj.Columns.Add("右股右调量", Type.GetType("System.Double"));
            adj.Columns.Add("右股上调量", Type.GetType("System.Double"));
            adj.Columns.Add("右股下调量", Type.GetType("System.Double"));
            adj.Columns.Add("CP3里程", Type.GetType("System.Double"));
            for (int i = 0; i < dt.Rows.Count; i++) {
                DataRow dr = adj.NewRow();
                dr["里程"] = dt.Rows[i]["里程"];
                dr["轨枕编号"] = dt.Rows[i]["轨枕编号"];
                dr["左股左调量"] = dt.Rows[i]["左股左调量"];
                dr["左股右调量"] = dt.Rows[i]["左股右调量"];
                dr["左股上调量"] = dt.Rows[i]["左股上调量"];
                dr["左股下调量"] = dt.Rows[i]["左股下调量"];
                dr["右股左调量"] = dt.Rows[i]["右股左调量"];
                dr["右股右调量"] = dt.Rows[i]["右股右调量"];
                dr["右股上调量"] = dt.Rows[i]["右股上调量"];
                dr["右股下调量"] = dt.Rows[i]["右股下调量"];
                dr["CP3里程"] = dt.Rows[i]["CP3里程"];
                adj.Rows.Add(dr);
            }
            dgvAdjustableData.DataSource = adj;
            dgvAdjustableData.SetDoubleBuffered(true);
            SetFormat(dgvAdjustableData);
        }

        private static void SetFormat(DataGridView dgv) {
            dgv.Columns["里程"].DefaultCellStyle.Format = format;
            dgv.Columns["左股左调量"].DefaultCellStyle.Format = format;
            dgv.Columns["左股右调量"].DefaultCellStyle.Format = format;
            dgv.Columns["左股上调量"].DefaultCellStyle.Format = format;
            dgv.Columns["左股下调量"].DefaultCellStyle.Format = format;
            dgv.Columns["右股左调量"].DefaultCellStyle.Format = format;
            dgv.Columns["右股右调量"].DefaultCellStyle.Format = format;
            dgv.Columns["右股上调量"].DefaultCellStyle.Format = format;
            dgv.Columns["右股下调量"].DefaultCellStyle.Format = format;
            dgv.Columns["CP3里程"].DefaultCellStyle.Format = format;
        }
    }
}
