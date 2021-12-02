using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackRedesign {
     public class TagObject {
        public DataTable DtToDate { get; set; }
        public DataTable DtFroDate { get; set; }
        public DataTable  PPAdjustPlan { get; set; }
        public DataTable  ElevationAdjustPlan { get; set; }
        public DataTable  Model { get; set; }
        public string Path { get; set; }
        public DataTable Para { get; set; }
        public DataTable Export { get; set; }
        public int DatumTrack { get; set; }
    }
}
