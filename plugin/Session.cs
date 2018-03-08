using System;
using System.Collections.Generic;

namespace Pear {

    [Serializable]
    public class Session {

        public string game;
        public string build;
        public string scene;
        public string startDate;
        public uint duration;
        public List<Metric> metrics;

        public Session(string game, string build, string scene) {
            this.game = game;
            this.build = build;
            this.scene = scene;
            startDate = System.DateTime.Now.ToString();
            duration = 0;
            metrics = new List<Metric>();
        }

        public override string ToString() {
            string str = String.Format(
                    "{0} - version {1}\n" +
                    "Niveau: {2}\n" +
                    "{3} - {4} ms\n\n" +
                    "Metrics\n" +
                    "-------\n\n",
                    game, build, scene, startDate, duration
            );

            if(Configuration.FpsEnabled) {
                str += "FPS:\n";
                foreach(Metric metric in metrics) {
                    if(metric.type == "fps")
                        str += metric.ToString() + "\n";
                }
            }

            return str.Remove(str.Length - 2, 2);
        }

        public bool createMetric(Metric metric) {
            if(!metrics.Contains(metric)) {
                metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool deleteMetric(Metric metric) {
            if(metrics.Contains(metric))
                return metrics.Remove(metric);
            return false;
        }
    }
}
