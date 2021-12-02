using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    /// <summary>
    /// EXCEL 导出 读取帮助类
    /// </summary>
    public class ExceNPOIlHelep
    {


        #region 公共变量
        /// <summary>
        /// 桌面物理路径
        /// </summary>
        public static string bootPathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+"\\";

        static string remark = "面向里程增长方向。拨道方向:+表示向右拨,-表示向左拨.起道方向:+表示起道,-表示降道.水平：+表示右轨高于左轨";
        /// <summary>
        /// 模板物理路径
        /// </summary>
        public static string bootTemplate = Application.StartupPath + "\\";
        //public static string bootTemplate = Application.StartupPath + "\\ExcelTemplate\\";
        #endregion

        #region Winfrom框架下使用
        /// <summary>
        /// 导出Excel 
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="dw">DataGridView</param>
        /// <param name="filepath">导入文件全路径</param>
        /// <returns>bool值成功或失败</returns>
        public static bool ReportExcelForDataGridView(DataTable dt, DataGridView dw, string filepath)
        {
            try
            {
                DataTable dtnew = new DataTable();

                #region DataGridView 表头利用 对应的转换

                List<string> ReportColumn = new List<string>();
                foreach (DataGridViewColumn item in dw.Columns)
                {
                    string DataPropertyName = item.DataPropertyName;
                    string HeaderText = item.HeaderText;
                    dt.Columns[DataPropertyName].ColumnName = HeaderText;
                    ReportColumn.Add(HeaderText);
                }

                dtnew = dt.DefaultView.ToTable(false, ReportColumn.ToArray());
                #endregion

                return ReportExcel(dtnew, filepath);
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
        #endregion

        #region 公共
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="filepath">导入文件全路径</param>
        /// <returns>bool值成功或失败</returns>
        public static  bool ReportExcel(DataTable dt,string filepath)
        {
            try
            {
                 NPOI.SS.UserModel.IWorkbook book = null;
            book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("sheet1");
            // 添加表头
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);

            int index = 0;
            foreach (DataColumn item in dt.Columns)
            {

                NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
                cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                cell.SetCellValue(item.ColumnName);
                cell.CellStyle = GetHeadStyle(book, true);
                #region  根据列名设定列宽度
                int ColumnNameLength = System.Text.Encoding.Default.GetBytes(item.ColumnName).Length;
                if (ColumnNameLength > 16)
                {

                    sheet.SetColumnWidth(index, 30 * ColumnNameLength * 10);
                }
                else
                {
                    sheet.SetColumnWidth(index, 30 * 150);
                }
                #endregion
                index++;

            }

            // 添加数据     
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                index = 0;
                row = sheet.CreateRow(i + 1);
                foreach (DataColumn item in dt.Columns)
                {
                    NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
                    cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                    cell.SetCellValue(dt.Rows[i][item.ColumnName].ToString());
                    cell.CellStyle = GetHeadStyle(book, false); ;
                    index++;
                }
            }
            // 写入 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            book = null;


            //导出到桌面
            //string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\aaaa.xls";


            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }

            ms.Close();
            ms.Dispose();
            return true;
            }
            catch (Exception) {

                return false;
            }
            
        }


        /// <summary>
        /// 模板导出
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="filepath">导出的路径</param>
        /// <param name="template">模板路径</param>
        /// <returns></returns>
        public static bool ReportExcelForTemplate(DataTable dt, string filepath, string template, string head, string time, string maker) {
            try {
                FileStream fs1 = new FileStream(template, FileMode.Open, FileAccess.Read);
                HSSFWorkbook book = new HSSFWorkbook(fs1);
                HSSFCellStyle styleHead = (HSSFCellStyle)book.CreateCellStyle();
                IDataFormat formatNum = book.CreateDataFormat();
                styleHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CenterSelection;
                IFont fontHead = book.CreateFont();
                fontHead.FontName = "宋体";
                fontHead.FontHeightInPoints = 12;
                styleHead.SetFont(fontHead);
                //默认只取第一个sheet
                ISheet sheet = book.GetSheetAt(0);
                //sheet.CreateFreezePane(0, 4);
                //最后一行
                int LastRowNum = 4;
                //最后一行 行对象
                IRow LastRow = sheet.GetRow(LastRowNum);
                //行对象

                //添加表头
                IRow headRow = null;
                for (int i = 0; i < 2; i++) {
                    headRow = sheet.CreateRow(i);
                    ICell headCell = headRow.CreateCell(0);
                    headCell.SetCellType(CellType.String);
                    if (i == 0) {
                        ICellStyle cellStyle = book.CreateCellStyle();
                        cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CenterSelection;
                        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.RightBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.TopBorderColor = HSSFColor.Black.Index;
                        IFont font = book.CreateFont();
                        font.FontName = "宋体";
                        font.FontHeightInPoints = 14;
                        font.IsBold = true;
                        cellStyle.SetFont(font);
                        headCell.CellStyle = cellStyle;
                        headCell.SetCellValue(head.Trim());
                    }
                    else {
                        ICellStyle cellStyle = book.CreateCellStyle();
                        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.RightBorderColor = HSSFColor.Black.Index;
                        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        cellStyle.TopBorderColor = HSSFColor.Black.Index;
                        IFont font = book.CreateFont();
                        font.FontName = "黑体";
                        font.FontHeightInPoints = 12;
                        cellStyle.SetFont(font);
                        headCell.CellStyle = cellStyle;
                        headCell.SetCellValue($"精测时间：{time.Trim()}".PadLeft(25) + $"方案制定人：{maker.Trim()}".PadLeft(60));
                    }
                }
                //添加数据
                IRow row = null;
                //导出数据的总行数     
                for (int i = 0; i < dt.Rows.Count; i++) {
                    row = sheet.CreateRow(i + LastRowNum);
                    if (i == 0) {
                        //添加备注
                        ICell cellRemark = row.CreateCell(19);
                        HSSFCellStyle styleRemark = (HSSFCellStyle)book.CreateCellStyle();
                        styleRemark.WrapText = true;
                        styleRemark.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CenterSelection;
                        styleRemark.VerticalAlignment = VerticalAlignment.Center;
                        IFont font = book.CreateFont();
                        font.FontName = "宋体";
                        font.FontHeightInPoints = 12;
                        styleRemark.SetFont(font);
                        cellRemark.CellStyle = styleRemark;
                        cellRemark.SetCellType(CellType.String);
                        cellRemark.CellStyle = styleHead;
                        //string remarkNew = string.Empty;
                        //for (int count = 0; count < remark.Length; count++) {
                        //    remarkNew += remark[count] + "\n";
                        //}
                        cellRemark.SetCellValue(remark);
                    }
                    //导出数据 行对应的列数
                    for (int j = 0; j < 19; j++) {
                        //string strCellValue = LastRow.Cells[j].StringCellValue.Replace("#", "");
                        string data = dt.Rows[i][j].ToString();
                        Boolean isNum = false;//data是否为数值型
                        if (data != null) {
                            //判断data是否为数值型
                            isNum = Regex.IsMatch(data.ToString(), "^(-?\\d+)(\\.\\d+)?$");
                        }
                        //列对象
                        ICell cell = row.CreateCell(j);
                        //模板列对应的值
                        string strSetCellValue =data;
                        //列数据类型 
                        if (isNum) {
                            if (j != 0 && j != 1) {
                                styleHead.DataFormat = formatNum.GetFormat("0.0");
                                cell.CellStyle = styleHead;
                            }
                            cell.SetCellType(CellType.Numeric);
                            //列对应值
                            cell.SetCellValue(double.Parse(strSetCellValue));
                        }
                        else {
                            cell.CellStyle = styleHead;
                            cell.SetCellType(CellType.String);
                            cell.SetCellValue(strSetCellValue);
                        }
                    }
                }

                // 写入 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                book = null;

                if (File.Exists(filepath)) {
                    File.Delete(filepath);
                }
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write)) {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }

                ms.Close();
                ms.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show("发生错误" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


       

        /// <summary>
        /// 表头和内容样式
        /// </summary>
        /// <param name="book">操作对象</param>
        /// <param name="boo">是否是表头</param>
        /// <returns></returns>
        private static ICellStyle GetHeadStyle(NPOI.SS.UserModel.IWorkbook book, bool boo) {
            ICellStyle styleHead = book.CreateCellStyle();
            styleHead.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.BottomBorderColor = HSSFColor.Black.Index;
            styleHead.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.LeftBorderColor = HSSFColor.Black.Index;
            styleHead.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.RightBorderColor = HSSFColor.Black.Index;
            styleHead.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.TopBorderColor = HSSFColor.Black.Index;

            IFont fontHead = book.CreateFont();
            fontHead.FontName = "宋体";
            fontHead.FontHeightInPoints = 12;
            if (boo) {
                //表头样式    
                styleHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                fontHead.Boldweight = (short)FontBoldWeight.Bold;
            }
            else {
                //内容样式

            }
            styleHead.SetFont(fontHead);
            return styleHead;
        }

        #endregion
    }
}
