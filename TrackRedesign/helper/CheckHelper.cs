using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackRedesign {
    public class CheckHelper {
        public string Path { get; set; }
        public CheckHelper(string _path)
        {
            Path = _path;
        }
        private static string split = "\r\n---------------------------------------------------------\r\n";
        private static readonly double tolerance = 0.1;

        /// <summary>
        /// 检查轨道扣件文件的轨枕里程与往返测文件的里程是否一致
        /// </summary>
        /// <returns></returns>
        public static string CheckMiles(DataTable dtAverage, DataTable dtAdjustable,out string miles) {
            miles = string.Empty;
            int isNull = CalcHelper.NumOfNull(dtAdjustable);
            StringBuilder sb = new StringBuilder();
            sb.Append(split + "检查轨道扣件文件的里程与往返测文件的里程是否一致" + split);
            int toCount = dtAverage.Rows.Count;
            int AdjCount = dtAdjustable.Rows.Count - isNull;
            int countS = toCount <= AdjCount ? toCount : AdjCount;//取行数较小的一个
            //第一行数据两个里程相等，直接开始比较
            if (Math.Abs(double.Parse(dtAverage.Rows[0]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[isNull]["里程"].ToString()))<=tolerance) {
                int sum = 0;
                for (int i = 1; i < countS; i++) {
                    if (Math.Abs(double.Parse(dtAverage.Rows[i]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[i + isNull]["里程"].ToString()))>tolerance) {
                        sum++;
                        sb.Append($"往返测数据里程{double.Parse(dtAverage.Rows[i]["里程"].ToString())}与扣件数据里程{double.Parse(dtAdjustable.Rows[i+isNull]["里程"].ToString())}不一致\r\n");
                    }
                }
                int derta = dtAdjustable.Rows.Count - isNull - dtAverage.Rows.Count;
                if (derta >= 0) {//如果扣件行数多，则删除多的部分
                    for (int i = 0; i < derta; i++) {
                        dtAdjustable.Rows.RemoveAt(dtAdjustable.Rows.Count - 1);
                    }
                    sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAverage.Rows[toCount - 1]["里程"]}\r\n");
                    miles+=($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAverage.Rows[toCount - 1]["里程"]}");
                }
                else {
                    sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}\r\n");
                    miles += ($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}");
                }
                if (sum == 0) {
                    sb.Append("\r\n重叠段里程全部相等！" + split);
                }
                else {
                    sb.Append($"往返测数据与轨道扣件数据里程不一致个数：{sum}个" + split);
                }
            }
            //第一行数据往返测里程大于可调理里程，遍历可调量里程，找到里程相等的行
            else if(double.Parse(dtAverage.Rows[0]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[isNull]["里程"].ToString())>tolerance) {
                int index = 0;//起始里程相当的行索引，如index=2，表示第3行相等
                for (int i = 1; i < countS; i++) {
                    //if (Math.Abs(Math.Round((decimal)double.Parse(dtAverage.Rows[0]["里程"].ToString()),2,MidpointRounding.AwayFromZero)-Math.Round((decimal)double.Parse(dtAdjustable.Rows[i+isNull]["里程"].ToString()), 2, MidpointRounding.AwayFromZero))<= (decimal)0.05) {
                    if (Math.Abs(double.Parse(dtAverage.Rows[0]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[i + isNull]["里程"].ToString()))<=tolerance) {
                        index = i;//找到里程相等的行索引
                        break;
                    }
                }       
                if (index == 0) {
                    sb.Append("往返测数据与轨道扣件数据不是同一测段,请重新导入！" + split);
                }
                else {
                    for (int i = 0; i < index + isNull; i++) {
                        dtAdjustable.Rows.RemoveAt(0);
                    }
                    int count = toCount >= dtAdjustable.Rows.Count ? dtAdjustable.Rows.Count : toCount;//得到较小的行数
                    int sum = 0;
                    for (int i = 0; i < count; i++) {
                        if (Math.Abs(double.Parse(dtAverage.Rows[i]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[i]["里程"].ToString())) > tolerance) {
                            sb.Append($"往返测数据里程{double.Parse(dtAverage.Rows[i]["里程"].ToString())}与扣件数据里程{double.Parse(dtAdjustable.Rows[i]["里程"].ToString())}不一致\r\n");
                            sum++;
                        }
                    }
                    int derta = dtAdjustable.Rows.Count - dtAverage.Rows.Count;
                    if (derta >= 0) {//如果可调量行数多，则删除多的部分
                        for (int i = 0; i < derta; i++) {
                            dtAdjustable.Rows.RemoveAt(dtAdjustable.Rows.Count - 1);
                        }
                        sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAverage.Rows[toCount - 1]["里程"]}\r\n");
                        miles += ($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAverage.Rows[toCount - 1]["里程"]}");
                    }
                    else {
                        sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}\r\n");
                        miles += ($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}");
                    }
                    if (sum == 0) {
                        sb.Append("\r\n重叠段里程全部相等！" + split);
                    }
                    else {
                        sb.Append($"往返测数据与轨道扣件数据里程不一致个数：{sum}个" + split);
                    }
                }
            }

            //第一行数据往返测里程小于可调量里程，遍历往返测里程，找到里程相等的行
            else {
                int index = 0;
                for (int i = 1; i < countS; i++) {
                    if (Math.Abs(double.Parse(dtAverage.Rows[i]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[isNull]["里程"].ToString()))<=tolerance) {
                        index = i;
                        break;
                    }
                }
                if (index == 0) {
                    sb.Append("往返测数据与轨道扣件数据不是同一测段！！！" + split);
                }
                else {
                    for (int i = 0; i < index - isNull; i++) {
                        dtAdjustable.Rows.InsertAt(dtAdjustable.NewRow(), 0);
                    }
                    int isNullBig = CalcHelper.NumOfNull(dtAdjustable);
                    int count = toCount >= (dtAdjustable.Rows.Count) ? (dtAdjustable.Rows.Count) : toCount;//得到较小的行数
                    int sum = 0;
                    for (int i = index; i < count; i++) {
                        if (Math.Abs(double.Parse(dtAverage.Rows[i]["里程"].ToString()) - double.Parse(dtAdjustable.Rows[i]["里程"].ToString()))>tolerance) {
                            sb.Append($"往返测数据里程{double.Parse(dtAverage.Rows[i]["里程"].ToString())}与扣件数据里程{double.Parse(dtAdjustable.Rows[i]["里程"].ToString())}不一致\r\n");
                            sum++;
                        }
                    }
                    int derta = dtAdjustable.Rows.Count - dtAverage.Rows.Count;
                    if (derta >= 0) {//如果可调量行数多，则删除多的部分
                        for (int i = 0; i < derta; i++) {
                            dtAdjustable.Rows.RemoveAt(dtAdjustable.Rows.Count - 1);
                        }
                        sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAverage.Rows[toCount - 1]["里程"]}\r\n");
                        miles += ($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAverage.Rows[toCount - 1]["里程"]}");
                    }
                    else {
                        sb.Append($"\r\n重叠段起点里程为{dtAverage.Rows[0]["里程"]},终点里程为{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}\r\n");
                        miles += ($"里程范围={dtAverage.Rows[0]["里程"]}-{dtAdjustable.Rows[dtAdjustable.Rows.Count - 1]["里程"]}");
                    }
                    if (sum == 0) {
                        sb.Append("\r\n重叠段里程全部相等！" + split);
                    }
                    else {
                        sb.Append($"往返测数据与轨道扣件数据里程不一致个数：{sum}个" + split);
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 检查轨枕挡块型号与平面位置可调余量是否一致
        /// </summary>
        /// <returns></returns>
        public static string CheckStopper(DataTable dtModel) {
            StringBuilder sb = new StringBuilder();
            int sum = 0;
            sb.Append(split + "检查轨枕挡块型号与平面位置可调余量是否一致" + split);
            for (int i = 0; i < dtModel.Rows.Count; i++) {
                if (Convert.ToInt32(dtModel.Rows[i]["左股左调量"]) != (Convert.ToInt32(dtModel.Rows[i]["左股外口轨距块"]) - 4)) {
                    dtModel.Rows[i]["左股左调量"] = Convert.ToInt32(dtModel.Rows[i]["左股外口轨距块"]) - 4;
                    sb.Append($"左股向左可调量与轨枕挡块型号在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["左股右调量"]) != (14 - Convert.ToInt32(dtModel.Rows[i]["左股外口轨距块"]))) {
                    dtModel.Rows[i]["左股右调量"] = 14 - Convert.ToInt32(dtModel.Rows[i]["左股外口轨距块"]);
                    sb.Append($"左股向右可调量与轨枕挡块型号在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["右股左调量"]) != (Convert.ToInt32(dtModel.Rows[i]["右股里口轨距块"]) - 4)) {
                    dtModel.Rows[i]["右股左调量"] = Convert.ToInt32(dtModel.Rows[i]["右股里口轨距块"]) - 4;
                    sb.Append($"右股向左可调量与轨枕挡块型号在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["右股右调量"]) != (14 - Convert.ToInt32(dtModel.Rows[i]["右股里口轨距块"]))) {
                    dtModel.Rows[i]["右股右调量"] = 14 - Convert.ToInt32(dtModel.Rows[i]["右股里口轨距块"]);
                    sb.Append($"右股向右可调量与轨枕挡块型号在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
            }
            if (sum!=0) {
                sb.Append($"\r\n轨枕挡块型号与平面位置可调余量共有{sum}处不一致，已修改" + split);
            }
            else {
                sb.Append("\r\n轨枕挡块型号与平面位置可调余量全部一致！" + split);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 检查轨枕垫片型号与高程可调余量是否一致
        /// </summary>
        /// <returns></returns>
        public static string CheckSpacer(DataTable dtModel) {
            StringBuilder sb = new StringBuilder();
            int sum = 0;
            for (int i = 0; i < dtModel.Rows.Count; i++) {
                if (Convert.ToDouble(dtModel.Rows[i]["左股上调量"]) != 18-Convert.ToDouble(dtModel.Rows[i]["左股胶垫厚度"])) {
                    dtModel.Rows[i]["左股上调量"] = 18 - Convert.ToDouble(dtModel.Rows[i]["左股胶垫厚度"]);
                    sb.Append($"左股向上可调量与轨枕垫片厚度在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["左股下调量"]) != (Convert.ToInt32(dtModel.Rows[i]["左股胶垫厚度"]) - 2)) {
                    dtModel.Rows[i]["左股下调量"] = Convert.ToInt32(dtModel.Rows[i]["左股胶垫厚度"]) - 2;
                    sb.Append($"左股向下可调量与轨枕垫片厚度在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["右股上调量"]) != 18 - Convert.ToDouble(dtModel.Rows[i]["右股胶垫厚度"])) {
                    dtModel.Rows[i]["右股上调量"] = 18 - Convert.ToDouble(dtModel.Rows[i]["右股胶垫厚度"]);
                    sb.Append($"右股向上可调量与轨枕垫片厚度在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
                if (Convert.ToInt32(dtModel.Rows[i]["右股下调量"]) != (Convert.ToInt32(dtModel.Rows[i]["右股胶垫厚度"]) - 2)) {
                    dtModel.Rows[i]["右股下调量"] = Convert.ToInt32(dtModel.Rows[i]["右股胶垫厚度"]) - 2;
                    sb.Append($"右股向下可调量与轨枕垫片厚度在里程{dtModel.Rows[i]["里程"]}处不一致\r\n");
                    sum++;
                }
            }
            if (sum!=0) {
                sb.Append($"\r\n轨枕垫片型号与高程可调余量共有{sum}处不一致，已修改" + split);
            }
            else {
                sb.Append("\r\n轨枕垫片型号与高程可调余量全部一致！" + split);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 检查往返测高程、平面位置、轨距、水平较差是否满足限差阈值要求。
        /// </summary>
        /// <returns></returns>
        public static string CheckToAndFroData(DataTable dtTo, DataTable dtFro,DataTable parameter) {
            StringBuilder sb = new StringBuilder();
            sb.Append(split + "检测往返测高程、平面位置、轨距、水平较差是否满足限差阈值要求" + split );
            int sumHeight = 0, sumPlainPosition = 0, sumGauge = 0, sumHerizontal = 0;
            for (int i = 0; i < dtTo.Rows.Count; i++) {
                double detarH = Math.Abs(Convert.ToDouble(dtTo.Rows[i]["调前高程"]) - Convert.ToDouble(dtFro.Rows[i]["调前高程"]));
                double detarP = Math.Abs(Convert.ToDouble(dtTo.Rows[i]["调前平面"]) - Convert.ToDouble(dtFro.Rows[i]["调前平面"]));
                double detarG = Math.Abs(Convert.ToDouble(dtTo.Rows[i]["调前水平"]) - Convert.ToDouble(dtFro.Rows[i]["调前水平"]));
                double detarHt = Math.Abs(Convert.ToDouble(dtTo.Rows[i]["调前轨距"]) - Convert.ToDouble(dtFro.Rows[i]["调前轨距"]));
                if (detarH > double.Parse(parameter.Rows[0][1].ToString())) {
                    sumHeight++;
                    sb.Append($"往返测高程在里程{dtTo.Rows[i]["里程"]}处超限，超限值为{detarH}\r\n");
                }
                if (detarP > double.Parse(parameter.Rows[1][1].ToString())) {
                    sumPlainPosition++;
                    sb.Append($"往返测平面位置在里程{dtTo.Rows[i]["里程"]}处超限，超限值为{detarP}\r\n");
                }
                if (detarG > double.Parse(parameter.Rows[3][1].ToString())) {
                    sumHerizontal++;
                    sb.Append($"往返测水平在里程{dtTo.Rows[i]["里程"]}处超限，超限值为{detarG}\r\n");
                }
                if (detarHt > double.Parse(parameter.Rows[2][1].ToString())) {
                    sumGauge++;
                    sb.Append($"往返测轨距在里程{dtTo.Rows[i]["里程"]}处超限，超限值为{detarHt}\r\n");
                }
            }
            sb.Append($"\r\n往返测高程超限个数：{sumHeight}个\r\n");
            sb.Append($"往返测平面位置超限个数：{sumPlainPosition}个\r\n");
            sb.Append($"往返测水平超限个数：{sumHerizontal}个\r\n");
            sb.Append($"往返轨距超限个数：{sumGauge}个" + split);
            return sb.ToString ();
        }
    }
}
