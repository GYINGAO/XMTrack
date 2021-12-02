using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackRedesign {
    public partial class FrmToFroImage : Form {
        private DataTable dtTo;
        private DataTable dtFro;
        private string path;
        public FrmToFroImage() {
            InitializeComponent();
        }

        private void FrmToFroImage_Load(object sender, EventArgs e) {
            if (this.Tag != null) {
                TagObject tagObject = (TagObject)this.Tag;
                this.dtTo = tagObject.DtToDate;
                this.dtFro = tagObject.DtFroDate;
                this.path = tagObject.Path;
            }
            try {
                ChartHelper.AddOneSeries(dtTo, ctclPP, "往测平面位置", "调前平面");
                ChartHelper.AddOneSeries(dtFro, ctclPP, "返测平面位置", "调前平面");
                ChartHelper.SetLegendAndXY(ctclPP);

                ChartHelper.AddOneSeries(dtTo, ctclElevation, "往测高程", "调前高程");
                ChartHelper.AddOneSeries(dtFro, ctclElevation, "返测高程", "调前高程");
                ChartHelper.SetLegendAndXY(ctclElevation);

                ChartHelper.AddOneSeries(dtTo, ctclGauge, "往测轨距", "调前轨距");
                ChartHelper.AddOneSeries(dtFro, ctclGauge, "返测轨距", "调前轨距");
                ChartHelper.SetLegendAndXY(ctclGauge);

                ChartHelper.AddOneSeries(dtTo, ctclLevel, "往测水平", "调前水平");
                ChartHelper.AddOneSeries(dtFro, ctclLevel, "返测水平", "调前水平");
                ChartHelper.SetLegendAndXY(ctclLevel);
            }
            catch(Exception ex) {
                MessageBox.Show("发生异常：" + ex.Message+"\n请重试！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbxX_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xyPP = ctclPP.Diagram as XYDiagram;
            XYDiagram xyGauge = ctclGauge.Diagram as XYDiagram;
            XYDiagram xyElevation = ctclElevation.Diagram as XYDiagram;
            XYDiagram xyLevel = ctclLevel.Diagram as XYDiagram;
            if (cbxX.Checked) {
                xyPP.EnableAxisXZooming = true;
                xyGauge.EnableAxisXZooming = true;
                xyElevation.EnableAxisXZooming = true;
                xyLevel.EnableAxisXZooming = true;
            }
            else {
                xyPP.EnableAxisXZooming = false;
                xyGauge.EnableAxisXZooming = false;
                xyElevation.EnableAxisXZooming = false;
                xyLevel.EnableAxisXZooming = false;
            }
        }

        private void cbxY_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xyPP = ctclPP.Diagram as XYDiagram;
            XYDiagram xyGauge = ctclGauge.Diagram as XYDiagram;
            XYDiagram xyElevation = ctclElevation.Diagram as XYDiagram;
            XYDiagram xyLevel = ctclLevel.Diagram as XYDiagram;
            if (cbxY.Checked) {
                xyPP.EnableAxisYZooming = true;
                xyGauge.EnableAxisYZooming = true;
                xyElevation.EnableAxisYZooming = true;
                xyLevel.EnableAxisYZooming = true;
            }
            else {
                xyPP.EnableAxisYZooming = false;
                xyGauge.EnableAxisYZooming = false;
                xyElevation.EnableAxisYZooming = false;
                xyLevel.EnableAxisYZooming = false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            ctclPP.ExportToImage(path + @"预处理文件\调前平面位置.png", ImageFormat.Png);
            ctclGauge.ExportToImage(path + @"预处理文件\调前轨距.png", ImageFormat.Png);
            ctclElevation.ExportToImage(path + @"预处理文件\调前高程.png", ImageFormat.Png);
            ctclLevel.ExportToImage(path + @"预处理文件\调前水平.png", ImageFormat.Png);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
