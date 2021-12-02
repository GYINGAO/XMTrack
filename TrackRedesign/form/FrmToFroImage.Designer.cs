namespace TrackRedesign {
    partial class FrmToFroImage {
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tabclToFroImage = new System.Windows.Forms.TabControl();
            this.tabpPP = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctclGauge = new DevExpress.XtraCharts.ChartControl();
            this.ctclPP = new DevExpress.XtraCharts.ChartControl();
            this.tabpGauge = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ctclElevation = new DevExpress.XtraCharts.ChartControl();
            this.ctclLevel = new DevExpress.XtraCharts.ChartControl();
            this.cbxX = new System.Windows.Forms.CheckBox();
            this.cbxY = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.tabclToFroImage.SuspendLayout();
            this.tabpPP.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctclGauge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclPP)).BeginInit();
            this.tabpGauge.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctclElevation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(965, 27);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 24);
            this.toolStripLabel1.Text = "比例尺缩放";
            // 
            // tabclToFroImage
            // 
            this.tabclToFroImage.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabclToFroImage.Controls.Add(this.tabpPP);
            this.tabclToFroImage.Controls.Add(this.tabpGauge);
            this.tabclToFroImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabclToFroImage.Location = new System.Drawing.Point(0, 27);
            this.tabclToFroImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabclToFroImage.Name = "tabclToFroImage";
            this.tabclToFroImage.SelectedIndex = 0;
            this.tabclToFroImage.Size = new System.Drawing.Size(965, 503);
            this.tabclToFroImage.TabIndex = 2;
            // 
            // tabpPP
            // 
            this.tabpPP.Controls.Add(this.tableLayoutPanel1);
            this.tabpPP.Location = new System.Drawing.Point(4, 4);
            this.tabpPP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpPP.Name = "tabpPP";
            this.tabpPP.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpPP.Size = new System.Drawing.Size(957, 477);
            this.tabpPP.TabIndex = 0;
            this.tabpPP.Text = "平面";
            this.tabpPP.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ctclGauge, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ctclPP, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(953, 473);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ctclGauge
            // 
            this.ctclGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclGauge.Legend.Name = "Default Legend";
            this.ctclGauge.Location = new System.Drawing.Point(2, 238);
            this.ctclGauge.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclGauge.Name = "ctclGauge";
            this.ctclGauge.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclGauge.Size = new System.Drawing.Size(949, 233);
            this.ctclGauge.TabIndex = 3;
            // 
            // ctclPP
            // 
            this.ctclPP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclPP.Legend.Name = "Default Legend";
            this.ctclPP.Location = new System.Drawing.Point(2, 2);
            this.ctclPP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclPP.Name = "ctclPP";
            this.ctclPP.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclPP.Size = new System.Drawing.Size(949, 232);
            this.ctclPP.TabIndex = 0;
            // 
            // tabpGauge
            // 
            this.tabpGauge.BackColor = System.Drawing.Color.Transparent;
            this.tabpGauge.Controls.Add(this.tableLayoutPanel2);
            this.tabpGauge.Location = new System.Drawing.Point(4, 4);
            this.tabpGauge.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpGauge.Name = "tabpGauge";
            this.tabpGauge.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpGauge.Size = new System.Drawing.Size(957, 483);
            this.tabpGauge.TabIndex = 1;
            this.tabpGauge.Text = "高程";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.ctclElevation, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ctclLevel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(953, 479);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // ctclElevation
            // 
            this.ctclElevation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclElevation.Legend.Name = "Default Legend";
            this.ctclElevation.Location = new System.Drawing.Point(2, 2);
            this.ctclElevation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclElevation.Name = "ctclElevation";
            this.ctclElevation.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclElevation.Size = new System.Drawing.Size(949, 235);
            this.ctclElevation.TabIndex = 7;
            // 
            // ctclLevel
            // 
            this.ctclLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctclLevel.Legend.Name = "Default Legend";
            this.ctclLevel.Location = new System.Drawing.Point(2, 241);
            this.ctclLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ctclLevel.Name = "ctclLevel";
            this.ctclLevel.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.ctclLevel.Size = new System.Drawing.Size(949, 236);
            this.ctclLevel.TabIndex = 2;
            // 
            // cbxX
            // 
            this.cbxX.AutoSize = true;
            this.cbxX.Location = new System.Drawing.Point(156, 6);
            this.cbxX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbxX.Name = "cbxX";
            this.cbxX.Size = new System.Drawing.Size(48, 16);
            this.cbxX.TabIndex = 3;
            this.cbxX.Text = "横轴";
            this.cbxX.UseVisualStyleBackColor = true;
            this.cbxX.CheckedChanged += new System.EventHandler(this.cbxX_CheckedChanged);
            // 
            // cbxY
            // 
            this.cbxY.AutoSize = true;
            this.cbxY.Location = new System.Drawing.Point(108, 6);
            this.cbxY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbxY.Name = "cbxY";
            this.cbxY.Size = new System.Drawing.Size(48, 16);
            this.cbxY.TabIndex = 4;
            this.cbxY.Text = "纵轴";
            this.cbxY.UseVisualStyleBackColor = true;
            this.cbxY.CheckedChanged += new System.EventHandler(this.cbxY_CheckedChanged);
            // 
            // FrmToFroImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 530);
            this.Controls.Add(this.cbxY);
            this.Controls.Add(this.cbxX);
            this.Controls.Add(this.tabclToFroImage);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmToFroImage";
            this.Text = "显示往返测图像";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmToFroImage_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabclToFroImage.ResumeLayout(false);
            this.tabpPP.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctclGauge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclPP)).EndInit();
            this.tabpGauge.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctclElevation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctclLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.TabControl tabclToFroImage;
        private System.Windows.Forms.TabPage tabpPP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl ctclGauge;
        private DevExpress.XtraCharts.ChartControl ctclPP;
        private System.Windows.Forms.TabPage tabpGauge;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraCharts.ChartControl ctclElevation;
        private DevExpress.XtraCharts.ChartControl ctclLevel;
        private System.Windows.Forms.CheckBox cbxX;
        private System.Windows.Forms.CheckBox cbxY;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}