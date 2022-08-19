using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using TrackRedesign.Properties;
using System.Configuration;

namespace TrackRedesign {
    public partial class FrmMain : Form {

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED

                if (this.IsXpOr2003 == true) {
                    cp.ExStyle |= 0x00080000;  // Turn on WS_EX_LAYERED
                    this.Opacity = 1;
                }

                return cp;

            }

        }  //防止闪烁

        private Boolean IsXpOr2003 {
            get {
                OperatingSystem os = Environment.OSVersion;
                Version vs = os.Version;

                if (os.Platform == PlatformID.Win32NT)
                    if ((vs.Major == 5) && (vs.Minor != 0))
                        return true;
                    else
                        return false;
                else
                    return false;
            }
        }

        string writer = ConfigurationManager.AppSettings["writer"].Trim();
        string version = ConfigurationManager.AppSettings["version"].Trim();
        private readonly Encoding encoding = Encoding.Default;
        //readonly Encoding encoding = Encoding.GetEncoding("GB2312");
        public FrmMain() {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Bitmap bm = new Bitmap(Resources.backgroundImage2);//fbImage图片路径
            this.BackgroundImage = bm;//设置背景图片
            this.BackgroundImageLayout = ImageLayout.Stretch;//设置背景图片自动适应
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Text += $"  研制者:{writer}  版本号:{version}";

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            helpProvider1.HelpNamespace = Application.StartupPath + @"\软件操作手册.CHM";
            helpProvider1.SetShowHelp(this, true);
        }
        //定义全局变量
        public DataTable importW = null;
        public DataTable importF = null;
        public DataTable averageValue = new DataTable("偏差平均值");
        public DataTable tendLinePP = new DataTable("平面趋势线");
        public DataTable tendLineElevation = new DataTable("高程趋势线");
        public DataTable slopePP = new DataTable("平面调整中线斜率");
        public DataTable slopeElevation = new DataTable("高程调整中线斜率");
        public DataTable simulationAdjustmentPP = new DataTable("平面模拟调整量");
        public DataTable simulationAdjustmentElevation = new DataTable("高程模拟调整量");
        public DataTable actualAdjustmentPP = new DataTable("平面调整量取整");
        public DataTable actualAdjustmentElevation = new DataTable("高程调整量取整");
        //public DataTable importAdjustable = new DataTable("可调量数据");
        public DataTable basicTrackAdjustablePP = new DataTable("平面基准轨可调量");
        public DataTable basicTrackAdjustableElevation = new DataTable("高程基准轨可调量");
        public DataTable AdjustPlanPP = new DataTable("平面调整方案");
        public DataTable AdjustPlanElevation = new DataTable("调整方案");
        public DataTable export = new DataTable("导出文件");
        //public DataTable modelData = new DataTable("型号数据");
        public DataTable parameter = new DataTable("参数数据");
        public DataTable railFastening = new DataTable("轨道扣件");
        //调整量超限
        public DataTable overrunPP = null;
        public DataTable overrunEle = null;
        public DataTable zhexianPP = new DataTable("折线纵坐标");
        public int datumTrack = 3;//左轨0右轨1
        public string path = string.Empty;//定义工程项目路径
        public string[] pathFile = new string[2];
        public string prjName = string.Empty;

        //定义方案导出信息
        public string title;
        public DateTime time;
        public string maker;
        /// <summary>
        /// 关闭已打开的窗口
        /// </summary>
        public void CloseForm() {
            CloseOpenForm(typeof(FrmAddAdjustLine).Name);
            CloseOpenForm(typeof(FrmCalcMaterial).Name);
            CloseOpenForm(typeof(FrmSetting).Name);
            CloseOpenForm(typeof(FrmShowBAChart).Name);
            CloseOpenForm(typeof(FrmToFroImage).Name);
            CloseOpenForm(typeof(FrmViewAdjustableData).Name);
            CloseOpenForm(typeof(FrmViewModelData).Name);
            CloseOpenForm(typeof(FrmViewToAndFro).Name);
            CloseOpenForm(typeof(FrmInfo).Name);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearDt() {
            importW.Rows.Clear();
            importF.Rows.Clear();
            railFastening.Rows.Clear();
            AdjustPlanPP.Rows.Clear();
            AdjustPlanElevation.Rows.Clear();
            averageValue.Rows.Clear();
            tendLinePP.Rows.Clear();
            tendLineElevation.Rows.Clear();
            slopePP.Rows.Clear();
            slopeElevation.Rows.Clear();
            simulationAdjustmentPP.Rows.Clear();
            simulationAdjustmentElevation.Rows.Clear();
            actualAdjustmentPP.Rows.Clear();
            actualAdjustmentElevation.Rows.Clear();
            basicTrackAdjustablePP.Rows.Clear();
            basicTrackAdjustableElevation.Rows.Clear();
            export.Rows.Clear();
            path = string.Empty;
            datumTrack = 3;
            for (int i = 0; i < parameter.Rows.Count - 1; i++) {
                parameter.Rows[i][1] = 10;
            }
            parameter.Rows[parameter.Rows.Count - 1][1] = 1;
        }
        /// <summary>
        /// 打开新建项目窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建项目ToolStripMenuItem_Click(object sender, EventArgs e) {
            //CloseForm();
            bool b = CheckForm(typeof(FrmCreatNewProject).Name);
            if (!b) {
                FrmCreatNewProject fm = new FrmCreatNewProject(this);
                //fm.MdiParent = this;
                fm.TransfEvent += fm_TransfEvent;
                fm.ShowDialog();
            }
        }
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="value"></param>
        void fm_TransfEvent(string value) {
            path = value;
        }
        /// <summary>
        /// 打开参数设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(path)) {
                MessageBox.Show("请新建项目！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool b = CheckForm(typeof(FrmSetting).Name);
            if (!b) {
                FrmSetting setting = new FrmSetting(this) {
                    //MdiParent = this
                };
                setting.ShowDialog();
            }
        }

        /// <summary>
        /// 初始化往返册Datatable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private DataTable InitDatatable(string name) {
            DataTable dt = new DataTable(name);
            dt.Columns.Add("里程", Type.GetType("System.Double"));
            dt.Columns.Add("调前高程", Type.GetType("System.Double"));
            dt.Columns.Add("调前平面", Type.GetType("System.Double"));
            dt.Columns.Add("调前水平", Type.GetType("System.Double"));
            dt.Columns.Add("调前轨距", Type.GetType("System.Double"));
            dt.Columns.Add("导向轨", Type.GetType("System.String"));
            dt.Columns.Add("调左轨高", Type.GetType("System.Double"));
            dt.Columns.Add("调右轨高", Type.GetType("System.Double"));
            dt.Columns.Add("调左平面", Type.GetType("System.Double"));
            dt.Columns.Add("调右平面", Type.GetType("System.Double"));
            dt.Columns.Add("调后高程", Type.GetType("System.Double"));
            dt.Columns.Add("调后平面", Type.GetType("System.Double"));
            dt.Columns.Add("调后水平", Type.GetType("System.Double"));
            dt.Columns.Add("调后轨距", Type.GetType("System.Double"));
            dt.Columns.Add("设计超高", Type.GetType("System.Double"));
            return dt;
        }

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e) {

            #region 初始化每个Datatable
            zhexianPP.Columns.Add("里程", Type.GetType("System.Double"));
            zhexianPP.Columns.Add("纵坐标", Type.GetType("System.Double"));

            //初始化slopeP
            slopePP.Columns.Add("起始里程", Type.GetType("System.Double"));
            slopePP.Columns.Add("结束里程", Type.GetType("System.Double"));
            slopePP.Columns.Add("斜率", Type.GetType("System.String"));
            slopePP.Columns.Add("截距", Type.GetType("System.Double"));
            //初始化slopeG
            slopeElevation.Columns.Add("起始里程", Type.GetType("System.Double"));
            slopeElevation.Columns.Add("结束里程", Type.GetType("System.Double"));
            slopeElevation.Columns.Add("斜率", Type.GetType("System.String"));
            slopeElevation.Columns.Add("截距", Type.GetType("System.Double"));
            //初始化averageValue
            averageValue.Columns.Add("里程", Type.GetType("System.Double"));
            averageValue.Columns.Add("调前平面", Type.GetType("System.Double"));
            averageValue.Columns.Add("调前轨距", Type.GetType("System.Double"));
            averageValue.Columns.Add("调前高程", Type.GetType("System.Double"));
            averageValue.Columns.Add("调前水平", Type.GetType("System.Double"));
            averageValue.Columns.Add("调左轨高", Type.GetType("System.Double"));
            averageValue.Columns.Add("调右轨高", Type.GetType("System.Double"));
            averageValue.Columns.Add("调左平面", Type.GetType("System.Double"));
            averageValue.Columns.Add("调右平面", Type.GetType("System.Double"));
            averageValue.Columns.Add("设计超高", Type.GetType("System.Double"));
            //初始化import
            importW = InitDatatable("往测数据");
            importF = InitDatatable("返测数据");
            //初始化tendLineP
            tendLinePP.Columns.Add("里程", Type.GetType("System.Double"));
            tendLinePP.Columns.Add("偏差值", Type.GetType("System.Double"));
            //初始化tendLineG
            tendLineElevation.Columns.Add("里程", Type.GetType("System.Double"));
            tendLineElevation.Columns.Add("偏差值", Type.GetType("System.Double"));
            //初始化simulationAdjustmentPP
            simulationAdjustmentPP.Columns.Add("里程", Type.GetType("System.Double"));
            simulationAdjustmentPP.Columns.Add("调左平面", Type.GetType("System.Double"));
            simulationAdjustmentPP.Columns.Add("调右平面", Type.GetType("System.Double"));
            simulationAdjustmentPP.Columns.Add("调后平面", Type.GetType("System.Double"));
            simulationAdjustmentPP.Columns.Add("调后轨距", Type.GetType("System.Double"));
            //初始化simulationAdjustmentElevation
            simulationAdjustmentElevation.Columns.Add("里程", Type.GetType("System.Double"));
            simulationAdjustmentElevation.Columns.Add("调左高程", Type.GetType("System.Double"));
            simulationAdjustmentElevation.Columns.Add("调右高程", Type.GetType("System.Double"));
            simulationAdjustmentElevation.Columns.Add("调后高程", Type.GetType("System.Double"));
            simulationAdjustmentElevation.Columns.Add("调后水平", Type.GetType("System.Double"));
            //初始化actualAdjustmentPP
            actualAdjustmentPP.Columns.Add("里程", Type.GetType("System.Double"));
            actualAdjustmentPP.Columns.Add("调左平面", Type.GetType("System.Double"));
            actualAdjustmentPP.Columns.Add("调右平面", Type.GetType("System.Double"));
            //初始化actualAdjustmentElevation
            actualAdjustmentElevation.Columns.Add("里程", Type.GetType("System.Double"));
            actualAdjustmentElevation.Columns.Add("调左高程", Type.GetType("System.Double"));
            actualAdjustmentElevation.Columns.Add("调右高程", Type.GetType("System.Double"));

            //初始化railFastening
            railFastening.Columns.Add("轨枕数", Type.GetType("System.Double"));
            railFastening.Columns.Add("轨枕编号", Type.GetType("System.String"));
            railFastening.Columns.Add("里程", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股外口轨距块", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股里口轨距块", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股胶垫厚度", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股里口轨距块", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股外口轨距块", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股胶垫厚度", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股左调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股右调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股上调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("左股下调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股左调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股右调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股上调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("右股下调量", Type.GetType("System.Double"));
            railFastening.Columns.Add("CP3里程", Type.GetType("System.Double"));
            railFastening.Columns.Add("CP3点号", Type.GetType("System.String"));

            //初始化basicTrackAdjustablePP
            basicTrackAdjustablePP.Columns.Add("轨枕编号", Type.GetType("System.String"));
            basicTrackAdjustablePP.Columns.Add("里程", Type.GetType("System.String"));
            basicTrackAdjustablePP.Columns.Add("基准股右调", Type.GetType("System.String"));//>0
            basicTrackAdjustablePP.Columns.Add("基准股左调", Type.GetType("System.String"));//<0

            //初始化basicTrackAdjustableEvelation
            basicTrackAdjustableElevation.Columns.Add("轨枕编号", Type.GetType("System.String"));
            basicTrackAdjustableElevation.Columns.Add("里程", Type.GetType("System.String"));
            basicTrackAdjustableElevation.Columns.Add("基准股下调", Type.GetType("System.String"));//>0
            basicTrackAdjustableElevation.Columns.Add("基准股上调", Type.GetType("System.String"));//<0
            //初始化export   
            export.Columns.Add("测量点", Type.GetType("System.Int32"));
            export.Columns.Add("轨枕编号", Type.GetType("System.String"));
            export.Columns.Add("里程/m", Type.GetType("System.Double"));
            export.Columns.Add("调前高程/mm", Type.GetType("System.Double"));
            export.Columns.Add("调前平面/mm", Type.GetType("System.Double"));
            export.Columns.Add("调前水平/mm", Type.GetType("System.Double"));
            export.Columns.Add("调前轨距/mm", Type.GetType("System.Double"));
            export.Columns.Add("基准股", Type.GetType("System.String"));
            export.Columns.Add("调左高程/mm", Type.GetType("System.Double"));
            export.Columns.Add("调右高程/mm", Type.GetType("System.Double"));
            export.Columns.Add("调左平面/mm", Type.GetType("System.Double"));
            export.Columns.Add("调右平面/mm", Type.GetType("System.Double"));
            export.Columns.Add("调后高程/mm", Type.GetType("System.Double"));
            export.Columns.Add("调后平面/mm", Type.GetType("System.Double"));
            export.Columns.Add("调后水平/mm", Type.GetType("System.Double"));
            export.Columns.Add("调后轨距/mm", Type.GetType("System.Double"));
            export.Columns.Add("注解", Type.GetType("System.String"));
            export.Columns.Add("设计超高/mm", Type.GetType("System.Double"));
            export.Columns.Add("CP3里程/m", Type.GetType("System.Double"));



            //初始化parameter
            parameter.Columns.Add("参数名", Type.GetType("System.String"));
            parameter.Columns.Add("参数值", Type.GetType("System.Double"));
            DataRow dr1 = parameter.NewRow();
            dr1["参数名"] = "高程往返测限差";
            dr1["参数值"] = 10;
            parameter.Rows.Add(dr1);
            DataRow dr2 = parameter.NewRow();
            dr2["参数名"] = "平面往返测限差";
            dr2["参数值"] = 10;
            parameter.Rows.Add(dr2);
            DataRow dr3 = parameter.NewRow();
            dr3["参数名"] = "轨距往返测限差";
            dr3["参数值"] = 10;
            parameter.Rows.Add(dr3);
            DataRow dr4 = parameter.NewRow();
            dr4["参数名"] = "水平往返测限差";
            dr4["参数值"] = 10;
            parameter.Rows.Add(dr4);
            DataRow dr5 = parameter.NewRow();
            dr5["参数名"] = "扣件极差";
            dr5["参数值"] = 1;
            parameter.Rows.Add(dr5);

            //初始化AdjustPlanPP
            AdjustPlanPP.Columns.Add("测量点", Type.GetType("System.Int32"));
            AdjustPlanPP.Columns.Add("里程", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("轨枕编号", Type.GetType("System.String"));
            AdjustPlanPP.Columns.Add("调前平面", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("调前轨距", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("调左平面", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("调右平面", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("调后平面", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("调后轨距", Type.GetType("System.Double"));
            AdjustPlanPP.Columns.Add("基准股", Type.GetType("System.String"));

            //初始化AdjustPlanElevation
            AdjustPlanElevation.Columns.Add("测量点", Type.GetType("System.Int32"));
            AdjustPlanElevation.Columns.Add("里程", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("轨枕编号", Type.GetType("System.String"));
            AdjustPlanElevation.Columns.Add("调前高程", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("调前水平", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("调左高程", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("调右高程", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("调后高程", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("调后水平", Type.GetType("System.Double"));
            AdjustPlanElevation.Columns.Add("基准股", Type.GetType("System.String"));
            #endregion

            XmlNodeList xmlNodeList = XMLHelper.ReadXml();
            if (xmlNodeList != null) {
                ToolStripMenuItem terMenu;
                foreach (XmlElement item in xmlNodeList) {
                    string filePath = item.InnerText;
                    terMenu = new ToolStripMenuItem {
                        Name = "ter" + item.GetAttribute("name"),
                        Text = item.InnerText
                    };
                    //注册事件
                    terMenu.Click += new EventHandler(terMenu_Click);
                    ((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Add(terMenu);
                }
            }
        }


        /// <summary>
        /// 动态添加子菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void terMenu_Click(object sender, EventArgs e) {
            ToolStripMenuItem downItem = sender as ToolStripMenuItem;
            if (!Directory.Exists(downItem.Text)) {
                MessageBox.Show("该项目已被删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                XMLHelper.RemoveElement(downItem.Text);
                ((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Remove(downItem);
                return;
            }
            toolStripStatusLabel1.Text = $"当前项目:{downItem.Text}";
            ClearDt();
            CloseForm();
            path = downItem.Text;
            string[] arr = path.Split(new char[] { '\\' });
            prjName = arr[arr.Length - 2] + ".prj";

            //读取文件路径
            Array.Clear(pathFile, 0, pathFile.Length);
            DirectoryInfo root = new DirectoryInfo(path + "原始文件");
            foreach (FileInfo path in root.GetFiles()) {
                //if (Regex.IsMatch(path.FullName, ".OUT")) {
                //    pathFile[0] = path.FullName;
                //    continue;
                //}
                //if (Regex.IsMatch(path.FullName, ".fas")) {
                //    pathFile[1] = path.FullName;
                //    continue;
                //}
                if (path.FullName.EndsWith(".OUT")) {
                    pathFile[0] = path.FullName;
                    continue;
                }
                if (path.FullName.EndsWith(".fas")) {
                    pathFile[1] = path.FullName;
                    continue;
                }
            }
            if (string.IsNullOrEmpty(pathFile[0])) {
                if (MessageBox.Show("未检测到OUT文件，是否手动选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    OpenFileDialog fileDialog = new OpenFileDialog {
                        Multiselect = false,
                        InitialDirectory = path + "原始文件",
                        Title = "请手动选择OUT文件",
                        Filter = "(OUT)|*.OUT|所有文件(*.*)|*.*"
                    };
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        pathFile[0] = fileDialog.FileName;
                    }
                }
            }
            if (string.IsNullOrEmpty(pathFile[1])) {
                if (MessageBox.Show("未检测到轨道扣件文件，是否手动选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    OpenFileDialog fileDialog = new OpenFileDialog {
                        Multiselect = false,
                        InitialDirectory = path + "原始文件",
                        Title = "请手动选择轨道扣件文件",
                        Filter = "(fas)|*.fas|所有文件(*.*)|*.*"
                    };
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        pathFile[1] = fileDialog.FileName;
                    }
                }
            }
            splashScreenManager1.ShowWaitForm();
            try {
                //导入往返测文件
                string report = string.Empty;
                if (File.Exists(pathFile[0])) {
                    //string content = File.ReadAllText(pathFile[0], encoding);
                    //File.WriteAllText(pathFile[0], content, encoding);
                    bool boo = IOHelper.OutDataImportToDataTable(importW, importF, pathFile[0]);
                    if (boo) {
                        if (Path.GetDirectoryName(pathFile[0]) != (path + "原始文件")) {
                            //删除同名文件
                            DirectoryInfo info = new DirectoryInfo(path + "原始文件");
                            foreach (FileInfo path in info.GetFiles()) {
                                if (Regex.IsMatch(path.FullName, ".OUT")) {
                                    File.Delete(path.FullName);
                                }
                            }
                            File.Copy(pathFile[0], path + @"原始文件\" + pathFile[0].Substring(pathFile[0].LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                        }
                        if (importW.Rows[0]["导向轨"].ToString() == "左轨" ||
                            importW.Rows[0]["导向轨"].ToString() == "左" || importW.Rows[0]["导向轨"].ToString() == "左股")
                            datumTrack = 0;
                        if (importW.Rows[0]["导向轨"].ToString() == "右轨" ||
                            importW.Rows[0]["导向轨"].ToString() == "右" || importW.Rows[0]["导向轨"].ToString() == "右股")
                            datumTrack = 1;
                        averageValue.Rows.Clear();
                        CalcAve();
                        IOHelper.DataTableExportToTxt(averageValue, path + @"预处理文件\往返测平均值.txt");
                    }
                }
                else {
                    report += "未找到OUT文件\n";
                }
                //导入轨道扣件文件
                if (File.Exists(pathFile[1])) {
                    //string content = File.ReadAllText(pathFile[1], encoding);
                    //File.WriteAllText(pathFile[1], content, encoding);
                    bool boo = IOHelper.TxtToDataTable(railFastening, pathFile[1]);
                    if (boo) {
                        if (Path.GetDirectoryName(pathFile[1]) != (path + "原始文件")) {
                            DirectoryInfo info2 = new DirectoryInfo(path + "原始文件");
                            foreach (FileInfo path in info2.GetFiles()) {
                                if (Regex.IsMatch(path.FullName, "扣件")) {
                                    File.Delete(path.FullName);
                                }
                            }
                            File.Copy(pathFile[1], path + "原始文件\\" + pathFile[1].Substring(pathFile[1].LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                        }
                    }
                }
                else {
                    report += "未找到轨道扣件文件\n";
                }
                ////导入往返测平均值文件
                //if (File.Exists(path + "预处理文件\\往返测平均值.txt")) {
                //    IOHelper.AllTxtToDataTable(averageValue, path + "预处理文件\\往返测平均值.txt");
                //}
                //else {
                //    report += "未找到往返测平均值文件\n";
                //}
                //导入平面基准股可调量
                if (File.Exists(path + "调整文件\\平面基准股可调量.txt")) {
                    IOHelper.AllTxtToDataTable(basicTrackAdjustablePP, path + "调整文件\\平面基准股可调量.txt");
                }
                else {
                    report += "未找到平面基准股可调量文件\n";
                }
                //导入平面模拟调整量
                if (File.Exists(path + "调整文件\\平面模拟调整量.txt")) {
                    IOHelper.AllTxtToDataTable(simulationAdjustmentPP, path + "调整文件\\平面模拟调整量.txt");
                }
                else {
                    report += "未找到平面模拟调整量文件\n";
                }
                //导入平面模拟调整量取整
                if (File.Exists(path + "调整文件\\平面模拟调整量取整.txt")) {
                    IOHelper.AllTxtToDataTable(actualAdjustmentPP, path + "调整文件\\平面模拟调整量取整.txt");
                }
                else {
                    report += "未找到平面模拟调整量取整文件\n";
                }
                //导入平面调整方案
                if (File.Exists(path + "调整文件\\平面调整方案.txt")) {
                    IOHelper.AllTxtToDataTable(AdjustPlanPP, path + "调整文件\\平面调整方案.txt");
                }
                else {
                    report += "未找到平面调整方案文件\n";
                }
                //导入平面调整中线
                if (File.Exists(path + "调整文件\\平面调整中线.txt")) {
                    IOHelper.AllTxtToDataTable(tendLinePP, path + "调整文件\\平面调整中线.txt");
                }
                else {
                    report += "未找到平面调整中线文件\n";
                }
                //导入平面调整中线斜率
                if (File.Exists(path + "调整文件\\平面调整中线斜率.txt")) {
                    IOHelper.AllTxtToDataTable(slopePP, path + "调整文件\\平面调整中线斜率.txt");
                }
                else {
                    report += "未找到平面调整中线斜率文件\n";
                }
                //导入高程基准股可调量
                if (File.Exists(path + "调整文件\\高程基准股可调量.txt")) {
                    IOHelper.AllTxtToDataTable(basicTrackAdjustableElevation, path + "调整文件\\高程基准股可调量.txt");
                }
                else {
                    report += "未找到高程基准股可调量文件\n";
                }
                //导入高程模拟调整量
                if (File.Exists(path + "调整文件\\高程模拟调整量.txt")) {
                    IOHelper.AllTxtToDataTable(simulationAdjustmentElevation, path + "调整文件\\高程模拟调整量.txt");
                }
                else {
                    report += "未找到高程模拟调整量文件\n";
                }
                //导入高程模拟调整量取整
                if (File.Exists(path + "调整文件\\高程模拟调整量取整.txt")) {
                    IOHelper.AllTxtToDataTable(actualAdjustmentElevation, path + "调整文件\\高程模拟调整量取整.txt");
                }
                else {
                    report += "未找到高程模拟调整量取整文件\n";
                }
                //导入高程调整方案
                if (File.Exists(path + "调整文件\\高程调整方案.txt")) {
                    IOHelper.AllTxtToDataTable(AdjustPlanElevation, path + "调整文件\\高程调整方案.txt");
                }
                else {
                    report += "未找到高程调整方案文件\n";
                }
                //导入高程调整中线
                if (File.Exists(path + "调整文件\\高程调整中线.txt")) {
                    IOHelper.AllTxtToDataTable(tendLineElevation, path + "调整文件\\高程调整中线.txt");
                }
                else {
                    report += "未找到高程调整中线文件\n";
                }
                //导入高程调整中线斜率
                if (File.Exists(path + "调整文件\\高程调整中线斜率.txt")) {
                    IOHelper.AllTxtToDataTable(slopeElevation, path + "调整文件\\高程调整中线斜率.txt");
                }
                else {
                    report += "未找到高程调整中线斜率文件\n";
                }
                //导入参数
                if (File.Exists(path + "预处理文件\\参数文件.txt")) {
                    //string content = File.ReadAllText(path + "预处理文件\\参数文件.txt", Encoding.Default);
                    //File.WriteAllText(path + "预处理文件\\参数文件.txt", content, Encoding.UTF8);
                    IOHelper.ParaTxtToDataTable(parameter, path + "预处理文件\\参数文件.txt");
                }
                else {
                    report += "未找到参数文件\n";
                }
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show("打开成功！\n" + report, "打开提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show("发生异常：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 打开调整中线窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subAddAdjustLine_Click(object sender, EventArgs e) {
            bool b = CheckForm(typeof(FrmAddAdjustLine).Name);
            if (!b) {
                if (averageValue.Rows.Count == 0 && (AdjustPlanPP.Rows.Count == 0 || AdjustPlanElevation.Rows.Count == 0)) {
                    MessageBox.Show("请导入OUT文件！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (railFastening.Rows.Count == 0 && (AdjustPlanPP.Rows.Count == 0 || AdjustPlanElevation.Rows.Count == 0)) {
                    MessageBox.Show("请导入轨道扣件数据！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (datumTrack == 3) {
                    MessageBox.Show("未识别出基准股，请检查OUT文件是否为乱码！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                splashScreenManager1.ShowWaitForm();
                FrmAddAdjustLine frmAdd = new FrmAddAdjustLine(this) {
                    MdiParent = this
                };
                frmAdd.Show();
                string str = string.Empty;
                for (int i = 0; i < pathFile.Length; i++) {
                    str += pathFile[i] + "\r\n";
                }
                IOHelper.WriteStrToTxt(str, path + @"原始文件\原始文件路径.txt");
                splashScreenManager1.CloseWaitForm();
            }
        }

        /// <summary>
        /// 导入往返测数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void terImportToFro_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(path)) {
                MessageBox.Show("请新建项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (importW.Rows.Count == 0) {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = false;
                fileDialog.Title = "请选择OUT文件";
                fileDialog.Filter = "(.OUT)|*.OUT|所有文件|*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK) {

                    string file = fileDialog.FileName;
                    //string content = File.ReadAllText(file, encoding);
                    //File.WriteAllText(file, content, encoding);
                    bool boo = IOHelper.OutDataImportToDataTable(importW, importF, file);
                    if (Path.GetDirectoryName(file) != (path + "原始文件")) {
                        //删除同名文件
                        DirectoryInfo root = new DirectoryInfo(path + "原始文件");
                        foreach (FileInfo path in root.GetFiles()) {
                            if (Regex.IsMatch(path.FullName, ".OUT")) {
                                File.Delete(path.FullName);
                            }
                        }
                        File.Copy(file, path + @"原始文件\" + file.Substring(file.LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                    }
                    pathFile[0] = file;
                    if (boo) {
                        if (importW.Rows[0]["导向轨"].ToString() == "左轨" ||
                        importW.Rows[0]["导向轨"].ToString() == "左" || importW.Rows[0]["导向轨"].ToString() == "左股")
                            datumTrack = 0;
                        if (importW.Rows[0]["导向轨"].ToString() == "右轨" ||
                            importW.Rows[0]["导向轨"].ToString() == "右" || importW.Rows[0]["导向轨"].ToString() == "右股")
                            datumTrack = 1;
                        MessageBox.Show("导入成功", "导入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        #region 计算平均值
                        averageValue.Rows.Clear();
                        CalcAve();
                        IOHelper.DataTableExportToTxt(averageValue, path + @"预处理文件\往返测平均值.txt");
                        //dataTableExportToText(averageValue, path + "预处理文件\\平均值.txt");
                        #endregion
                    }
                    else {
                        return;
                    }
                }
            }
            else {
                if (MessageBox.Show("是否重新导入？", "导入提示", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes) {
                    importW.Rows.Clear();
                    importF.Rows.Clear();
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = false;
                    fileDialog.Title = "请选择OUT文件";
                    fileDialog.Filter = "(.OUT)|*.OUT|所有文件|*.*";
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        string file = fileDialog.FileName;
                        //string content = File.ReadAllText(file, encoding);
                        //File.WriteAllText(file, content, encoding);
                        bool boo = IOHelper.OutDataImportToDataTable(importW, importF, file);
                        if (boo) {
                            if (Path.GetDirectoryName(file) != (path + "原始文件")) {
                                //删除同名文件
                                DirectoryInfo root = new DirectoryInfo(path + "原始文件");
                                foreach (FileInfo path in root.GetFiles()) {
                                    if (Regex.IsMatch(path.FullName, ".OUT")) {
                                        File.Delete(path.FullName);
                                    }
                                }
                                File.Copy(file, path + @"原始文件\" + file.Substring(file.LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                            }
                            //pathFile[0] = path + @"原始文件\" + Path.GetFileName(file) + "\r\n";
                            pathFile[0] = file;
                            if (importW.Rows[0]["导向轨"].ToString() == "左轨" ||
                        importW.Rows[0]["导向轨"].ToString() == "左" || importW.Rows[0]["导向轨"].ToString() == "左股")
                                datumTrack = 0;
                            if (importW.Rows[0]["导向轨"].ToString() == "右轨" ||
                                importW.Rows[0]["导向轨"].ToString() == "右" || importW.Rows[0]["导向轨"].ToString() == "右股")
                                datumTrack = 1;
                            MessageBox.Show("导入成功", "导入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            #region 计算平均值
                            averageValue.Rows.Clear();
                            CalcAve();
                            IOHelper.DataTableExportToTxt(averageValue, path + @"预处理文件\往返测平均值.txt");
                            //dataTableExportToText(averageValue, path + "预处理文件\\平均值.txt");
                            #endregion
                        }
                        else {
                            return;
                        }
                    }
                }
            }
        }

        private void CalcAve() {
            for (int i = 0; i < importW.Rows.Count; i++) {
                DataRow dr = averageValue.NewRow();
                double num1 = Convert.ToDouble(importW.Rows[i]["调前平面"].ToString());
                double num2 = Convert.ToDouble(importF.Rows[i]["调前平面"].ToString());
                double num3 = Convert.ToDouble(importW.Rows[i]["调前轨距"].ToString());
                double num4 = Convert.ToDouble(importF.Rows[i]["调前轨距"].ToString());
                double num5 = Convert.ToDouble(importW.Rows[i]["调前高程"].ToString());
                double num6 = Convert.ToDouble(importF.Rows[i]["调前高程"].ToString());
                double num7 = Convert.ToDouble(importW.Rows[i]["调前水平"].ToString());
                double num8 = Convert.ToDouble(importF.Rows[i]["调前水平"].ToString());
                double num9 = Convert.ToDouble(importW.Rows[i]["设计超高"].ToString());
                double num10 = Convert.ToDouble(importF.Rows[i]["设计超高"].ToString());
                double num11 = Convert.ToDouble(importW.Rows[i]["调左轨高"].ToString());
                double num12 = Convert.ToDouble(importF.Rows[i]["调左轨高"].ToString());
                double num13 = Convert.ToDouble(importW.Rows[i]["调右轨高"].ToString());
                double num14 = Convert.ToDouble(importF.Rows[i]["调右轨高"].ToString());
                double num15 = Convert.ToDouble(importW.Rows[i]["调左平面"].ToString());
                double num16 = Convert.ToDouble(importF.Rows[i]["调左平面"].ToString());
                double num17 = Convert.ToDouble(importW.Rows[i]["调右平面"].ToString());
                double num18 = Convert.ToDouble(importF.Rows[i]["调右平面"].ToString());
                dr["里程"] = double.Parse(importW.Rows[i]["里程"].ToString());
                dr["调前平面"] = (num1 + num2) * 0.5;
                dr["调前轨距"] = (num3 + num4) * 0.5;
                dr["调前高程"] = (num5 + num6) * 0.5;
                dr["调前水平"] = (num7 + num8) * 0.5;
                dr["设计超高"] = (num9 + num10) * 0.5;
                dr["调左轨高"] = (num11 + num12) * 0.5;
                dr["调右轨高"] = (num13 + num14) * 0.5;
                dr["调左平面"] = (num15 + num16) * 0.5;
                dr["调右平面"] = (num17 + num18) * 0.5;
                averageValue.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 打开往返测数据窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TerVievToFro_Click(object sender, EventArgs e) {
            bool b = CheckForm(typeof(FrmViewToAndFro).Name);
            if (!b) {
                FrmViewToAndFro viewToAndFro = new FrmViewToAndFro();
                viewToAndFro.MdiParent = this;
                viewToAndFro.Tag = new TagObject() {
                    DtToDate = importW,
                    DtFroDate = importF
                };
                viewToAndFro.Show();
            }
        }
        /// <summary>
        /// 打开往返测图像窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subShowToFroImage_Click(object sender, EventArgs e) {
            try {
                bool b = CheckForm(typeof(FrmToFroImage).Name);
                if (!b) {
                    FrmToFroImage fm = new FrmToFroImage();
                    fm.Tag = new TagObject() {
                        DtToDate = importF,
                        DtFroDate = importW,
                        Path = path
                    };
                    fm.MdiParent = this;
                    fm.Show();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误，请重试:" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 检查当前窗口是否打开,如果打开就激活窗体
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool CheckForm(string formName) {
            bool b = false;
            foreach (Form f in Application.OpenForms) {
                if (f.Name == formName) {
                    b = true;
                    f.Activate();
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// 检查当前窗体是否打开,如果打开则关闭
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private static void CloseOpenForm(string formName) {
            foreach (Form f in Application.OpenForms) {
                if (f.Name == formName) {
                    f.Close();
                    break;
                }
            }
        }
        /// <summary>
        /// 导入轨道扣件文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void terImportAdjustFile_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(path)) {
                MessageBox.Show("请新建项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (railFastening.Rows.Count == 0) {
                OpenFileDialog fileDialog = new OpenFileDialog {
                    Multiselect = false,
                    Title = "请选择轨道扣件文件",
                    Filter = "(.fas)|*.fas|所有文件|*.*"
                };
                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    string file = fileDialog.FileName;
                    //string content = File.ReadAllText(file, encoding);
                    //File.WriteAllText(file, content, encoding);
                    bool boo = IOHelper.TxtToDataTable(railFastening, file);
                    if (boo) {
                        if (Path.GetDirectoryName(file) != (path + "原始文件")) {
                            DirectoryInfo root = new DirectoryInfo(path + "原始文件");
                            foreach (FileInfo path in root.GetFiles()) {
                                if (Regex.IsMatch(path.FullName, "扣件")) {
                                    File.Delete(path.FullName);
                                }
                            }
                            File.Copy(file, path + "原始文件\\" + file.Substring(file.LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                                                                                                                //pathFile[1] = path + @"原始文件\" + Path.GetFileName(file) + "\r\n";
                        }
                        pathFile[1] = file;
                        MessageBox.Show("导入成功", "导入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        return;
                    }
                }
            }
            else {
                if (MessageBox.Show("是否重新导入？", "导入提示", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes) {
                    railFastening.Rows.Clear();
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = false;
                    fileDialog.Title = "请选择轨道扣件文件";
                    fileDialog.Filter = "(.fas)|*.fas|所有文件|*.*";
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        string file = fileDialog.FileName;
                        //string content = File.ReadAllText(file, encoding);
                        //File.WriteAllText(file, content, encoding);
                        bool boo = IOHelper.TxtToDataTable(railFastening, file);
                        if (boo) {
                            if (Path.GetDirectoryName(file) != (path + "原始文件")) {
                                DirectoryInfo root = new DirectoryInfo(path + "原始文件");
                                foreach (FileInfo path in root.GetFiles()) {
                                    if (Regex.IsMatch(path.FullName, "扣件")) {
                                        File.Delete(path.FullName);
                                    }
                                }
                                File.Copy(file, path + "原始文件\\" + file.Substring(file.LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                                                                                                                    //pathFile[1] = path + @"原始文件\" + Path.GetFileName(file) + "\r\n";
                            }
                            pathFile[1] = file;
                            MessageBox.Show("导入成功", "导入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else {
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 查看可调量数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void terViewAdjData_Click(object sender, EventArgs e) {
            bool b = CheckForm(typeof(FrmViewAdjustableData).Name);
            if (!b) {
                FrmViewAdjustableData frm = new FrmViewAdjustableData();
                frm.Tag = railFastening;
                frm.MdiParent = this;
                frm.Show();
            }
        }
        /// <summary>
        /// 数据检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subCheckDate_Click(object sender, EventArgs e) {
            if (importF.Rows.Count == 0 || importW.Rows.Count == 0 || averageValue.Rows.Count == 0) {
                MessageBox.Show("请导入OUT文件！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (railFastening.Rows.Count == 0) {
                MessageBox.Show("请导入轨道扣件数据！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string miles = string.Empty;
            string split = "\r\n---------------------------------------------------------\r\n";
            StringBuilder message = new StringBuilder("---------------------------------------------------------\r\n" +
                "                    数据检查结果" + split);
            message.Append(CheckHelper.CheckStopper(railFastening));
            message.Append(CheckHelper.CheckSpacer(railFastening));
            message.Append(CheckHelper.CheckMiles(importF, railFastening, out miles));
            message.Append(CheckHelper.CheckToAndFroData(importW, importF, parameter));
            IOHelper.WriteStrToTxt(message.ToString(), path + @"预处理文件\检测结果.txt");
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, prjName), true, encoding)) {
                sw.Write(miles + "\r");
            }
            DirectoryInfo dir = new DirectoryInfo(path + "原始文件");
            foreach (FileInfo file in dir.GetFiles()) {
                if (file.FullName.Contains(".fas")) {
                    file.Delete();
                }
            }
            IOHelper.DataTableExportToTxt(railFastening, path + @"原始文件\轨道扣件.fas");
            if (MessageBox.Show("检查完毕，是否查看检查结果？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                System.Diagnostics.Process.Start("Explorer", "/select," + path + @"预处理文件\" + "检测结果.txt");
            }
        }
        /// <summary>
        /// 退出系统提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            DialogResult result = MessageBox.Show("您确定要关闭软件吗？", "退出提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.ExitThread();
            else {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 打开调整前后对比图窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 显示调整前后对比图ToolStripMenuItem_Click(object sender, EventArgs e) {
            bool b = CheckForm(typeof(FrmShowBAChart).Name);
            if (!b) {
                if (AdjustPlanPP.Rows.Count == 0 && AdjustPlanElevation.Rows.Count == 0) {
                    MessageBox.Show("没有平面和高程数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (AdjustPlanPP.Rows.Count == 0) {
                    MessageBox.Show("没有平面数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AdjustPlanElevation.Rows.Count == 0) {
                    MessageBox.Show("没有高程数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                var fm = new FrmShowBAChart {
                    Tag = new TagObject() {
                        PPAdjustPlan = AdjustPlanPP,
                        ElevationAdjustPlan = AdjustPlanElevation,
                        Path = path,
                        Para = parameter
                    },
                    MdiParent = this
                };
                fm.Show();
            }
        }

        /// <summary>
        /// 打开型号数据窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void terViewModelData_Click(object sender, EventArgs e) {
            bool b = CheckForm(typeof(FrmViewModelData).Name);
            if (!b) {
                FrmViewModelData frm = new FrmViewModelData();
                frm.Tag = railFastening;
                frm.MdiParent = this;
                frm.Show();
            }
        }


        /// <summary>
        /// 导出调整方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subExportExcel_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(path)) {
                MessageBox.Show("请创建项目！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (AdjustPlanPP.Rows.Count == 0 && AdjustPlanElevation.Rows.Count == 0) {
                MessageBox.Show("没有数据！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (AdjustPlanPP.Rows.Count == 0) {
                MessageBox.Show("没有平面方案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (AdjustPlanElevation.Rows.Count == 0) {
                MessageBox.Show("没有高程方案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            DtAddExport(AdjustPlanPP, AdjustPlanElevation);
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Rick\Desktop\300m.txt")) {
                foreach (var item in CalcChord.calc_300m(export)) {
                    sw.WriteLine(item);
                }
            }
            bool b = CheckForm(typeof(FrmInfo).Name);
            if (!b) {
                FrmInfo fm = new FrmInfo(path, parameter, export, datumTrack, title, time, maker, overrunPP, overrunEle);
                fm.TransfEvent += fm_TransfEvent2;
                //fm.MdiParent = this;
                fm.ShowDialog();
            }
        }

        void fm_TransfEvent2(string _title, DateTime _time, string _maker) {
            title = _title;
            time = _time;
            maker = _maker;
        }
        /// <summary>
        /// 合并平面和高程调整方案
        /// </summary>
        /// <param name="dtPP"></param>
        /// <param name="dtElevation"></param>
        private void DtAddExport(DataTable dtPP, DataTable dtElevation) {
            export.Rows.Clear();
            int count = dtPP.Rows.Count >= dtElevation.Rows.Count ? dtPP.Rows.Count : dtElevation.Rows.Count;
            for (int i = 0; i < count; i++) {
                DataRow dr = export.NewRow();
                if (dtPP.Rows.Count != 0) {
                    dr["测量点"] = dtPP.Rows[i]["测量点"];
                    dr["里程/m"] = dtPP.Rows[i]["里程"];
                    dr["基准股"] = dtPP.Rows[i]["基准股"];
                    dr["轨枕编号"] = dtPP.Rows[i]["轨枕编号"];
                }
                else if (dtElevation.Rows.Count != 0) {
                    dr["测量点"] = dtElevation.Rows[i]["测量点"];
                    dr["里程/m"] = dtElevation.Rows[i]["里程"];
                    dr["基准股"] = dtElevation.Rows[i]["基准股"];
                    dr["轨枕编号"] = dtElevation.Rows[i]["轨枕编号"];
                }
                if (i < railFastening.Rows.Count) {
                    dr["CP3里程/m"] = railFastening.Rows[i]["CP3里程"];
                    dr["注解"] = railFastening.Rows[i]["CP3点号"];
                }
                if (dtPP.Rows.Count != 0) {
                    dr["调前平面/mm"] = dtPP.Rows[i]["调前平面"];
                    dr["调前轨距/mm"] = dtPP.Rows[i]["调前轨距"];
                    dr["调左平面/mm"] = dtPP.Rows[i]["调左平面"];
                    dr["调右平面/mm"] = dtPP.Rows[i]["调右平面"];
                    dr["调后平面/mm"] = dtPP.Rows[i]["调后平面"];
                    dr["调后轨距/mm"] = dtPP.Rows[i]["调后轨距"];
                }
                if (dtElevation.Rows.Count != 0) {
                    dr["调前高程/mm"] = dtElevation.Rows[i]["调前高程"];
                    dr["调前水平/mm"] = dtElevation.Rows[i]["调前水平"];
                    dr["调左高程/mm"] = dtElevation.Rows[i]["调左高程"];
                    dr["调右高程/mm"] = dtElevation.Rows[i]["调右高程"];
                    dr["调后高程/mm"] = dtElevation.Rows[i]["调后高程"];
                    dr["调后水平/mm"] = dtElevation.Rows[i]["调后水平"];
                }
                dr["设计超高/mm"] = averageValue.Rows[i]["设计超高"];
                export.Rows.Add(dr);
            }
        }

        private void 计算调轨材料ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (railFastening.Rows.Count == 0) {
                MessageBox.Show("没有轨道扣件数据，无法计算！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (AdjustPlanPP.Rows.Count == 0) {
                MessageBox.Show("没有平面调整数据！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (AdjustPlanElevation.Rows.Count == 0) {
                MessageBox.Show("没有高程调整数据！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bool b = CheckForm(typeof(FrmCalcMaterial).Name);
            if (!b) {
                FrmCalcMaterial fm = new FrmCalcMaterial {
                    Tag = new TagObject {
                        PPAdjustPlan = AdjustPlanPP,
                        ElevationAdjustPlan = AdjustPlanElevation,
                        Model = railFastening,
                        Path = path,
                        Para = parameter
                    },
                    MdiParent = this
                };
                fm.Show();
            }
        }


        private void 查看基准股ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (datumTrack == 0)
                MessageBox.Show("左");
            else if (datumTrack == 1)
                MessageBox.Show("右");
            else
                MessageBox.Show("未检测到基准股");
        }

        private void 选择文件ToolStripMenuItem_Click(object sender, EventArgs e) {
            string prjPath = string.Empty;
        tag: OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择项目文件";
            ofd.Multiselect = false;
            ofd.Filter = "*.prj|*.prj";
            if (ofd.ShowDialog() == DialogResult.OK) {
                prjPath = Path.GetDirectoryName(ofd.FileName) + @"\";
                prjName = Path.GetFileName(ofd.FileName);
                //using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("UTF-8")))
                //{
                //    string line;
                //    while (sr.Peek() > -1)
                //    {
                //        line = sr.ReadLine().Trim();
                //        if (!string.IsNullOrEmpty(line) && Regex.IsMatch(line, "项目位置="))
                //        {
                //            prjPath = line.Remove(0, 5);
                //        }
                //    }
                //}
                if (!Directory.Exists(prjPath + "原始文件") || !Directory.Exists(prjPath + "预处理文件") ||
                !Directory.Exists(prjPath + "调整文件") || !Directory.Exists(prjPath + "调整方案")) {
                    MessageBox.Show("请选择正确的文件夹！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto tag;
                }
                toolStripStatusLabel1.Text = $"当前项目:{prjPath}";
                ClearDt();
                CloseForm();
                path = prjPath;
            }
            else {
                return;
            }

            //动态添加三级菜单
            XmlNodeList xmlNodeList = XMLHelper.ReadXml();
            bool IsCreatMenu = true;
            if (xmlNodeList != null) {
                foreach (XmlElement item in xmlNodeList) {
                    if (item.InnerText == prjPath) {
                        IsCreatMenu = false;
                        break;
                    }
                }
            }
            if (IsCreatMenu == true) {
                ToolStripMenuItem terMenu = new ToolStripMenuItem {
                    Name = "ter" + Path.GetFileNameWithoutExtension(prjPath),
                    Text = prjPath
                };
                terMenu.Click += new EventHandler(terMenu_Click);
                ((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Add(terMenu);
            }
            //把文件写进xml
            XMLHelper.WriteXml(prjPath);

            //读取文件路径
            Array.Clear(pathFile, 0, pathFile.Length);
            DirectoryInfo root = new DirectoryInfo(path + "原始文件");
            foreach (FileInfo path in root.GetFiles()) {
                //if (Regex.IsMatch(path.FullName, ".OUT")) {
                //    pathFile[0] = path.FullName;
                //    continue;
                //}
                //if (Regex.IsMatch(path.FullName, ".fas")) {
                //    pathFile[1] = path.FullName;
                //    continue;
                //}
                if (path.FullName.EndsWith(".OUT")) {
                    pathFile[0] = path.FullName;
                    continue;
                }
                if (path.FullName.EndsWith(".fas")) {
                    pathFile[1] = path.FullName;
                    continue;
                }
            }
            if (string.IsNullOrEmpty(pathFile[0])) {
                if (MessageBox.Show("未检测到OUT文件，是否手动选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    OpenFileDialog fileDialog = new OpenFileDialog {
                        Multiselect = false,
                        InitialDirectory = path + "原始文件",
                        Title = "请手动选择OUT文件",
                        Filter = "(OUT)|*.OUT|所有文件(*.*)|*.*"
                    };
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        pathFile[0] = fileDialog.FileName;
                    }
                }
            }
            if (string.IsNullOrEmpty(pathFile[1])) {
                if (MessageBox.Show("未检测到轨道扣件文件，是否手动选择？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    OpenFileDialog fileDialog = new OpenFileDialog {
                        Multiselect = false,
                        InitialDirectory = path + "原始文件",
                        Title = "请手动选择轨道扣件文件",
                        Filter = "(fas)|*.fas|所有文件(*.*)|*.*"
                    };
                    if (fileDialog.ShowDialog() == DialogResult.OK) {
                        pathFile[1] = fileDialog.FileName;
                    }
                }
            }
            splashScreenManager1.ShowWaitForm();
            try {
                //导入往返测文件
                string report = string.Empty;
                if (File.Exists(pathFile[0])) {
                    //string content = File.ReadAllText(pathFile[0], encoding);
                    //File.WriteAllText(pathFile[0], content, encoding);
                    bool boo = IOHelper.OutDataImportToDataTable(importW, importF, pathFile[0]);
                    if (boo) {
                        if (Path.GetDirectoryName(pathFile[0]) != (path + "原始文件")) {
                            //删除同名文件
                            DirectoryInfo info = new DirectoryInfo(path + "原始文件");
                            foreach (FileInfo path in info.GetFiles()) {
                                if (Regex.IsMatch(path.FullName, ".OUT")) {
                                    File.Delete(path.FullName);
                                }
                            }
                            File.Copy(pathFile[0], path + @"原始文件\" + pathFile[0].Substring(pathFile[0].LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                        }
                        if (importW.Rows[0]["导向轨"].ToString() == "左轨" ||
                            importW.Rows[0]["导向轨"].ToString() == "左" || importW.Rows[0]["导向轨"].ToString() == "左股")
                            datumTrack = 0;
                        if (importW.Rows[0]["导向轨"].ToString() == "右轨" ||
                            importW.Rows[0]["导向轨"].ToString() == "右" || importW.Rows[0]["导向轨"].ToString() == "右股")
                            datumTrack = 1;
                        averageValue.Rows.Clear();
                        CalcAve();
                        IOHelper.DataTableExportToTxt(averageValue, path + @"预处理文件\往返测平均值.txt");
                    }
                }
                else {
                    report += "未找到OUT文件\n";
                }
                //导入轨道扣件文件
                if (File.Exists(pathFile[1])) {
                    //string content = File.ReadAllText(pathFile[1], encoding);
                    //File.WriteAllText(pathFile[1], content, encoding);
                    bool boo = IOHelper.TxtToDataTable(railFastening, pathFile[1]);
                    if (boo) {
                        if (Path.GetDirectoryName(pathFile[1]) != (path + "原始文件")) {
                            DirectoryInfo info2 = new DirectoryInfo(path + "原始文件");
                            foreach (FileInfo path in info2.GetFiles()) {
                                if (Regex.IsMatch(path.FullName, "扣件")) {
                                    File.Delete(path.FullName);
                                }
                            }
                            File.Copy(pathFile[1], path + "原始文件\\" + pathFile[1].Substring(pathFile[1].LastIndexOf("\\") + 1), true);//去掉了zd路径, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                        }
                    }
                }
                else {
                    report += "未找到轨道扣件文件\n";
                }
                ////导入往返测平均值文件
                //if (File.Exists(path + "预处理文件\\往返测平均值.txt")) {
                //    IOHelper.AllTxtToDataTable(averageValue, path + "预处理文件\\往返测平均值.txt");
                //}
                //else {
                //    report += "未找到往返测平均值文件\n";
                //}
                //导入平面基准股可调量
                if (File.Exists(path + "调整文件\\平面基准股可调量.txt")) {
                    IOHelper.AllTxtToDataTable(basicTrackAdjustablePP, path + "调整文件\\平面基准股可调量.txt");
                }
                else {
                    report += "未找到平面基准股可调量文件\n";
                }
                //导入平面模拟调整量
                if (File.Exists(path + "调整文件\\平面模拟调整量.txt")) {
                    IOHelper.AllTxtToDataTable(simulationAdjustmentPP, path + "调整文件\\平面模拟调整量.txt");
                }
                else {
                    report += "未找到平面模拟调整量文件\n";
                }
                //导入平面模拟调整量取整
                if (File.Exists(path + "调整文件\\平面模拟调整量取整.txt")) {
                    IOHelper.AllTxtToDataTable(actualAdjustmentPP, path + "调整文件\\平面模拟调整量取整.txt");
                }
                else {
                    report += "未找到平面模拟调整量取整文件\n";
                }
                //导入平面调整方案
                if (File.Exists(path + "调整文件\\平面调整方案.txt")) {
                    IOHelper.AllTxtToDataTable(AdjustPlanPP, path + "调整文件\\平面调整方案.txt");
                }
                else {
                    report += "未找到平面调整方案文件\n";
                }
                //导入平面调整中线
                if (File.Exists(path + "调整文件\\平面调整中线.txt")) {
                    IOHelper.AllTxtToDataTable(tendLinePP, path + "调整文件\\平面调整中线.txt");
                }
                else {
                    report += "未找到平面调整中线文件\n";
                }
                //导入平面调整中线斜率
                if (File.Exists(path + "调整文件\\平面调整中线斜率.txt")) {
                    IOHelper.AllTxtToDataTable(slopePP, path + "调整文件\\平面调整中线斜率.txt");
                }
                else {
                    report += "未找到平面调整中线斜率文件\n";
                }
                //导入高程基准股可调量
                if (File.Exists(path + "调整文件\\高程基准股可调量.txt")) {
                    IOHelper.AllTxtToDataTable(basicTrackAdjustableElevation, path + "调整文件\\高程基准股可调量.txt");
                }
                else {
                    report += "未找到高程基准股可调量文件\n";
                }
                //导入高程模拟调整量
                if (File.Exists(path + "调整文件\\高程模拟调整量.txt")) {
                    IOHelper.AllTxtToDataTable(simulationAdjustmentElevation, path + "调整文件\\高程模拟调整量.txt");
                }
                else {
                    report += "未找到高程模拟调整量文件\n";
                }
                //导入高程模拟调整量取整
                if (File.Exists(path + "调整文件\\高程模拟调整量取整.txt")) {
                    IOHelper.AllTxtToDataTable(actualAdjustmentElevation, path + "调整文件\\高程模拟调整量取整.txt");
                }
                else {
                    report += "未找到高程模拟调整量取整文件\n";
                }
                //导入高程调整方案
                if (File.Exists(path + "调整文件\\高程调整方案.txt")) {
                    IOHelper.AllTxtToDataTable(AdjustPlanElevation, path + "调整文件\\高程调整方案.txt");
                }
                else {
                    report += "未找到高程调整方案文件\n";
                }
                //导入高程调整中线
                if (File.Exists(path + "调整文件\\高程调整中线.txt")) {
                    IOHelper.AllTxtToDataTable(tendLineElevation, path + "调整文件\\高程调整中线.txt");
                }
                else {
                    report += "未找到高程调整中线文件\n";
                }
                //导入高程调整中线斜率
                if (File.Exists(path + "调整文件\\高程调整中线斜率.txt")) {
                    IOHelper.AllTxtToDataTable(slopeElevation, path + "调整文件\\高程调整中线斜率.txt");
                }
                else {
                    report += "未找到高程调整中线斜率文件\n";
                }
                //导入参数
                if (File.Exists(path + "预处理文件\\参数文件.txt")) {
                    //string content = File.ReadAllText(path + "预处理文件\\参数文件.txt", Encoding.Default);
                    //File.WriteAllText(path + "预处理文件\\参数文件.txt", content, Encoding.UTF8);
                    IOHelper.ParaTxtToDataTable(parameter, path + "预处理文件\\参数文件.txt");
                }
                else {
                    report += "未找到参数文件\n";
                }
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show("打开成功！\n" + report, "打开提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show("发生异常：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e) {
            File.Delete(Application.StartupPath + "//RecentProject.xml");
            while (((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Count > 2) {
                ((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.RemoveAt(((ToolStripDropDownItem)((ToolStripDropDownItem)MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Count - 1);
            }
        }
        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\软件操作手册.CHM");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e) {
            FrmAboutBox about = new FrmAboutBox();
            about.Show();
        }
    }
}
