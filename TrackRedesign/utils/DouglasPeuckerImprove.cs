using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackRedesign.utils {
    public class DouglasPeuckerImprove {
        private double _errorBound; // degree
        private List<Point> zhedian;
        // 构造函数
        public DouglasPeuckerImprove(double errorBound) {
            zhedian = new List<Point>();
            _errorBound = errorBound;
        }



        private List<Point> CompressHelper(List<Point> pointsList) {
            if (pointsList.Count < 2) {
                return pointsList;
            }

            List<Point> result = new List<Point>();

            // 有可能是polygon
            if (pointsList.First().Equals(pointsList.Last())) {
                var r1 = CompressHelper(pointsList.GetRange(0, pointsList.Count / 2));
                var r2 = CompressHelper(pointsList.GetRange(pointsList.Count / 2, pointsList.Count - pointsList.Count / 2));
                result.AddRange(r1);
                result.AddRange(r2);
                return result;
            }

            Line line = new Line() { p1 = pointsList.First(), p2 = pointsList.Last() };

            double maxDistance = 0;
            int maxIndex = 0;

            for (int i = 1; i < pointsList.Count - 1; i++) {
                var distance = Distance(pointsList[i], line);
                if (distance > maxDistance) {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxDistance <= _errorBound) {
                result.Add(pointsList.First());
            }
            else {
                var r1 = CompressHelper(pointsList.GetRange(0, maxIndex));
                var r2 = CompressHelper(pointsList.GetRange(maxIndex + 1, pointsList.Count - maxIndex - 1));
                result.AddRange(r1);
                result.Add(pointsList[maxIndex]);
                result.AddRange(r2);
            }

            return result;
        }

        private double Distance(Point p, Line line) {
            var p1 = line.p1;
            var p2 = line.p2;
            return Math.Abs(
                    ((p2.Lng - p1.Lng) * p.Lat + (p1.Lat - p2.Lat) * p.Lng + (p1.Lng - p2.Lng) * p1.Lat + (p2.Lat - p1.Lat) * p1.Lng) /
                    Math.Sqrt((p2.Lng - p1.Lng) * (p2.Lng - p1.Lng) + (p1.Lat - p2.Lat) * (p1.Lat - p2.Lat))
                );
        }
    }
}
