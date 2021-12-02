using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackRedesign {
    public class Fractional {
        /// <summary>
        /// 分子
        /// </summary>
        public decimal Member { get; }
        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator { get; }

        private Fractional() { }

        /// <summary>
        /// 初始化对象的同时,计算出小数的分子和分母
        /// </summary>
        /// <param name="num"></param>
        public Fractional(decimal num) {
            decimal denominator = 1;
            decimal member = 0;
            while (num != (int)num) {
                denominator *= 10;
                num *= 10;
            }
            decimal t = GCD(denominator, num);
            member = num;
            member /= t;
            denominator /= t;
            this.Member = member;
            this.Denominator = denominator;
        }

        /// <summary>
        /// 计算两个数的最大公约数
        /// </summary>
        private decimal GCD(decimal a, decimal b) {
            if (a % b == 0) return b;
            return GCD(b, a % b);
        }
    }
}
