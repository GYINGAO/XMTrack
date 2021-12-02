using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace TrackRedesign {
    public class XMLHelper {
        public static readonly int projectNum = int.Parse(ConfigurationManager.AppSettings["projectNum"].Trim());
        public static void SaveConfig(string ConnenctionString, string strKey) {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径  
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            // string  strFileName= AppDomain.CurrentDomain.BaseDirectory + "\\exe.config";  
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++) {
                //获得将当前元素的key属性  
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素  
                if (att.Value == strKey) {
                    //对目标元素中的第二个属性赋值  
                    att = nodes[i].Attributes["value"];
                    att.Value = ConnenctionString;
                    break;
                }
            }
            //保存上面的修改  
            doc.Save(strFileName);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 将文件路劲添加到xml中
        /// </summary>
        /// <param name="fileName"></param>
        public static void WriteXml(string fileName) {
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = Application.StartupPath;
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElement;
            if (!File.Exists(path + "\\RecentProject.xml")) {
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                xmlDocument.AppendChild(xmlDeclaration);
                //根节点
                xmlElement = xmlDocument.CreateElement("OpenProjects");
                xmlDocument.AppendChild(xmlElement);
            }
            else {
                xmlDocument.Load(path + "\\RecentProject.xml");
                xmlElement = (XmlElement)xmlDocument.SelectSingleNode("OpenProjects");
                XmlNodeList xmlNodeList = xmlElement.ChildNodes;
                foreach (XmlElement item in xmlNodeList) {
                    if (item.InnerText == fileName) {
                        return;
                    }
                }
                int count = xmlNodeList.Count;
                if (count > projectNum - 1) {
                    int delCount = count - projectNum + 1;
                    for (int i = 0; i < delCount; i++) {
                        xmlElement.RemoveChild(xmlElement.SelectSingleNode("Project"));
                    }
                }
            }
            //添加子节点
            XmlElement xmlElement1 = xmlDocument.CreateElement("Project");
            xmlElement1.SetAttribute("name", Path.GetFileNameWithoutExtension(fileName));
            xmlElement1.InnerText = fileName;
            xmlElement.AppendChild(xmlElement1);
            xmlDocument.Save(path + "\\RecentProject.xml");
        }

        /// <summary>
        /// 返回指定xml的子节点列表
        /// </summary>
        /// <returns></returns>
        public static XmlNodeList ReadXml() {
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = Application.StartupPath;
            XmlNodeList xmlNodeList = null;
            if (File.Exists(path + "\\RecentProject.xml")) {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path + "\\RecentProject.xml");
                //获取根节点
                XmlNode xmlNode = xmlDocument.SelectSingleNode("OpenProjects");
                xmlNodeList = xmlNode.ChildNodes;
            }
            return xmlNodeList;
        }

        /// <summary>
        /// 移除指定的子节点
        /// </summary>
        /// <param name="str"></param>
        public static void RemoveElement(string str) {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(Application.StartupPath + "//RecentProject.xml"))
                return;
            doc.Load(Application.StartupPath + "//RecentProject.xml");
            XmlElement xmlElement = doc.DocumentElement;
            XmlNodeList list = xmlElement.ChildNodes;
            foreach (XmlElement item in list) {
                if (item.InnerText==str) {
                    xmlElement.RemoveChild(item);
                    break;
                }
            }
            doc.Save(Application.StartupPath + "//RecentProject.xml");
        }
    }
}
