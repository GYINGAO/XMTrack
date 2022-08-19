using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TrackRedesign.model;

namespace TrackRedesign {
    public class CalcChord {

        public static ArrayList calc_300m(DataTable plan) {
            List<Deviation> dev = new List<Deviation>();
            for (int i = 0; i < plan.Rows.Count; i++) {
                Deviation deviation = new Deviation();
                deviation.mile = Convert.ToDouble(plan.Rows[i]["里程/m"]);
                deviation.plane = Convert.ToDouble(plan.Rows[i]["调后平面/mm"]);
                deviation.elevation = Convert.ToDouble(plan.Rows[i]["调后高程/mm"]);
                dev.Add(deviation);
            }
            ArrayList res = new ArrayList();
            int start = 0;
            while (start + 480 < dev.Count) {
                for (int i = start + 1; i <= start + 239; i++) {
                    var value = dev[i].plane - dev[i + 240].plane + (dev[start + 480].plane - dev[start].plane) / 2;
                    res.Add(value);
                }
                start += 239;
            }
            return res;
        }
    }
}
