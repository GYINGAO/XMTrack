using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackRedesign {
    public class CalcHelper {
        public CalcHelper() {

        }
        //private const int precision = 2;
        //private const string format = "0.00";
        //private const string formatMile = "0.0";

        /// <summary>
        /// 定义点的结构
        /// </summary>
        public struct Point {
            public double x;
            public double y;
        }

        /// <summary>
        /// 给定两点，计算趋势线斜率和截距
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Tuple<double, double> CaCalculateLine(Point point1, Point point2) {
            double slope = (point1.y - point2.y) / (point1.x - point2.x);
            double intercept = point1.y - slope * point1.x;
            Tuple<double, double> tup = new Tuple<double, double>(slope, intercept);
            return tup;
        }

        /// <summary>
        /// 计算调整中线的斜率
        /// </summary>
        /// <param name="tendLine"></param>
        /// <returns></returns>
        public void CalcSlope(DataTable tendLine, DataTable slope) {
            slope.Rows.Clear();
            for (int i = 0; i < tendLine.Rows.Count - 1; i++) {
                Point pointP1, pointP2;
                pointP1.x = Convert.ToDouble(tendLine.Rows[i]["里程"].ToString());
                pointP1.y = Convert.ToDouble(tendLine.Rows[i]["偏差值"].ToString());
                pointP2.x = Convert.ToDouble(tendLine.Rows[i + 1]["里程"].ToString());
                pointP2.y = Convert.ToDouble(tendLine.Rows[i + 1]["偏差值"].ToString());
                var tuple = CaCalculateLine(pointP1, pointP2);

                DataRow dr = slope.NewRow();
                dr["起始里程"] = tendLine.Rows[i]["里程"];
                dr["结束里程"] = tendLine.Rows[i + 1]["里程"];
                dr["斜率"] = tuple.Item1;
                dr["截距"] = tuple.Item2;
                slope.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 将dgv数据导入调整中线datatable中
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public void CalaTendLine(DataGridView dgv, DataTable tendLine, params string[] colName) {
            tendLine.Rows.Clear();
            // 循环行
            for (int count = 0; count < dgv.Rows.Count; count++) {
                DataRow dr = tendLine.NewRow();
                dr["里程"] = Convert.ToDouble(dgv.Rows[count].Cells[colName[0]].Value);
                dr["偏差值"] = Convert.ToDouble(dgv.Rows[count].Cells[colName[1]].Value);
                tendLine.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 计算平面模拟调整量
        /// </summary>
        /// <returns></returns>
        public void CalcPPSimulationAdjustment(DataTable averageValue,
            DataTable simulationAdjustmentPP, DataTable slopeP, DataTable tendLineP, int datumTrack) {
            simulationAdjustmentPP.Rows.Clear();
            int averageValueCount = averageValue.Rows.Count;
            for (int i = 0; i < averageValueCount; i++) {
                double beforeAPlane = Convert.ToDouble(averageValue.Rows[i]["调前平面"].ToString());
                double beforeAGauge = Convert.ToDouble(averageValue.Rows[i]["调前轨距"].ToString());
                double x = Convert.ToDouble(averageValue.Rows[i]["里程"].ToString());
                double afterAPlane = GetPPOrElevationAnalogAdj(x, slopeP, tendLineP);
                DataRow dr1 = simulationAdjustmentPP.NewRow();
                dr1["里程"] = Convert.ToDouble(averageValue.Rows[i]["里程"].ToString());
                dr1["调后平面"] = afterAPlane;
                switch (datumTrack) {
                    case 0://基准轨为左轨
                        {
                            double leftPCount = beforeAPlane - afterAPlane;
                            double rightPCount = Convert.ToDouble(averageValue.Rows[i]["调右平面"].ToString()) -
                                Convert.ToDouble(averageValue.Rows[i]["调左平面"].ToString()) + beforeAPlane - afterAPlane;
                            double afterAGauge = beforeAGauge - leftPCount + rightPCount;
                            dr1["调后轨距"] = afterAGauge;
                            dr1["调左平面"] = leftPCount;
                            dr1["调右平面"] = rightPCount;
                            break;
                        }
                    case 1://基准轨为右轨
                        {
                            double rightPCount = beforeAPlane - afterAPlane;
                            double leftPCount = rightPCount + beforeAGauge;
                            double afterAGauge = beforeAGauge - leftPCount + rightPCount;
                            dr1["调后轨距"] = afterAGauge;
                            dr1["调右平面"] = rightPCount;
                            dr1["调左平面"] = leftPCount;
                            break;
                        }
                    default: {
                            //MessageBox.Show("请检查基准股!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
                simulationAdjustmentPP.Rows.Add(dr1);
            }
        }

        /// <summary>
        /// 计算高程模拟调整量
        /// </summary>
        /// <returns></returns>
        public void CalcElevationSimulationAdjustment(DataTable averageValue,
            DataTable simulationAdjustmentEvelation, DataTable slopeG, DataTable tendLineG, int datumTrack) {
            simulationAdjustmentEvelation.Rows.Clear();
            int averageValueCount = averageValue.Rows.Count;
            for (int i = 0; i < averageValueCount; i++) {
                double beforeAG = Convert.ToDouble(averageValue.Rows[i]["调前高程"].ToString());
                double beforeALevel = Convert.ToDouble(averageValue.Rows[i]["调前水平"].ToString());
                double x = Convert.ToDouble(averageValue.Rows[i]["里程"].ToString());
                double afterAG = GetPPOrElevationAnalogAdj(x, slopeG, tendLineG);
                DataRow dr1 = simulationAdjustmentEvelation.NewRow();
                dr1["里程"] = Convert.ToDouble(averageValue.Rows[i]["里程"].ToString());
                dr1["调后高程"] = afterAG;
                switch (datumTrack) {
                    case 0://基准轨为左轨
                        {
                            double leftGCount = afterAG - beforeAG;
                            double rightGCount = Convert.ToDouble(averageValue.Rows[i]["调右轨高"].ToString()) -
                                Convert.ToDouble(averageValue.Rows[i]["调左轨高"].ToString()) - beforeAG + afterAG;
                            double afterALevel = beforeALevel - leftGCount + rightGCount;
                            dr1["调后水平"] = afterALevel;
                            dr1["调左高程"] = leftGCount;
                            dr1["调右高程"] = rightGCount;
                            break;
                        }
                    case 1://基准轨为右轨
                        {
                            double plane3 = afterAG - beforeAG;
                            double plane4 = beforeALevel + plane3;
                            double afterALevel = beforeALevel - plane4 + plane3;
                            dr1["调后水平"] = afterALevel;
                            dr1["调右高程"] = plane3;
                            dr1["调左高程"] = plane4;
                            break;
                        }
                    default: {
                            //MessageBox.Show("请检查基准股", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
                simulationAdjustmentEvelation.Rows.Add(dr1);
            }
        }

        /// <summary>
        /// 根据调整中线的斜率，获得任意里程平面或高程模拟调整量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="slope"></param>
        /// <param name="tendLine"></param>
        /// <returns></returns>
        public double GetPPOrElevationAnalogAdj(double x, DataTable slope, DataTable tendLine) {
            int slopeCount = slope.Rows.Count;
            double result = 0;
            for (int i = 0; i < slopeCount; i++) {
                double PreviousY = Convert.ToDouble(tendLine.Rows[i]["偏差值"].ToString());
                double k = Convert.ToDouble(slope.Rows[i]["斜率"].ToString());
                double PreviousX = Convert.ToDouble(slope.Rows[i]["起始里程"].ToString());
                double lastX = Convert.ToDouble(slope.Rows[i]["结束里程"].ToString());
                if (x >= PreviousX && x <= lastX) {
                    result = PreviousY + k * (x - PreviousX);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 平面模拟调整量取整
        /// </summary>
        /// <param name="actualAdjustment"></param>
        /// <param name="averageValue"></param>
        /// <param name="simulationAdjustment"></param>
        /// <param name="datumTrack"></param>
        /// <returns></returns> 
        public void CalcPPActualAdjustment(DataTable actualAdjustmentPP, DataTable averageValue,
            DataTable simulationAdjustmentPP, int datumTrack, double range) {
            actualAdjustmentPP.Rows.Clear();
            int count = simulationAdjustmentPP.Rows.Count;
            if (range == 0) {
                for (int i = 0; i < count; i++) {
                    DataRow dr = actualAdjustmentPP.NewRow();
                    dr["里程"] = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["里程"].ToString());
                    dr["调左平面"] = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["调左平面"].ToString());
                    dr["调右平面"] = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["调右平面"].ToString());
                    actualAdjustmentPP.Rows.Add(dr);
                }
            }
            else if (range == 1) {
                for (int i = 0; i < count; i++) {
                    DataRow dr1 = actualAdjustmentPP.NewRow();
                    double beforeSimulationAGauge = Convert.ToDouble(averageValue.Rows[i]["调前轨距"].ToString());
                    double simulationRightAAamount = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["调右平面"].ToString());
                    double simulationLeftAAmount = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["调左平面"].ToString());
                    double actualRightAAmonut, actualLeftAAmount;
                    dr1["里程"] = Convert.ToDouble(simulationAdjustmentPP.Rows[i]["里程"].ToString());
                    switch (datumTrack) {
                        case 0: {
                                actualLeftAAmount = Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5);
                                dr1["调左平面"] = actualLeftAAmount;

                                //平面
                                if (beforeSimulationAGauge - actualLeftAAmount + Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5) <= -0.5) {
                                    actualRightAAmonut = Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5) + 1;
                                    dr1["调右平面"] = actualRightAAmonut;
                                }
                                else if (beforeSimulationAGauge - actualLeftAAmount + Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5) > 0.5) {
                                    actualRightAAmonut = Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5) - 1;
                                    dr1["调右平面"] = actualRightAAmonut;

                                }
                                else {
                                    actualRightAAmonut = Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5);
                                    dr1["调右平面"] = actualRightAAmonut;

                                }
                                break;
                            }
                        case 1: {
                                //double actualRightAAmonut, actualLeftAAmount, actualRightAGAmonut, actualLeftAGAmount;
                                actualRightAAmonut = Math.Sign(simulationRightAAamount) * INT(Math.Abs(simulationRightAAamount) + 0.5);
                                dr1["调右平面"] = actualRightAAmonut;
                                //平面
                                if (beforeSimulationAGauge + actualRightAAmonut - Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5) < -0.5) {
                                    actualLeftAAmount = Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5) - 1;
                                    dr1["调左平面"] = actualLeftAAmount;
                                }
                                else if (beforeSimulationAGauge + actualRightAAmonut - Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5) >= 0.5) {
                                    actualLeftAAmount = Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5) + 1;
                                    dr1["调左平面"] = actualLeftAAmount;

                                }
                                else {
                                    actualLeftAAmount = Math.Sign(simulationLeftAAmount) * INT(Math.Abs(simulationLeftAAmount) + 0.5);
                                    dr1["调左平面"] = actualLeftAAmount;

                                }
                                break;
                            }
                        default: {
                                //MessageBox.Show("请检查基准股！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                    }
                    actualAdjustmentPP.Rows.Add(dr1);
                }
            }
        }

        /// <summary>
        /// 高程模拟调整量取整
        /// </summary>
        /// <param name="actualAdjustment"></param>
        /// <param name="averageValue"></param>
        /// <param name="simulationAdjustment"></param>
        /// <param name="datumTrack"></param>
        /// <returns></returns>
        public void CalcElevationActualAdjustment(DataTable actualAdjustmentEvelation, DataTable averageValue,
            DataTable simulationAdjustmentEvelation, int datumTrack, double range) {
            actualAdjustmentEvelation.Rows.Clear();
            int count = simulationAdjustmentEvelation.Rows.Count;
            if (range == 0) {
                for (int i = 0; i < count; i++) {
                    DataRow dr = actualAdjustmentEvelation.NewRow();
                    dr["里程"] = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["里程"].ToString());
                    dr["调左高程"] = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["调左高程"].ToString());
                    dr["调右高程"] = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["调右高程"].ToString());
                    actualAdjustmentEvelation.Rows.Add(dr);
                }
            }
            else {
                for (int i = 0; i < count; i++) {
                    DataRow dr = actualAdjustmentEvelation.NewRow();
                    double beforeSimulationALevel = Convert.ToDouble(averageValue.Rows[i]["调前水平"].ToString());
                    double simulationRightAGamount = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["调右高程"].ToString());
                    double simulationLeftAGmount = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["调左高程"].ToString());
                    double actualRightAGAmonut, actualLeftAGAmount;
                    dr["里程"] = Convert.ToDouble(simulationAdjustmentEvelation.Rows[i]["里程"].ToString());
                    switch (datumTrack) {
                        case 0: {
                                actualLeftAGAmount = Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5);
                                dr["调左高程"] = actualLeftAGAmount;
                                //高程
                                if (beforeSimulationALevel - actualLeftAGAmount + Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5) <= -0.5) {
                                    actualRightAGAmonut = Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5) + 1;
                                    dr["调右高程"] = actualRightAGAmonut;
                                }
                                else if (beforeSimulationALevel - actualLeftAGAmount + Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5) > 0.5) {
                                    actualRightAGAmonut = Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5) - 1;
                                    dr["调右高程"] = actualRightAGAmonut;
                                }
                                else {
                                    actualRightAGAmonut = Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5);
                                    dr["调右高程"] = actualRightAGAmonut;
                                }
                                break;
                            }
                        case 1: {
                                actualRightAGAmonut = Math.Sign(simulationRightAGamount) * INT(Math.Abs(simulationRightAGamount) + 0.5);
                                dr["调右高程"] = actualRightAGAmonut;
                                //高程
                                if (beforeSimulationALevel + actualRightAGAmonut - Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5) < -0.5) {
                                    actualLeftAGAmount = Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5) - 1;
                                    dr["调左高程"] = actualLeftAGAmount;
                                }
                                else if (beforeSimulationALevel + actualRightAGAmonut - Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5) >= 0.5) {
                                    actualLeftAGAmount = Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5) + 1;
                                    dr["调左高程"] = actualLeftAGAmount;
                                }
                                else {
                                    actualLeftAGAmount = Math.Sign(simulationLeftAGmount) * INT(Math.Abs(simulationLeftAGmount) + 0.5);
                                    dr["调左高程"] = actualLeftAGAmount;
                                }
                                break;
                            }
                        default: {
                                //MessageBox.Show("请检查基准股！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                    }
                    actualAdjustmentEvelation.Rows.Add(dr);
                }
            }
        }

        /// <summary>
        /// 取整函数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private double INT(double n) {
            if (n >= 0)
                return Math.Floor(n);
            else
                return -Math.Ceiling(-n);
        }

        /// <summary>
        /// 计算平面基准股可调量
        /// </summary>
        /// <param name="averageValue"></param>
        /// <param name="importAdjustable"></param>
        /// <param name="simulationAdjustment"></param>
        /// <param name="basicTrackAdjustable"></param>
        /// <param name="datumTrack"></param>
        /// <returns></returns>
        public void CalcPPBasicTrackAdjustable(DataTable averageValue, DataTable importAdjustable,
            DataTable simulationAdjustmentPP, DataTable basicTrackAdjustablePP, int datumTrack) {
            basicTrackAdjustablePP.Rows.Clear();
            int isNull = NumOfNull(importAdjustable);
            int count1 = averageValue.Rows.Count;
            int count2 = importAdjustable.Rows.Count - isNull;
            int count3 = count1 >= count2 ? count2 : count1;
            for (int i = isNull; i < count3; i++) {
                DataRow dr = basicTrackAdjustablePP.NewRow();
                dr["轨枕编号"] = importAdjustable.Rows[i]["轨枕编号"].ToString();
                dr["里程"] = Convert.ToDouble(importAdjustable.Rows[i]["里程"].ToString());
                double afterSimuPlane = Convert.ToDouble(simulationAdjustmentPP.Rows[i - isNull]["调后平面"].ToString());
                switch (datumTrack) {
                    case 0: {
                            double numRight = Convert.ToDouble(importAdjustable.Rows[i]["左股右调量"].ToString());
                            double numLeft = Convert.ToDouble(importAdjustable.Rows[i]["左股左调量"].ToString());
                            dr["基准股左调"] = afterSimuPlane - numLeft;
                            dr["基准股右调"] = afterSimuPlane + numRight;
                            break;
                        }
                    case 1: {
                            double numRight = Convert.ToDouble(importAdjustable.Rows[i]["右股右调量"].ToString());
                            double numLeft = Convert.ToDouble(importAdjustable.Rows[i]["右股左调量"].ToString());
                            dr["基准股左调"] = afterSimuPlane - numLeft;
                            dr["基准股右调"] = afterSimuPlane + numRight;
                            break;
                        }
                    default: {
                            //MessageBox.Show("请检查基准股！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
                basicTrackAdjustablePP.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 计算高程基准股可调量
        /// </summary>
        /// <param name="averageValue"></param>
        /// <param name="importAdjustable"></param>
        /// <param name="simulationAdjustment"></param>
        /// <param name="basicTrackAdjustable"></param>
        /// <param name="datumTrack"></param>
        /// <returns></returns>
        public void CalcElevationBasicTrackAdjustable(DataTable averageValue, DataTable importAdjustable,
            DataTable simulationAdjustmentElevation, DataTable basicTrackAdjustableElevation, int datumTrack) {
            basicTrackAdjustableElevation.Rows.Clear();
            int isNull = NumOfNull(importAdjustable);
            int count1 = averageValue.Rows.Count;
            int count2 = importAdjustable.Rows.Count - isNull;
            int count3 = count1 >= count2 ? count2 : count1;
            for (int i = isNull; i < count3; i++) {
                DataRow dr = basicTrackAdjustableElevation.NewRow();
                dr["轨枕编号"] = importAdjustable.Rows[i]["轨枕编号"].ToString();
                dr["里程"] = Convert.ToDouble(importAdjustable.Rows[i]["里程"].ToString());
                double afterSimuG = Convert.ToDouble(simulationAdjustmentElevation.Rows[i - isNull]["调后高程"].ToString());
                switch (datumTrack) {
                    case 0: {
                            double numUp = Convert.ToDouble(importAdjustable.Rows[i]["左股上调量"].ToString());
                            double numDown = Convert.ToDouble(importAdjustable.Rows[i]["左股下调量"].ToString());
                            dr["基准股下调"] = afterSimuG + numDown;
                            dr["基准股上调"] = afterSimuG - numUp;
                            break;
                        }
                    case 1: {
                            double numUp = Convert.ToDouble(importAdjustable.Rows[i]["右股上调量"].ToString());
                            double numDown = Convert.ToDouble(importAdjustable.Rows[i]["右股下调量"].ToString());
                            dr["基准股下调"] = afterSimuG + numDown;
                            dr["基准股上调"] = afterSimuG - numUp;
                            break;
                        }
                    default: break;
                }
                basicTrackAdjustableElevation.Rows.Add(dr);
            }
        }
        /// <summary>
        /// 判断datatable空行的个数
        /// </summary>
        /// <returns></returns>
        public static int NumOfNull(DataTable dt) {
            int sum = 0;
            for (int i = 0; i < dt.Rows.Count; i++) {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString())) {
                    sum++;
                }
            }
            return sum;
        }

        /// <summary>
        /// 计算直线方程参数
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        public static double[] CalcLine(Point p1, Point p2) {
            double[] l = new double[3];
            l[0] = p2.y - p1.y;
            l[1] = p1.x - p2.x;
            l[2] = p1.y * (p2.x - p1.x) - p1.x * (p2.y - p1.y);
            return l;
        }

        /// <summary>
        /// 计算折点处角度
        /// </summary>
        /// <param name="ps">起点</param>
        /// <param name="pm">中点</param>
        /// <param name="pe">终点</param>
        /// <returns></returns>
        public static double CalcAngle(Point ps, Point pm, Point pe) {
            //double[] l1 = CalcLine(ps, pm);
            //double[] l2 = CalcLine(pm, pe);
            //double angle = Math.Acos(Math.Abs(l1[1] * l2[1] + l1[0] * l2[0]) / (Math.Sqrt(l1[0] * l1[0] + l1[1] * l1[1]) * Math.Sqrt(l2[0] * l2[0] + l2[1] * l2[1])));
            //return angle;
            Point pm2ps = new Point() { x = ps.x - pm.x, y = ps.y - pm.y };
            Point pm2pe = new Point() { x = pe.x - pm.x, y = pe.y - pm.y };
            double pm2psLenth = Math.Sqrt(pm2ps.x * pm2ps.x + pm2ps.y * pm2ps.y);
            double pm2peLenth = Math.Sqrt(pm2pe.x * pm2pe.x + pm2pe.y * pm2pe.y);
            double angle = Math.Acos((pm2ps.x * pm2pe.x + pm2ps.y * pm2pe.y) / (pm2psLenth * pm2peLenth));
            return angle;
        }

        public static double CalcBend(double angle, double length) {
            return Math.Round(length / 2 / Math.Tan(angle / 2), 2);
        }

        /// <summary>
        /// 计算基准轨超限报表
        /// </summary>
        /// <param name="range">可调范围</param>
        /// <param name="plan">调整量</param>
        /// <returns></returns>
        public static Tuple<DataTable, int, double> CalcOverrunResult(DataTable range, DataTable plan, int datumTrack) {
            DataTable dt = new DataTable("OverRunTable");
            dt.Columns.Add("序号", Type.GetType("System.Int32"));
            dt.Columns.Add("里程/m", Type.GetType("System.Double"));
            dt.Columns.Add("超限量/mm", Type.GetType("System.Double"));
            int count = plan.Rows.Count <= range.Rows.Count ? plan.Rows.Count : range.Rows.Count;
            int maxIndex = 0;
            double maxValue = 0;

            if (datumTrack == 0) {
                int basicCol = plan.Columns.Count > 4 ? 5 : 1;
                int index = 1;
                for (int i = 0; i < count; i++) {
                    double positive = Convert.ToDouble(range.Rows[i][2]);
                    double negative = Convert.ToDouble(range.Rows[i][3]);
                    double num = Convert.ToDouble(plan.Rows[i][basicCol]);
                    if (num > positive) {
                        num = Math.Abs(num);
                        if (num >= maxValue) {
                            maxIndex = i;
                            maxValue = num;
                        }
                        DataRow dr = dt.NewRow();
                        dr["序号"] = index;
                        dr["里程/m"] = plan.Rows[i]["里程"];
                        dr["超限量/mm"] = Math.Round(num - positive, 3);
                        dt.Rows.Add(dr);
                        index++;
                    }
                    else if (num < negative) {
                        num = Math.Abs(num);
                        if (num >= maxValue) {
                            maxIndex = i;
                            maxValue = num;
                        }
                        DataRow dr = dt.NewRow();
                        dr["序号"] = index;
                        dr["里程/m"] = plan.Rows[i]["里程"];
                        dr["超限量/mm"] = Math.Round(num - negative, 3);
                        dt.Rows.Add(dr);
                        index++;
                    }
                }
            }
            else if (datumTrack == 1) {
                int index = 1;
                int basicCol = plan.Columns.Count > 4 ? 6 : 2;
                for (int i = 0; i < count; i++) {
                    double positive = Convert.ToDouble(range.Rows[i][2]);
                    double negative = Convert.ToDouble(range.Rows[i][3]);
                    double num = Convert.ToDouble(plan.Rows[i][basicCol]);
                    if (num > positive) {
                        num = Math.Abs(num);
                        if (num >= maxValue) {
                            maxIndex = i;
                            maxValue = num;
                        }
                        DataRow dr = dt.NewRow();
                        dr["序号"] = index;
                        dr["里程/m"] = plan.Rows[i]["里程"];
                        dr["超限量/mm"] = Math.Round(num - positive, 3);
                        dt.Rows.Add(dr);
                        index++;
                    }
                    else if (num < negative) {
                        num = Math.Abs(num);
                        if (num >= maxValue) {
                            maxIndex = i;
                            maxValue = num;
                        }
                        DataRow dr = dt.NewRow();
                        dr["序号"] = index;
                        dr["里程/m"] = plan.Rows[i]["里程"];
                        dr["超限量/mm"] = Math.Round(num - negative, 3);
                        dt.Rows.Add(dr);
                        index++;
                    }
                }
            }

            var dv = new DataView(dt);
            dv.Sort = "超限量/mm desc";
            return new Tuple<DataTable, int, double>(dv.ToTable(), maxIndex, maxValue);
        }
    }
}
