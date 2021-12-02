namespace TrackRedesign {
    partial class FrmMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.MnuMain = new System.Windows.Forms.MenuStrip();
            this.miProjectMann = new System.Windows.Forms.ToolStripMenuItem();
            this.subCreatProject = new System.Windows.Forms.ToolStripMenuItem();
            this.subOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.选择文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subSetPara = new System.Windows.Forms.ToolStripMenuItem();
            this.subLoadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.terImportToFro = new System.Windows.Forms.ToolStripMenuItem();
            this.terImportAdjustFile = new System.Windows.Forms.ToolStripMenuItem();
            this.subImportData = new System.Windows.Forms.ToolStripMenuItem();
            this.terVievToFro = new System.Windows.Forms.ToolStripMenuItem();
            this.terViewAdjData = new System.Windows.Forms.ToolStripMenuItem();
            this.terViewModelData = new System.Windows.Forms.ToolStripMenuItem();
            this.查看基准股ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miDate = new System.Windows.Forms.ToolStripMenuItem();
            this.subCheckDate = new System.Windows.Forms.ToolStripMenuItem();
            this.subShowToFroImage = new System.Windows.Forms.ToolStripMenuItem();
            this.subAddAdjustLine = new System.Windows.Forms.ToolStripMenuItem();
            this.显示调整前后对比图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.计算调轨材料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.subExportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::TrackRedesign.WaitForm2), true, true, true);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.MnuMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MnuMain
            // 
            this.MnuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miProjectMann,
            this.miDate,
            this.miExport,
            this.帮助ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.MnuMain.Location = new System.Drawing.Point(0, 0);
            this.MnuMain.Name = "MnuMain";
            this.MnuMain.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MnuMain.Size = new System.Drawing.Size(1692, 28);
            this.MnuMain.TabIndex = 0;
            this.MnuMain.Text = "menuStrip1";
            // 
            // miProjectMann
            // 
            this.miProjectMann.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subCreatProject,
            this.subOpenProject,
            this.subSetPara,
            this.subLoadFile,
            this.subImportData,
            this.查看基准股ToolStripMenuItem});
            this.miProjectMann.Name = "miProjectMann";
            this.miProjectMann.Size = new System.Drawing.Size(83, 24);
            this.miProjectMann.Text = "项目管理";
            // 
            // subCreatProject
            // 
            this.subCreatProject.Name = "subCreatProject";
            this.subCreatProject.Size = new System.Drawing.Size(182, 26);
            this.subCreatProject.Text = "新建项目";
            this.subCreatProject.Click += new System.EventHandler(this.新建项目ToolStripMenuItem_Click);
            // 
            // subOpenProject
            // 
            this.subOpenProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择文件ToolStripMenuItem,
            this.清空ToolStripMenuItem});
            this.subOpenProject.Name = "subOpenProject";
            this.subOpenProject.Size = new System.Drawing.Size(182, 26);
            this.subOpenProject.Text = "打开项目";
            // 
            // 选择文件ToolStripMenuItem
            // 
            this.选择文件ToolStripMenuItem.Name = "选择文件ToolStripMenuItem";
            this.选择文件ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.选择文件ToolStripMenuItem.Text = "选择项目";
            this.选择文件ToolStripMenuItem.Click += new System.EventHandler(this.选择文件ToolStripMenuItem_Click);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // subSetPara
            // 
            this.subSetPara.Name = "subSetPara";
            this.subSetPara.Size = new System.Drawing.Size(182, 26);
            this.subSetPara.Text = "参数设置";
            this.subSetPara.Click += new System.EventHandler(this.参数设置ToolStripMenuItem_Click);
            // 
            // subLoadFile
            // 
            this.subLoadFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terImportToFro,
            this.terImportAdjustFile});
            this.subLoadFile.Name = "subLoadFile";
            this.subLoadFile.Size = new System.Drawing.Size(182, 26);
            this.subLoadFile.Text = "导入文件";
            // 
            // terImportToFro
            // 
            this.terImportToFro.Name = "terImportToFro";
            this.terImportToFro.Size = new System.Drawing.Size(182, 26);
            this.terImportToFro.Text = "OUT文件";
            this.terImportToFro.Click += new System.EventHandler(this.terImportToFro_Click);
            // 
            // terImportAdjustFile
            // 
            this.terImportAdjustFile.Name = "terImportAdjustFile";
            this.terImportAdjustFile.Size = new System.Drawing.Size(182, 26);
            this.terImportAdjustFile.Text = "轨道扣件文件";
            this.terImportAdjustFile.Click += new System.EventHandler(this.terImportAdjustFile_Click);
            // 
            // subImportData
            // 
            this.subImportData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terVievToFro,
            this.terViewAdjData,
            this.terViewModelData});
            this.subImportData.Name = "subImportData";
            this.subImportData.Size = new System.Drawing.Size(182, 26);
            this.subImportData.Text = "查看导入数据";
            // 
            // terVievToFro
            // 
            this.terVievToFro.Name = "terVievToFro";
            this.terVievToFro.Size = new System.Drawing.Size(167, 26);
            this.terVievToFro.Text = "往返测数据";
            this.terVievToFro.Click += new System.EventHandler(this.TerVievToFro_Click);
            // 
            // terViewAdjData
            // 
            this.terViewAdjData.Name = "terViewAdjData";
            this.terViewAdjData.Size = new System.Drawing.Size(167, 26);
            this.terViewAdjData.Text = "可调量数据";
            this.terViewAdjData.Click += new System.EventHandler(this.terViewAdjData_Click);
            // 
            // terViewModelData
            // 
            this.terViewModelData.Name = "terViewModelData";
            this.terViewModelData.Size = new System.Drawing.Size(167, 26);
            this.terViewModelData.Text = "型号数据";
            this.terViewModelData.Click += new System.EventHandler(this.terViewModelData_Click);
            // 
            // 查看基准股ToolStripMenuItem
            // 
            this.查看基准股ToolStripMenuItem.Name = "查看基准股ToolStripMenuItem";
            this.查看基准股ToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.查看基准股ToolStripMenuItem.Text = "查看基准股";
            this.查看基准股ToolStripMenuItem.Click += new System.EventHandler(this.查看基准股ToolStripMenuItem_Click);
            // 
            // miDate
            // 
            this.miDate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subCheckDate,
            this.subShowToFroImage,
            this.subAddAdjustLine,
            this.显示调整前后对比图ToolStripMenuItem,
            this.计算调轨材料ToolStripMenuItem});
            this.miDate.Name = "miDate";
            this.miDate.Size = new System.Drawing.Size(83, 24);
            this.miDate.Text = "数据处理";
            // 
            // subCheckDate
            // 
            this.subCheckDate.Name = "subCheckDate";
            this.subCheckDate.Size = new System.Drawing.Size(227, 26);
            this.subCheckDate.Text = "数据检查";
            this.subCheckDate.Click += new System.EventHandler(this.subCheckDate_Click);
            // 
            // subShowToFroImage
            // 
            this.subShowToFroImage.Name = "subShowToFroImage";
            this.subShowToFroImage.Size = new System.Drawing.Size(227, 26);
            this.subShowToFroImage.Text = "显示往返测图像";
            this.subShowToFroImage.Click += new System.EventHandler(this.subShowToFroImage_Click);
            // 
            // subAddAdjustLine
            // 
            this.subAddAdjustLine.Name = "subAddAdjustLine";
            this.subAddAdjustLine.Size = new System.Drawing.Size(227, 26);
            this.subAddAdjustLine.Text = "添加调整中线";
            this.subAddAdjustLine.Click += new System.EventHandler(this.subAddAdjustLine_Click);
            // 
            // 显示调整前后对比图ToolStripMenuItem
            // 
            this.显示调整前后对比图ToolStripMenuItem.Name = "显示调整前后对比图ToolStripMenuItem";
            this.显示调整前后对比图ToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.显示调整前后对比图ToolStripMenuItem.Text = "显示调整前后对比图";
            this.显示调整前后对比图ToolStripMenuItem.Click += new System.EventHandler(this.显示调整前后对比图ToolStripMenuItem_Click);
            // 
            // 计算调轨材料ToolStripMenuItem
            // 
            this.计算调轨材料ToolStripMenuItem.Name = "计算调轨材料ToolStripMenuItem";
            this.计算调轨材料ToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.计算调轨材料ToolStripMenuItem.Text = "计算调轨材料数量";
            this.计算调轨材料ToolStripMenuItem.Click += new System.EventHandler(this.计算调轨材料ToolStripMenuItem_Click);
            // 
            // miExport
            // 
            this.miExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subExportExcel});
            this.miExport.Name = "miExport";
            this.miExport.Size = new System.Drawing.Size(83, 24);
            this.miExport.Text = "文件导出";
            // 
            // subExportExcel
            // 
            this.subExportExcel.Name = "subExportExcel";
            this.subExportExcel.Size = new System.Drawing.Size(182, 26);
            this.subExportExcel.Text = "导出调整方案";
            this.subExportExcel.Click += new System.EventHandler(this.subExportExcel_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.帮助ToolStripMenuItem1});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem1
            // 
            this.帮助ToolStripMenuItem1.Name = "帮助ToolStripMenuItem1";
            this.帮助ToolStripMenuItem1.Size = new System.Drawing.Size(122, 26);
            this.帮助ToolStripMenuItem1.Text = "帮助";
            this.帮助ToolStripMenuItem1.Click += new System.EventHandler(this.帮助ToolStripMenuItem1_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 888);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1692, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 20);
            this.toolStripStatusLabel1.Text = "当前项目:";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1692, 914);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MnuMain);
            this.ForeColor = System.Drawing.SystemColors.MenuText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MnuMain;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmMain";
            this.Text = "厦门地铁轨道精调整数优化算法软件";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.MnuMain.ResumeLayout(false);
            this.MnuMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem miProjectMann;
        private System.Windows.Forms.ToolStripMenuItem subCreatProject;
        private System.Windows.Forms.ToolStripMenuItem subLoadFile;
        private System.Windows.Forms.ToolStripMenuItem subSetPara;
        private System.Windows.Forms.ToolStripMenuItem miDate;
        private System.Windows.Forms.ToolStripMenuItem subCheckDate;
        private System.Windows.Forms.ToolStripMenuItem subShowToFroImage;
        private System.Windows.Forms.ToolStripMenuItem subAddAdjustLine;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripMenuItem subExportExcel;
        private System.Windows.Forms.ToolStripMenuItem terImportToFro;
        private System.Windows.Forms.ToolStripMenuItem subImportData;
        private System.Windows.Forms.ToolStripMenuItem terVievToFro;
        private System.Windows.Forms.ToolStripMenuItem terImportAdjustFile;
        private System.Windows.Forms.ToolStripMenuItem terViewAdjData;
        private System.Windows.Forms.ToolStripMenuItem terViewModelData;
        private System.Windows.Forms.ToolStripMenuItem 显示调整前后对比图ToolStripMenuItem;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.ToolStripMenuItem 计算调轨材料ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.ToolStripMenuItem subOpenProject;
        private System.Windows.Forms.ToolStripMenuItem 查看基准股ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem 选择文件ToolStripMenuItem;
        public System.Windows.Forms.MenuStrip MnuMain;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem1;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}

