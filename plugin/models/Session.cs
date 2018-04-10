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
        public List<MetricsManager> metricsManagers;

        public Session(string game, string build, string scene) {
            this.game = game;
            this.build = build;
            this.scene = scene;
            this.startDate = System.DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss");
            this.duration = 0;

            this.metricsManagers = new List<MetricsManager>();

            foreach(MetricsManagerConfiguration metricsManager in ConfigurationManager.metricsManagers) {
                CreateMetricsManager(new MetricsManager(metricsManager));
            }
        }

        public override string ToString() {
            string str = String.Format(
                    "{0} - version {1}\n" +
                    "Niveau: {2}\n" +
                    "{3} - {4} ms\n\n" +
                    "Metrics\n" +
                    "-------\n\n",
                    game, build, scene, DateTime.Parse(startDate), duration
            );

            foreach(MetricsManager metricManager in metricsManagers) {
                if(metricManager.enabled) {
                    str += metricManager.ToString();
                }
            }

            return str.Remove(str.Length - 2, 2);
        }

        public bool CreateMetricsManager(MetricsManager metricManager) {
            if(!metricsManagers.Contains(metricManager)) {
                metricsManagers.Add(metricManager);
                return true;
            }
            return false;
        }

        public MetricsManager ReadMetricsManager(string name) {
            return metricsManagers.Find(x => x.name == name);
        }

        public bool DeleteMetricManager(MetricsManager metricManager) {
            if(metricsManagers.Contains(metricManager))
                return metricsManagers.Remove(metricManager);
            return false;
        }
    }
}
