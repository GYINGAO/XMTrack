namespace TrackRedesign {
    partial class FrmShowBAChart {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tcContrast = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ctclGaugeContrast = new DevExpress.XtraCharts.ChartControl();
            this.ctclPPContrast = new DevExpress.XtraCharts.ChartControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctclLevelContrast = new DevExpress.XtraCharts.ChartControl();
            this.ctclElevationContrast = new DevExpress.XtraCharts.ChartControl();
            this.cbxX = new System.Windows.Forms.CheckBox();
            this.cbxY = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.tcContrast.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctclGaugeContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclPPContrast)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctclLevelContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclElevationContrast)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(841, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::TrackRedesign.Properties.Resources.save;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Tag = "";
            this.toolStripButton1.Text = "保存图片";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 24);
            this.toolStripLabel1.Text = "比例尺缩放";
            // 
            // tcContrast
            // 
            this.tcContrast.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcContrast.Controls.Add(this.tabPage1);
            this.tcContrast.Controls.Add(this.tabPage2);
            this.tcContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcContrast.Location = new System.Drawing.Point(0, 27);
            this.tcContrast.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tcContrast.Name = "tcContrast";
            this.tcContrast.SelectedIndex = 0;
            this.tcContrast.Size = new System.Drawing.Size(841, 435);
            this.tcContrast.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(833, 409);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "平面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ctclGaugeContrast, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ctclPPContrast, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(829, 405);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // ctclGaugeContrast
            // 
            this.ctclGaugeContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclGaugeContrast.Legend.Name = "Default Legend";
            this.ctclGaugeContrast.Location = new System.Drawing.Point(2, 204);
            this.ctclGaugeContrast.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclGaugeContrast.Name = "ctclGaugeContrast";
            this.ctclGaugeContrast.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclGaugeContrast.Size = new System.Drawing.Size(825, 199);
            this.ctclGaugeContrast.TabIndex = 1;
            // 
            // ctclPPContrast
            // 
            this.ctclPPContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclPPContrast.Legend.Name = "Default Legend";
            this.ctclPPContrast.Location = new System.Drawing.Point(2, 2);
            this.ctclPPContrast.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclPPContrast.Name = "ctclPPContrast";
            this.ctclPPContrast.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclPPContrast.Size = new System.Drawing.Size(825, 198);
            this.ctclPPContrast.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Size = new System.Drawing.Size(833, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "高程";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ctclLevelContrast, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ctclElevationContrast, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(829, 411);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ctclLevelContrast
            // 
            this.ctclLevelContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclLevelContrast.Legend.Name = "Default Legend";
            this.ctclLevelContrast.Location = new System.Drawing.Point(2, 207);
            this.ctclLevelContrast.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclLevelContrast.Name = "ctclLevelContrast";
            this.ctclLevelContrast.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclLevelContrast.Size = new System.Drawing.Size(825, 202);
            this.ctclLevelContrast.TabIndex = 2;
            // 
            // ctclElevationContrast
            // 
            this.ctclElevationContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclElevationContrast.Legend.Name = "Default Legend";
            this.ctclElevationContrast.Location = new System.Drawing.Point(2, 2);
            this.ctclElevationContrast.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclElevationContrast.Name = "ctclElevationContrast";
            this.ctclElevationContrast.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclElevationContrast.Size = new System.Drawing.Size(825, 201);
            this.ctclElevationContrast.TabIndex = 1;
            // 
            // cbxX
            // 
            this.cbxX.AutoSize = true;
            this.cbxX.Location = new System.Drawing.Point(106, 6);
            this.cbxX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbxX.Name = "cbxX";
            this.cbxX.Size = new System.Drawing.Size(48, 16);
            this.cbxX.TabIndex = 2;
            this.cbxX.Text = "横轴";
            this.cbxX.UseVisualStyleBackColor = true;
            this.cbxX.CheckedChanged += new System.EventHandler(this.cbxX_CheckedChanged);
            // 
            // cbxY
            // 
            this.cbxY.AutoSize = true;
            this.cbxY.Location = new System.Drawing.Point(154, 6);
            this.cbxY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbxY.Name = "cbxY";
            this.cbxY.Size = new System.Drawing.Size(48, 16);
            this.cbxY.TabIndex = 3;
            this.cbxY.Text = "纵轴";
            this.cbxY.UseVisualStyleBackColor = true;
            this.cbxY.CheckedChanged += new System.EventHandler(this.cbxY_CheckedChanged);
            // 
            // FrmShowBAChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 462);
            this.Controls.Add(this.cbxY);
            this.Controls.Add(this.cbxX);
            this.Controls.Add(this.tcContrast);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmShowBAChart";
            this.Text = "调整前后对比图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmShowBAChart_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tcContrast.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctclGaugeContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclPPContrast)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctclLevelContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclElevationContrast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tcContrast;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraCharts.ChartControl ctclGaugeContrast;
        private DevExpress.XtraCharts.ChartControl ctclPPContrast;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl ctclLevelContrast;
        private DevExpress.XtraCharts.ChartControl ctclElevationContrast;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox cbxX;
        private System.Windows.Forms.CheckBox cbxY;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}