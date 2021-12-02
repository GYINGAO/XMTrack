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
    public partial class FrmShowBAChart : Form {
        private DataTable ppAdjustPlan, elevationAdjustPlan, para;
        private string path;
        public FrmShowBAChart() {
            InitializeComponent();
        }
        private void FrmShowBAChart_Load(object sender, EventArgs e) {
            if (this.Tag!=null) {
                TagObject tag = (TagObject)this.Tag;
                ppAdjustPlan = tag.PPAdjustPlan;
                elevationAdjustPlan = tag.ElevationAdjustPlan;
                path = tag.Path;
                para = tag.Para;
            }
            //平面位置对比图
            ChartHelper.AddOneSeries(ppAdjustPlan, ctclPPContrast, "调前平面", "调前平面");
            ChartHelper.AddOneSeries(ppAdjustPlan, ctclPPContrast, "调后平面", "调后平面");
            ChartHelper.SetLegendAndXY(ctclPPContrast);
            //轨距对比图
            ChartHelper.AddOneSeries(ppAdjustPlan, ctclGaugeContrast, "调前轨距", "调前轨距");
            ChartHelper.AddOneSeries(ppAdjustPlan, ctclGaugeContrast, "调后轨距", "调后轨距");
            ChartHelper.SetLegendAndXY(ctclGaugeContrast);
            //高程对比图
            ChartHelper.AddOneSeries(elevationAdjustPlan, ctclElevationContrast, "调前高程", "调前高程");
            ChartHelper.AddOneSeries(elevationAdjustPlan, ctclElevationContrast, "调后高程", "调后高程");
            ChartHelper.SetLegendAndXY(ctclElevationContrast);
            //水平对比图                                                                      
            ChartHelper.AddOneSeries(elevationAdjustPlan, ctclLevelContrast, "调前水平", "调前水平");
            ChartHelper.AddOneSeries(elevationAdjustPlan, ctclLevelContrast, "调后水平", "调后水平");
            ChartHelper.SetLegendAndXY(ctclLevelContrast);
        }

        private void cbxX_CheckedChanged(object sender, EventArgs e) {
            XYDiagram xyPP = ctclPPContrast.Diagram as XYDiagram;
            XYDiagram xyGauge = ctclGaugeContrast.Diagram as XYDiagram;
            XYDiagram xyElevation = ctclElevationContrast.Diagram as XYDiagram;
            XYDiagram xyLevel = ctclLevelContrast.Diagram as XYDiagram;
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
            XYDiagram xyPP = ctclPPContrast.Diagram as XYDiagram;
            XYDiagram xyGauge = ctclGaugeContrast.Diagram as XYDiagram;
            XYDiagram xyElevation = ctclElevationContrast.Diagram as XYDiagram;
            XYDiagram xyLevel = ctclLevelContrast.Diagram as XYDiagram;
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
            ctclPPContrast.ExportToImage(path + @$"调整方案\平面位置对比图(极差为{para.Rows[4][1]}).png", ImageFormat.Png);
            ctclElevationContrast.ExportToImage(path + @$"调整方案\高程对比图(极差为{para.Rows[4][1]}).png", ImageFormat.Png);
            ctclGaugeContrast.ExportToImage(path + @$"调整方案\轨距对比图(极差为{para.Rows[4][1]}).png", ImageFormat.Png);
            ctclLevelContrast.ExportToImage(path + @$"调整方案\水平对比图(极差为{para.Rows[4][1]}).png", ImageFormat.Png);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
