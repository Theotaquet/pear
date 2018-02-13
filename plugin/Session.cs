using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pear {

    [Serializable]
    public class Session {

        public string build;
        public string game;
        public string scene;
        public DateTime startDate;
        public uint duration;
        public List<Metric> metrics;

        public Session() {
            build = "buildVersion";
            game = "gameName";
            scene = "sceneName";
            startDate = System.DateTime.Now;
            duration = 0;
            metrics = new List<Metric>();
        }

        public override string ToString() {
            string str = build + " - " + game + " - " + scene + "\n" +
                         startDate + " - " + duration + "\n" +
                         "Metrics:\n";
            foreach(Metric metric in metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
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
