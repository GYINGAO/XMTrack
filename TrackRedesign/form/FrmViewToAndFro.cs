using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace TrackRedesign {
    public partial class FrmViewToAndFro : Form {
        private static string format = "0.0";
        private DataTable dtToDate;
        private DataTable dtFroDate;
        public FrmViewToAndFro() {
            InitializeComponent();
        }

        private void FrmViewToAndFro_Load(object sender, EventArgs e) {
            if (this.Tag != null) {
                TagObject tagObject = (TagObject)this.Tag;
                this.dtToDate = tagObject.DtToDate;
                this.dtFroDate = tagObject.DtFroDate;
            }
            this.dgvToDate.DataSource = dtToDate;
            this.dgvFroDate.DataSource = dtFroDate;
            dgvToDate.SetDoubleBuffered(true);
            dgvFroDate.SetDoubleBuffered(true);
            SetFormat(dgvToDate);
            SetFormat(dgvFroDate);
        }

        private void dgvToDate_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            //自动编号，与数据无关
            ChartHelper.AutoNumber(dgvToDate, e);
        }

        private void dgvFroDate_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            //自动编号，与数据无关
            ChartHelper.AutoNumber(dgvFroDate, e);
        }
        private static void SetFormat(DataGridView dgv) {
            dgv.Columns["调前高程"].DefaultCellStyle.Format = format;
            dgv.Columns["调前平面"].DefaultCellStyle.Format = format;
            dgv.Columns["调前水平"].DefaultCellStyle.Format = format;
            dgv.Columns["调前轨距"].DefaultCellStyle.Format = format;
            dgv.Columns["调左轨高"].DefaultCellStyle.Format = format;
            dgv.Columns["调右轨高"].DefaultCellStyle.Format = format;
            dgv.Columns["调左平面"].DefaultCellStyle.Format = format;
            dgv.Columns["调右平面"].DefaultCellStyle.Format = format;
            dgv.Columns["调后高程"].DefaultCellStyle.Format = format;
            dgv.Columns["调后平面"].DefaultCellStyle.Format = format;
            dgv.Columns["调后水平"].DefaultCellStyle.Format = format;
            dgv.Columns["调后轨距"].DefaultCellStyle.Format = format;
            dgv.Columns["里程"].DefaultCellStyle.Format = format;
            dgv.Columns["设计超高"].DefaultCellStyle.Format = format;
        }
    }
}

