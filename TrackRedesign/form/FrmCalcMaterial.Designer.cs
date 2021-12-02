namespace TrackRedesign
{
    partial class FrmCalcMaterial
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCalcMaterial));
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::TrackRedesign.WaitForm1), true, true, true);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCalc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvMetarial = new System.Windows.Forms.DataGridView();
            this.里程 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.轨枕编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.左股外口 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.左股里口 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.左股调高垫板 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.右股里口 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.右股外口 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.右股调高垫板 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPPNum = new System.Windows.Forms.DataGridView();
            this.轨距垫型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.轨距垫置换 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.轨距垫用量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.作差 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvElevationNum = new System.Windows.Forms.DataGridView();
            this.调高垫板型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.调高垫板置换 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.用量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.做差 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetarial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPPNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvElevationNum)).BeginInit();
            this.SuspendLayout();
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCalc,
            this.toolStripSeparator1,
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1196, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCalc
            // 
            this.btnCalc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCalc.Image = ((System.Drawing.Image)(resources.GetObject("btnCalc.Image")));
            this.btnCalc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(24, 24);
            this.btnCalc.Text = "计算";
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(24, 24);
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.46709F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.53292F));
            this.tableLayoutPanel1.Controls.Add(this.dgvMetarial, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvPPNum, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvElevationNum, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1196, 739);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // dgvMetarial
            // 
            this.dgvMetarial.AllowUserToAddRows = false;
            this.dgvMetarial.AllowUserToDeleteRows = false;
            this.dgvMetarial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMetarial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetarial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.里程,
            this.轨枕编号,
            this.左股外口,
            this.左股里口,
            this.左股调高垫板,
            this.右股里口,
            this.右股外口,
            this.右股调高垫板});
            this.dgvMetarial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMetarial.Location = new System.Drawing.Point(2, 2);
            this.dgvMetarial.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvMetarial.Name = "dgvMetarial";
            this.dgvMetarial.RowHeadersWidth = 51;
            this.tableLayoutPanel1.SetRowSpan(this.dgvMetarial, 2);
            this.dgvMetarial.RowTemplate.Height = 27;
            this.dgvMetarial.Size = new System.Drawing.Size(826, 735);
            this.dgvMetarial.TabIndex = 3;
            this.dgvMetarial.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvMetarial_RowPostPaint_1);
            // 
            // 里程
            // 
            this.里程.HeaderText = "里程";
            this.里程.MinimumWidth = 6;
            this.里程.Name = "里程";
            this.里程.ReadOnly = true;
            // 
            // 轨枕编号
            // 
            this.轨枕编号.HeaderText = "轨枕编号";
            this.轨枕编号.MinimumWidth = 6;
            this.轨枕编号.Name = "轨枕编号";
            this.轨枕编号.ReadOnly = true;
            // 
            // 左股外口
            // 
            this.左股外口.HeaderText = "左股外口轨距垫厚度变化量";
            this.左股外口.MinimumWidth = 6;
            this.左股外口.Name = "左股外口";
            this.左股外口.ReadOnly = true;
            // 
            // 左股里口
            // 
            this.左股里口.HeaderText = "左股里口轨距垫厚度变化量";
            this.左股里口.MinimumWidth = 6;
            this.左股里口.Name = "左股里口";
            this.左股里口.ReadOnly = true;
            // 
            // 左股调高垫板
            // 
            this.左股调高垫板.HeaderText = "左股调高垫板厚度变化量";
            this.左股调高垫板.MinimumWidth = 6;
            this.左股调高垫板.Name = "左股调高垫板";
            this.左股调高垫板.ReadOnly = true;
            // 
            // 右股里口
            // 
            this.右股里口.HeaderText = "右股里口轨距垫厚度变化量";
            this.右股里口.MinimumWidth = 6;
            this.右股里口.Name = "右股里口";
            this.右股里口.ReadOnly = true;
            // 
            // 右股外口
            // 
            this.右股外口.HeaderText = "右股外口轨距垫厚度变化量";
            this.右股外口.MinimumWidth = 6;
            this.右股外口.Name = "右股外口";
            this.右股外口.ReadOnly = true;
            // 
            // 右股调高垫板
            // 
            this.右股调高垫板.HeaderText = "右股调高垫板厚度变化量";
            this.右股调高垫板.MinimumWidth = 6;
            this.右股调高垫板.Name = "右股调高垫板";
            this.右股调高垫板.ReadOnly = true;
            // 
            // dgvPPNum
            // 
            this.dgvPPNum.AllowUserToAddRows = false;
            this.dgvPPNum.AllowUserToDeleteRows = false;
            this.dgvPPNum.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPPNum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPPNum.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.轨距垫型号,
            this.Column1,
            this.轨距垫置换,
            this.轨距垫用量,
            this.作差});
            this.dgvPPNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPPNum.Location = new System.Drawing.Point(832, 2);
            this.dgvPPNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvPPNum.Name = "dgvPPNum";
            this.dgvPPNum.RowHeadersVisible = false;
            this.dgvPPNum.RowHeadersWidth = 51;
            this.dgvPPNum.RowTemplate.Height = 27;
            this.dgvPPNum.Size = new System.Drawing.Size(362, 365);
            this.dgvPPNum.TabIndex = 4;
            // 
            // 轨距垫型号
            // 
            this.轨距垫型号.HeaderText = "轨距垫型号";
            this.轨距垫型号.MinimumWidth = 6;
            this.轨距垫型号.Name = "轨距垫型号";
            this.轨距垫型号.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "对应厚度";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // 轨距垫置换
            // 
            this.轨距垫置换.HeaderText = "置换";
            this.轨距垫置换.MinimumWidth = 6;
            this.轨距垫置换.Name = "轨距垫置换";
            this.轨距垫置换.ReadOnly = true;
            // 
            // 轨距垫用量
            // 
            this.轨距垫用量.HeaderText = "用量";
            this.轨距垫用量.MinimumWidth = 6;
            this.轨距垫用量.Name = "轨距垫用量";
            // 
            // 作差
            // 
            this.作差.HeaderText = "作差";
            this.作差.MinimumWidth = 6;
            this.作差.Name = "作差";
            // 
            // dgvElevationNum
            // 
            this.dgvElevationNum.AllowUserToAddRows = false;
            this.dgvElevationNum.AllowUserToDeleteRows = false;
            this.dgvElevationNum.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvElevationNum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvElevationNum.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.调高垫板型号,
            this.Column2,
            this.调高垫板置换,
            this.用量,
            this.做差});
            this.dgvElevationNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvElevationNum.Location = new System.Drawing.Point(832, 371);
            this.dgvElevationNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvElevationNum.Name = "dgvElevationNum";
            this.dgvElevationNum.RowHeadersVisible = false;
            this.dgvElevationNum.RowHeadersWidth = 51;
            this.dgvElevationNum.RowTemplate.Height = 27;
            this.dgvElevationNum.Size = new System.Drawing.Size(362, 366);
            this.dgvElevationNum.TabIndex = 5;
            // 
            // 调高垫板型号
            // 
            this.调高垫板型号.HeaderText = "调高垫板型号";
            this.调高垫板型号.MinimumWidth = 6;
            this.调高垫板型号.Name = "调高垫板型号";
            this.调高垫板型号.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "对应厚度";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // 调高垫板置换
            // 
            this.调高垫板置换.HeaderText = "置换";
            this.调高垫板置换.MinimumWidth = 6;
            this.调高垫板置换.Name = "调高垫板置换";
            this.调高垫板置换.ReadOnly = true;
            // 
            // 用量
            // 
            this.用量.HeaderText = "用量";
            this.用量.MinimumWidth = 6;
            this.用量.Name = "用量";
            // 
            // 做差
            // 
            this.做差.HeaderText = "作差";
            this.做差.MinimumWidth = 6;
            this.做差.Name = "做差";
            // 
            // FrmCalcMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 766);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmCalcMaterial";
            this.Text = "计算调轨材料";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmCalcMaterial_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetarial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPPNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvElevationNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCalc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvMetarial;
        private System.Windows.Forms.DataGridViewTextBoxColumn 里程;
        private System.Windows.Forms.DataGridViewTextBoxColumn 轨枕编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 左股外口;
        private System.Windows.Forms.DataGridViewTextBoxColumn 左股里口;
        private System.Windows.Forms.DataGridViewTextBoxColumn 左股调高垫板;
        private System.Windows.Forms.DataGridViewTextBoxColumn 右股里口;
        private System.Windows.Forms.DataGridViewTextBoxColumn 右股外口;
        private System.Windows.Forms.DataGridViewTextBoxColumn 右股调高垫板;
        private System.Windows.Forms.DataGridView dgvPPNum;
        private System.Windows.Forms.DataGridView dgvElevationNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn 轨距垫型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 轨距垫置换;
        private System.Windows.Forms.DataGridViewTextBoxColumn 轨距垫用量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 作差;
        private System.Windows.Forms.DataGridViewTextBoxColumn 调高垫板型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 调高垫板置换;
        private System.Windows.Forms.DataGridViewTextBoxColumn 用量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 做差;
    }
}