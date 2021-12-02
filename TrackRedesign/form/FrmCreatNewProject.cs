using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TrackRedesign {
    public partial class FrmCreatNewProject : Form {
        private FrmMain fm;
        public FrmCreatNewProject() {
            InitializeComponent();
        }
        public FrmCreatNewProject(FrmMain _fm) {
            InitializeComponent();
            fm = _fm;
        }
        public delegate void TransfDelegate(String value);
        public event TransfDelegate TransfEvent;
        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择工程项目路径";
            if (dialog.ShowDialog() == DialogResult.OK) {
                txtProjectLocation.Text = dialog.SelectedPath + @"\";
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtProjectLocation.Text)) {
                MessageBox.Show("请选择路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtProjectName.Text)) {
                MessageBox.Show("请输入项目名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string prjMessage = string.Empty;
            string str = txtProjectLocation.Text + txtProjectName.Text + @"\";
            fm.prjName = txtProjectName.Text + ".prj";
            try {
                if (Directory.Exists(str) == false) {
                    fm.CloseForm();
                    fm.ClearDt();
                    //创建母文件夹
                    Directory.CreateDirectory(str);
                    Directory.CreateDirectory(str + "原始文件");
                    Directory.CreateDirectory(str + "预处理文件");
                    Directory.CreateDirectory(str + "调整文件");
                    Directory.CreateDirectory(str + "调整方案");
                    MessageBox.Show("创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //fm.barStaticItem3.Caption = $"当前项目：{str}";
                    fm.toolStripStatusLabel1.Text = $"当前项目:{txtProjectLocation.Text + txtProjectName.Text}";
                    TransfEvent(str);
                    prjMessage += $"项目名称={txtProjectName.Text}\r项目位置={str}\r\r";
                    IOHelper.WriteStrToTxt(prjMessage, str + txtProjectName.Text + ".prj");
                    Close();
                }
                else
                    if (MessageBox.Show("项目已存在，是否覆盖?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) {
                    DirectoryInfo di = new DirectoryInfo(str);
                    di.Delete(true);
                    fm.CloseForm();
                    fm.ClearDt();
                    //创建母文件夹
                    Directory.CreateDirectory(str);
                    Directory.CreateDirectory(str + "原始文件");
                    Directory.CreateDirectory(str + "预处理文件");
                    Directory.CreateDirectory(str + "调整文件");
                    Directory.CreateDirectory(str + "调整方案");
                    MessageBox.Show("创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //fm.barStaticItem3.Caption = $"当前项目：{str}";
                    fm.toolStripStatusLabel1.Text = $"当前项目:{txtProjectLocation.Text + txtProjectName.Text}";
                    TransfEvent(str);
                    prjMessage += $"项目名称={txtProjectName.Text}\r项目位置={str}\r\r";
                    IOHelper.WriteStrToTxt(prjMessage, str + txtProjectName.Text + ".prj");
                    Close();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("创建失败：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //动态添加三级菜单
            XmlNodeList xmlNodeList = XMLHelper.ReadXml();
            bool IsCreatMenu = true;
            if (xmlNodeList != null)
            {
                foreach (XmlElement item in xmlNodeList)
                {
                    if (item.InnerText == str)
                    {
                        IsCreatMenu = false;
                        break;
                    }
                }
            }
            if (IsCreatMenu == true)
            {
                ToolStripMenuItem terMenu = new ToolStripMenuItem
                {
                    Name = "ter" + Path.GetFileNameWithoutExtension(str),
                    Text = str
                };
                terMenu.Click += new EventHandler(fm.terMenu_Click);
                ((ToolStripDropDownItem)((ToolStripDropDownItem)fm.MnuMain.Items["miProjectMann"]).DropDownItems["subOpenProject"]).DropDownItems.Add(terMenu);
            }
            //把文件写进xml
            XMLHelper.WriteXml(str);
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
