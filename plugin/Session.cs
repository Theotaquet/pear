using System;
using System.Collections.Generic;

namespace Pear {

    [Serializable]
    public class Session {

        public string Game {get; set;}
        public string Build {get; set;}
        public string Scene {get; set;}
        public DateTime StartDate {get; set;}
        public uint Duration {get; set;}
        public List<Metric> Metrics {get; set;}

        public Session(string game, string build, string scene) {
            this.Game = game;
            this.Build = build;
            this.Scene = scene;
            StartDate = System.DateTime.Now;
            Duration = 0;
            Metrics = new List<Metric>();
        }

        public override string ToString() {
            string str = String.Format(
                    "{0} - version {1}\n" +
                    "Niveau: {2}\n" +
                    "{3} - {4} ms\n\n" +
                    "Metrics\n" +
                    "-------\n\n",
                    Game, Build, Scene, StartDate, Duration
            );

            if(Configuration.FpsEnabled) {
                str += "FPS:\n";
                foreach(Metric metric in Metrics) {
                    if(metric.Type == "fps")
                        str += metric.ToString() + "\n";
                }
            }

            return str.Remove(str.Length - 2, 2);
        }

        public bool createMetric(Metric metric) {
            if(!Metrics.Contains(metric)) {
                Metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool deleteMetric(Metric metric) {
            if(Metrics.Contains(metric))
                return Metrics.Remove(metric);
            return false;
        }
    }
}
