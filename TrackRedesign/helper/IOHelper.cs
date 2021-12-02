using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace TrackRedesign {
    public class IOHelper {
        static readonly string remark = "面向里程增长方向。拨道方向:+表示向右拨,-表示向左拨.起道方向:+表示起道,-表示降道.水平：+表示右轨高于左轨";
        //static readonly Encoding encoding = Encoding.GetEncoding("GB2312");
        static readonly Encoding encoding = Encoding.Default;
        /// <summary>
        /// 判断字符串第一个字符是否为数字
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsNumberic(string message) {
            Regex rex = new Regex("^[0-9]");
            if (rex.IsMatch(message))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断第一个是否为字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetter(string str) {
            Regex rex = new Regex(@"^[A-Za-z]");
            if (rex.IsMatch(str))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 判断为字母或数字
        /// </summary>
        public static bool isNumOrAlp(string str) {
            string pattern = @"^[A-Za-z0-9]";  //@意思忽略转义，+匹配前面一次或多次，$匹配结尾
            Match match = Regex.Match(str, pattern);
            return match.Success;
        }

        /// <summary>
        /// 判断字符串是否能转换为浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str) {
            //return Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$");
            return float.TryParse(str, out _);
        }
        /// <summary>
        /// 判断字符串是否能转换为int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str) {
            return Regex.IsMatch(str, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 判断字符串是否能转换为int或double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIntOrDouble(string str) {
            return Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$") || Regex.IsMatch(str, @"^[+-]?\d*$");
        }
        /// <summary>
        /// 将往返测文件导入datatable
        /// </summary>
        /// <param name="toMeasure"></param>
        /// <param name="froMeasure"></param>
        /// <param name="filePath"></param>
        public static bool OutDataImportToDataTable(DataTable toMeasure, DataTable froMeasure, string filePath) {
            toMeasure.Rows.Clear();
            froMeasure.Rows.Clear();
            Encoding enco = TxtFileEncoding.GetEncoding(filePath, encoding);
            using StreamReader readFile = new StreamReader(filePath, enco);
            string dataLineTo;
            string[] dataArray;
            try {
                while (readFile.Peek() > -1) {
                    dataLineTo = readFile.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(dataLineTo) && IsNumberic(dataLineTo)) {
                        // 按行读取结果文件
                        dataArray = Regex.Split(dataLineTo, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                        AddDataToTable(dataArray, toMeasure);
                    }
                }
                SplitDt(toMeasure, froMeasure);
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("文件格式不正确，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                readFile.Close();
            }
        }
        /// <summary>
        /// 将往返测字符串数组导入datarow
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="resultDataTable"></param>
        private static void AddOutDataToTable(string[] dataArray, DataTable resultDataTable) {
            DataRow dr = resultDataTable.NewRow();
            int count = dataArray.Length;
            for (int i = 0; i < count; i++) {
                if (i == 1) {
                    dr[i] = Math.Round((decimal)double.Parse(dataArray[i].Trim()), 1, MidpointRounding.AwayFromZero);
                }
                else if (i == 5)
                    dr[i] = dataArray[i].Trim();
                else
                    dr[i] = double.Parse(dataArray[i].Trim());
            }
            resultDataTable.Rows.Add(dr);
        }
        /// <summary>
        /// 将一个datatable拆成两个
        /// </summary>
        public static void SplitDt(DataTable dtSource, DataTable dtTarget) {
            dtTarget.Rows.Clear();
            int count = dtSource.Rows.Count;
            for (int i = count / 2; i < count; i++) {
                DataRow dr = dtTarget.NewRow();
                dr.ItemArray = dtSource.Rows[i].ItemArray;
                dtTarget.Rows.Add(dr);
                //dtTarget.ImportRow(dtSource.Rows[i]);
            }
            for (int j = count - 1; j >= count / 2; j--) {
                dtSource.Rows.RemoveAt(j);
            }
        }
        /// <summary>
        /// 将轨道扣件文件导入datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static bool TxtToDataTable(DataTable dt, string filePath) {
            dt.Rows.Clear();
            try {
                Encoding enco = TxtFileEncoding.GetEncoding(filePath, encoding);
                using (StreamReader readFile = new StreamReader(filePath, enco)) {
                    string dataLine;
                    string[] dataArray;
                    while (readFile.Peek() > -1) {
                        dataLine = readFile.ReadLine().Trim();
                        if (!string.IsNullOrEmpty(dataLine) && IsNumberic(dataLine)) {
                            // 按行读取结果文件
                            dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                            AddDataToTable(dataArray, dt);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("文件格式不正确，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 将可调量字符串数组导入datarow
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="dt"></param>
        private static void AddTxtDataToTable(string[] dataArray, DataTable dt) {
            DataRow dr = dt.NewRow();
            int count = dataArray.Length;
            for (int i = 0; i < count; i++) {
                if (i == 0)
                    dr[i] = dataArray[i].Trim();
                else if (i == 1)
                    dr[i] = Math.Round((decimal)double.Parse(dataArray[i].Trim()), 1, MidpointRounding.AwayFromZero);
                else
                    dr[i] = int.Parse(dataArray[i].Trim());
            }
            dt.Rows.Add(dr);
        }
        /// <summary>
        /// 将型号字符串数组导入datarow
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="dt"></param>
        private static void AddModelDataToTable(string[] dataArray, DataTable dt) {
            DataRow dr = dt.NewRow();
            int count = dataArray.Length;
            for (int i = 0; i < count; i++) {
                if (i == 1)
                    dr[i] = dataArray[i].Trim();
                else if (i == 2)
                    dr[i] = Math.Round((decimal)double.Parse(dataArray[i].Trim()), 1, MidpointRounding.AwayFromZero);
                else
                    dr[i] = double.Parse(dataArray[i].Trim());
            }
            dt.Rows.Add(dr);
        }
        /// <summary>
        /// 将普通文件导入datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static bool AllTxtToDataTable(DataTable dt, string filePath) {
            dt.Rows.Clear();
            Encoding enco = TxtFileEncoding.GetEncoding(filePath, encoding);
            using StreamReader readFile = new StreamReader(filePath, enco);
            string dataLine;
            string[] dataArray;
            try {
                while (readFile.Peek() > -1) {
                    dataLine = readFile.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(dataLine) && isNumOrAlp(dataLine)) {
                        // 按行读取结果文件
                        dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                        AddDataToTable(dataArray, dt);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                readFile.Close();
            }
        }
        /// <summary>
        /// 将参数文件导入datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void ParaTxtToDataTable(DataTable dt, string filePath) {
            dt.Rows.Clear();
            Encoding enco = TxtFileEncoding.GetEncoding(filePath, encoding);
            using (StreamReader readFile = new StreamReader(filePath, enco)) {
                string dataLine;
                string[] dataArray;
                try {
                    int i = 0;
                    while (readFile.Peek() > -1) {
                        if (i >= 1) {
                            dataLine = readFile.ReadLine().Trim();
                            if (!string.IsNullOrEmpty(dataLine)) {
                                // 按行读取结果文件
                                dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                                AddDataToTable(dataArray, dt);
                            }
                        }
                        else {
                            readFile.ReadLine();
                            i++;
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("发生错误" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    readFile.Close();
                }
            }
        }
        /// <summary>
        /// 将字符串数组导入datarow
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="dt"></param>
        private static void AddDataToTable(string[] dataArray, DataTable dt) {
            DataRow dr = dt.NewRow();
            int count = dataArray.Length;
            for (int i = 0; i < count; i++) {
                if (!string.IsNullOrEmpty(dataArray[i].Trim())) {
                    dr[i] = dataArray[i].Trim();
                }
            }
            dt.Rows.Add(dr);
        }
        /// <summary>
        /// 将检查结果写入txt
        /// </summary>
        /// <param name="str"></param>
        /// <param name="file"></param>
        public static void WriteStrToTxt(string str, string file) {
            // 如果存在要保存的文件,则删除
            if (File.Exists(file)) {
                File.Delete(file);
            }
            FileStream fileStream = new FileStream(file, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream, encoding);
            streamWriter.Write(str + "\r\n");
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        /// <summary>
        /// 将DataTable导出为文本文件
        /// </summary>
        public static void DataTableExportToTxt(DataTable dt, string fileName) {
            StringBuilder sb = new StringBuilder();
            //StreamWriter streamWrite = new StreamWriter(Encoding.GetEncoding("GB2312"));
            // 如果存在要保存的文件,则删除
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            // 创建文件
            //FileInfo file = new FileInfo(fileName);
            //streamWrite = file.CreateText();
            StreamWriter streamWrite = new StreamWriter(fileName, false, Encoding.Default);
            try {
                if (dt.Rows.Count > 0) {
                    // 写数据
                    string dataLine = null;
                    string value = null;
                    //写入表名
                    foreach (DataColumn col in dt.Columns) {
                        value = col.ColumnName.ToString().Trim();
                        //sb.Append(String.Format("{0,-10}", value));
                        sb.Append(value + "\t");
                    }
                    dataLine = sb.ToString();
                    streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                    sb.Remove(0, sb.Length);
                    //写入数据
                    int columnCount = dt.Columns.Count;
                    int n = 0;
                    foreach (DataRow dr in dt.Rows) {
                        n++;
                        for (int j = 0; j < columnCount; j++) {
                            value = dr[j].ToString().Trim();
                            if (string.IsNullOrEmpty(value)) {
                                value = " ";
                            }
                            //if (j==0) {
                            //    value = int.Parse(value).ToString();
                            //}
                            //if (IsInt(value) == true) {
                            //    value = int.Parse(value).ToString("0");
                            //}
                            else if (IsIntOrDouble(value) == true) {
                                value = double.Parse(value).ToString("f1");
                            }
                            //sb.Append(String.Format("{0,-20}", value));
                            sb.Append(value + "\t ");
                        }
                        dataLine = sb.ToString();
                        // 按行写入数据
                        streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                        sb.Remove(0, sb.Length);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                streamWrite.Close();
            }
        }

        /// <summary>
        /// 将斜率导出为文本文件
        /// </summary>
        public static bool Slope2Txt(DataTable dt, string fileName) {
            StringBuilder sb = new StringBuilder();
            //StreamWriter streamWrite = null;

            // 如果存在要保存的文件,则删除
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            // 创建文件
            //FileInfo file = new FileInfo(fileName);
            //streamWrite = file.CreateText();
            StreamWriter streamWrite = new StreamWriter(fileName, false, encoding);
            try {
                if (dt.Rows.Count > 0) {
                    // 写数据
                    string dataLine = null;
                    string value = null;
                    //写入表名
                    foreach (DataColumn col in dt.Columns) {
                        value = col.ColumnName.ToString().Trim();
                        //sb.Append(String.Format("{0,-10}", value));
                        sb.Append(value + "\t");
                    }
                    dataLine = sb.ToString();
                    streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                    sb.Remove(0, sb.Length);
                    //写入数据
                    int columnCount = dt.Columns.Count;
                    int n = 0;
                    foreach (DataRow dr in dt.Rows) {
                        n++;
                        for (int j = 0; j < columnCount; j++) {
                            if (string.IsNullOrEmpty(dr[j].ToString().Trim())) {
                                value = " ";
                            }
                            if (j == 2) {
                                string input = Convert.ToDouble(dr[j].ToString()).ToString("f5");
                                decimal num = Convert.ToDecimal(input);
                                if (num == 0) {
                                    value = 0.ToString();
                                }
                                else {
                                    Fractional fractional = new Fractional(num);
                                    int c = 1000;
                                    decimal fz = fractional.Member;
                                    decimal fm = fractional.Denominator;
                                    decimal k = fm / c;
                                    fz = decimal.Truncate(fz / k);
                                    fm = c;
                                    value = string.Format("{0}/{1}", fz, fm);
                                }
                            }
                            else {
                                value = dr[j].ToString().Trim();
                                if (IsIntOrDouble(value) == true) {
                                    value = double.Parse(value).ToString("f1");
                                }
                            }
                            //sb.Append(String.Format("{0,-20}", value));
                            sb.Append(value + "\t ");
                        }
                        dataLine = sb.ToString();
                        // 按行写入数据
                        streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                        sb.Remove(0, sb.Length);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                streamWrite.Close();
            }
        }


        /// <summary>
        /// 导出调整方案
        /// </summary>
        public static bool ExportAdj(DataTable dt, string fileName) {
            StringBuilder sb = new StringBuilder();
            //StreamWriter streamWrite = null;
            // 如果存在要保存的文件,则删除
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            // 创建文件
            //FileInfo file = new FileInfo(fileName);
            //streamWrite = file.CreateText();
            StreamWriter streamWrite = new StreamWriter(fileName, false, encoding);
            try {
                if (dt.Rows.Count > 0) {
                    // 写数据
                    string dataLine = null;
                    string value = null;
                    //写入表名
                    foreach (DataColumn col in dt.Columns) {
                        value = col.ColumnName.ToString().Trim();
                        //sb.Append(String.Format("{0,-10}", value));
                        sb.Append(value + "\t");
                    }
                    sb.Append("备注\t");
                    dataLine = sb.ToString();
                    streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                    sb.Remove(0, sb.Length);
                    //写入数据
                    int columnCount = dt.Columns.Count;
                    int n = 0;
                    int count = 0;
                    foreach (DataRow dr in dt.Rows) {
                        n++;
                        for (int j = 0; j < columnCount; j++) {
                            value = dr[j].ToString().Trim();
                            if (string.IsNullOrEmpty(value)) {
                                value = " ";
                            }
                            else if (j == 0) {
                                value = int.Parse(value).ToString();
                            }
                            //if (IsInt(value) == true) {
                            //    value = int.Parse(value).ToString("0");
                            //}
                            else if (IsIntOrDouble(value) == true) {
                                value = double.Parse(value).ToString("f1");
                            }
                            //sb.Append(String.Format("{0,-20}", value));
                            sb.Append(value + "\t ");
                        }
                        if (count < remark.Length) {
                            sb.Append(remark[count] + "\t");
                            count++;
                        }
                        dataLine = sb.ToString();
                        // 按行写入数据
                        streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                        sb.Remove(0, sb.Length);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                streamWrite.Close();
            }
        }
        /// <summary>
        /// 导出平面或高程调整方案
        /// </summary>
        public static bool Export2Adj(DataTable dt, string fileName) {
            StringBuilder sb = new StringBuilder();
            //StreamWriter streamWrite = null;
            // 如果存在要保存的文件,则删除
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            // 创建文件
            //FileInfo file = new FileInfo(fileName);
            //streamWrite = file.CreateText();
            StreamWriter streamWrite = new StreamWriter(fileName, false, encoding);
            try {
                if (dt.Rows.Count > 0) {
                    // 写数据
                    string dataLine = null;
                    string value = null;
                    //写入表名
                    foreach (DataColumn col in dt.Columns) {
                        value = col.ColumnName.ToString().Trim();
                        //sb.Append(String.Format("{0,-10}", value));
                        sb.Append(value + "\t");
                    }
                    dataLine = sb.ToString();
                    streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                    sb.Remove(0, sb.Length);
                    //写入数据
                    int columnCount = dt.Columns.Count;
                    int n = 0;
                    int count = 0;
                    foreach (DataRow dr in dt.Rows) {
                        n++;
                        for (int j = 0; j < columnCount; j++) {
                            value = dr[j].ToString().Trim();
                            if (string.IsNullOrEmpty(value)) {
                                value = " ";
                            }
                            else if (j == 0) {
                                value = int.Parse(value).ToString();
                            }
                            //if (IsInt(value) == true) {
                            //    value = int.Parse(value).ToString("0");
                            //}
                            else if (IsIntOrDouble(value) == true) {
                                value = double.Parse(value).ToString("f1");
                            }
                            //sb.Append(String.Format("{0,-20}", value));
                            sb.Append(value + "\t ");
                        }
                        dataLine = sb.ToString();
                        // 按行写入数据
                        streamWrite.WriteLine(dataLine.Substring(0, dataLine.Length - 1));
                        sb.Remove(0, sb.Length);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally {
                streamWrite.Close();
            }
        }
        /// <summary>
        /// 将DataTable导出为Excel文件
        /// </summary>
        public static bool DataTableExportToExcel(DataTable table, string file) {
            // 如果存在要保存的文件,则删除
            if (File.Exists(file)) {
                File.Delete(file);
            }
            try {
                string title = "";
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
                //FileStream fs1 = File.Open(file, FileMode.Open, FileAccess.Read);
                StreamWriter sw = new StreamWriter(new BufferedStream(fs), encoding);
                for (int i = 0; i < table.Columns.Count; i++) {
                    title += table.Columns[i].ColumnName + "\t"; //栏位：自动跳到下一单元格
                }
                title += "备注\t";
                title = title.Substring(0, title.Length - 1) + "\n";
                sw.Write(title);
                int count = 0;
                foreach (DataRow row in table.Rows) {
                    string line = "";
                    for (int i = 0; i < table.Columns.Count; i++) {
                        line += row[i].ToString().Trim() + "\t"; //内容：自动跳到下一单元格
                    }
                    if (count < remark.Length) {
                        line += remark[count] + "\t";
                        count++;
                    }
                    line = line.Substring(0, line.Length - 1) + "\n";
                    sw.Write(line);
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 指定一个路径和后缀名，查找这个路径下以此后缀名结尾的文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetFileListWithExtend(DirectoryInfo directory, string pattern) {
            string result = String.Empty;
            if (directory.Exists || pattern.Trim() != string.Empty) {

                foreach (FileInfo info in directory.GetFiles(pattern)) {
                    result = info.FullName.ToString();
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 读取txt中的文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> GetFilePath(string filePath) {
            List<string> path = new List<string>();
            string rssuli = string.Empty;
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader readFile = new StreamReader(filePath, encoding)) {
                string dataLine;
                try {
                    while (readFile.Peek() > -1) {
                        dataLine = readFile.ReadLine().Trim();
                        // 按行读取结果文件
                        if (!string.IsNullOrEmpty(dataLine)) {
                            path.Add(dataLine);
                        }
                    }
                }
                catch (IOException ioEx) {
                    throw ioEx;
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
            return path;
        }


        /// <summary>
        /// datagridview导出文本文件
        /// </summary>
        /// <param name="gridview"></param>
        /// <param name="fileName"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public static bool DataGridViewToTxt(DataGridView gridview, string fileName, char strSplit) {
            if (gridview == null || gridview.Rows.Count == 0)
                return false;
            string filename = fileName;
            FileStream fileStream = new FileStream(filename, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream, encoding);
            StringBuilder strBuilder = new StringBuilder();
            try {
                strBuilder = new StringBuilder();
                for (int i = 0; i < gridview.Columns.Count; i++) {
                    strBuilder.Append(gridview.Columns[i].HeaderText + strSplit);
                }
                strBuilder.Remove(strBuilder.Length - 1, 1); // 将最后添加的一个strSplit删除掉
                streamWriter.WriteLine(strBuilder.ToString());
                for (int i = 0; i < gridview.Rows.Count; i++) {
                    strBuilder = new StringBuilder();
                    for (int j = 0; j < gridview.Columns.Count; j++) {
                        strBuilder.Append(gridview.Rows[i].Cells[j].Value.ToString() + strSplit);
                    }
                    strBuilder.Remove(strBuilder.Length - 1, 1); // 将最后添加的一个strSplit删除掉
                    streamWriter.WriteLine(strBuilder.ToString());
                }
            }
            catch (Exception ex) {
                string strErrorMessage = ex.Message;
                return false;
            }
            finally {
                streamWriter.Close();
                fileStream.Close();
            }
            return true;
        }

        /// <summary>
        /// 打开文件选择窗口
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string OpenFileDia(string title) {
            OpenFileDialog fileDialog = new OpenFileDialog {
                Multiselect = false,
                Title = title,
                Filter = "(*txt)|*.txt|(*.*)|*.*"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                return fileDialog.FileName;
            }
            else
                return null;
        }

        /// <summary>
        /// 将调整中线文件导入表格
        /// </summary>
        /// <param name="file"></param>
        public static void ImportAdjLine(string file, DataGridView dgv) {
            using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader readFile = new StreamReader(file, encoding);
            string dataLine;
            string[] dataArray;
            try {
                dgv.Rows.Clear();
                while (readFile.Peek() > -1) {
                    bool tag = true;
                    dataLine = readFile.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(dataLine) && IsNumberic(dataLine)) {
                        // 按行读取结果文件
                        dataArray = Regex.Split(dataLine, "[\\s]+|[,]+|[，]+", RegexOptions.IgnoreCase);
                        for (int i = 0; i < dgv.Rows.Count; i++) {
                            if (Math.Round(double.Parse(dgv.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(dataArray[0]), 1)) {
                                tag = false;
                                break;
                            }
                        }
                        if (tag == true) {
                            int index = dgv.Rows.Add();
                            dgv.Rows[index].Cells[0].Value = double.Parse(dataArray[0]);
                            dgv.Rows[index].Cells[1].Value = double.Parse(dataArray[1]);
                        }
                    }
                }
            }
            catch (Exception) {
                MessageBox.Show("文件格式有误，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                fs.Close();
                readFile.Close();
            }
        }

        public static void AddStrLine2Dgv(string str, DataGridView dgv) {
            string[] lines = Regex.Split(str, "\r\n");
            foreach (string line in lines) {
                bool tag = true;
                string[] data;
                if (!string.IsNullOrEmpty(line)) {
                    try {
                        data = Regex.Split(line, "[\\s]+|[,]|[，]", RegexOptions.IgnoreCase);
                        for (int i = 0; i < dgv.Rows.Count; i++) {
                            if (Math.Round(double.Parse(dgv.Rows[i].Cells[0].Value.ToString()), 1) == Math.Round(double.Parse(data[0]), 1)) {
                                tag = false;
                                break;
                            }
                        }
                        if (tag == true) {
                            int index = dgv.Rows.Add();
                            dgv.Rows[index].Cells[0].Value = double.Parse(data[0]);
                            dgv.Rows[index].Cells[1].Value = double.Parse(data[1]);
                        }
                    }
                    catch (Exception ex) {
                        //data = Regex.Split(line, @"[^,\s][^\,]*[^,\s]*", RegexOptions.IgnoreCase);
                        //dgvElevationLine.Rows[index].Cells[0].Value = data[0];
                        //dgvElevationLine.Rows[index].Cells[1].Value = data[1];
                        MessageBox.Show("发生异常：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="dt"></param>
        public static void Export2Excel(DataTable dt, string filepath) {
            if (dt == null || dt.Rows.Count == 0)
                return;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null) {
                MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try {
                Excel.Workbooks wbs = xlApp.Workbooks;
                Excel.Workbook wb = wbs.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                Excel.Range range;
                int totalCount = dt.Rows.Count;
                int rowRead = 0;
                float percent = 0;
                //ws.Columns.EntireColumn.AutoFit();
                for (int i = 0; i < dt.Columns.Count; i++) {
                    ws.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                    range = (Excel.Range)ws.Cells[1, i + 1];
                    range.Interior.ColorIndex = 15;
                    range.Font.Bold = true;
                    range.EntireColumn.AutoFit();
                }
                for (int r = 0; r < dt.Rows.Count; r++) {
                    for (int i = 0; i < dt.Columns.Count; i++) {
                        ws.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                    }
                    rowRead++;
                    percent = ((float)(100 * rowRead)) / totalCount;
                }
                xlApp.Visible = true;
                wb.Saved = true;
                wb.SaveCopyAs(filepath);
            }
            catch (Exception ex) {
                MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
            }
            finally {
                xlApp.Quit();
                GC.Collect();
            }
        }


        /// <summary>
        /// 将DataTable或DataSet导出为Excel
        /// 修改  2016/7/6
        /// </summary>
        /// <param name="dt">需要导出的DataTable,DataSet或GridView</param>
        /// <param name="AbosultedFilePath">Excel的绝对路径</param>
        /// <returns>是否成功</returns>
        public static bool ExportToExcel(DataTable dt, string AbosultedFilePath) {
            if (dt == null)
                return false;
            // 创建Excel应用程序对象
            Excel.Application excApp = new Excel.Application();
            Excel.Workbook workBook = excApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet workSeet = workBook.Worksheets[1]; //取得sheet1
            Excel.Range range = null;
            int tableCount = dt.Rows.Count;
            int rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++) {
                workSeet.Cells[1, i + 1] = dt.Columns[i].ColumnName;

                //设置标题的样式
                range = (Excel.Range)workSeet.Cells[1, i + 1];
                range.Font.Bold = true; //粗体
                range.Font.Size = "12";//字体大小
                range.Font.Name = "宋体";
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; //居中
                range.BorderAround(Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null); //背景色
                range.EntireColumn.AutoFit(); //自动设置列宽
                range.EntireRow.AutoFit(); //自动设置行高

            }
            for (int r = 0; r < dt.Rows.Count; r++) {
                for (int c = 0; c < dt.Columns.Count; c++) {
                    //写入内容
                    workSeet.Cells[r + 2, c + 1] = dt.Rows[r][c].ToString();
                    //设置样式
                    range = (Excel.Range)workSeet.Cells[r + 2, c + 1];
                    range.Font.Size = 9; //字体大小
                    range.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, null); //加边框
                    range.EntireColumn.AutoFit(); //自动调整列宽
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / tableCount;
                Application.DoEvents();
            }
            range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            if (dt.Columns.Count > 1) {
                range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
            }
            try {
                workBook.Saved = true;
                workBook.SaveCopyAs(AbosultedFilePath);
            }
            catch { }
            workBook.Close();
            if (excApp != null) {
                excApp.Workbooks.Close();
                excApp.Quit();
                int generation = GC.GetGeneration(excApp);
                Marshal.ReleaseComObject(excApp);
                excApp = null;
                GC.Collect(generation);
            }
            GC.Collect(); //强行销毁
            #region 强行杀死最近打开的Excel进程
            System.Diagnostics.Process[] excelProc = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            System.DateTime startTime = new DateTime();
            int m, killID = 0;
            for (m = 0; m < excelProc.Length; m++) {
                if (startTime < excelProc[m].StartTime) {
                    startTime = excelProc[m].StartTime;
                    killID = m;
                }
            }
            if (excelProc[killID].HasExited == false) {
                excelProc[killID].Kill();
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 使用模板导出Excel
        /// </summary>
        /// <param name="listEntity">数据集</param>
        /// <param name="path">路径</param>
        /// <param name="index">从第几行插入</param>
        /// <returns></returns>
        public static void TemplateOutput(DataTable dt, string tempPath, string filePath, int index) {
            if (dt.Rows.Count == 0) {
                return;
            }
            using (MemoryStream ms = new MemoryStream()) {
                string extension = Path.GetExtension(tempPath);
                FileStream fs = File.OpenRead(tempPath);
                IWorkbook workbook = null;
                if (extension.Equals(".xls")) {
                    workbook = new HSSFWorkbook(fs);
                }
                else {
                    workbook = new XSSFWorkbook(fs);
                }
                fs.Close();
                ISheet sheet = workbook.GetSheetAt(0);
                IRow rows = null;
                int i = 0;
                foreach (DataRow dr in dt.Rows) {
                    rows = sheet.CreateRow(i++ + index);
                    for (int j = 0; j < dt.Columns.Count; j++) {
                        var prop = dr[j];
                        var value = prop.ToString();
                        rows.CreateCell(j).SetCellValue(value ?? "");
                    }
                }
                workbook.Write(ms);
            }
        }

        /// <summary>
        /// 利用反射将Datatable转换为List<T>对象
        /// </summary>
        /// <typeparam name="T">集合</typeparam>
        /// <param name="dt"> datatable对象</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new() {

            //定义集合
            List<T> ts = new List<T>();

            //遍历dataTable中的数据行
            foreach (DataRow dr in dt.Rows) {
                T t = new T();
                //获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();

                //遍历该对象的所有属性
                foreach (PropertyInfo pi in propertys) {

                    string tempName = pi.Name;

                    if (!dt.Columns.Contains(tempName)) continue;   //检查datatable是否包含此列(列名==对象的属性名)    
                    object value = dr[tempName];      //取值
                    if (value == DBNull.Value) continue;  //如果非空，则赋给对象的属性
                    pi.SetValue(t, value, null);
                }
                //对象添加到泛型集合中
                ts.Add(t);
            }
            return ts;

        }

    }
}
