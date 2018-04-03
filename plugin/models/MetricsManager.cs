using System;
using System.Collections.Generic;

namespace Pear {

    [Serializable]
    public class MetricsManager {

        public string name;
        public bool enabled;
        public float updateFrequency;
        public List<Metric> metrics;

        public MetricsManager(string name, bool enabled, float updateFrequency) {
            this.name = name;
            this.enabled = enabled;
            SetUpdateFrequency(updateFrequency);
            this.metrics = new List<Metric>();
        }

        public MetricsManager(MetricsManagerConfiguration metricsManager) {
            this.name = metricsManager.name;
            this.enabled = Boolean.Parse(metricsManager.enabled);
            SetUpdateFrequency(float.Parse(metricsManager.updateFrequency));
            this.metrics = new List<Metric>();
        }

        public override string ToString() {
            string str = name + " - update frequency: " + updateFrequency + " ms\n";
			foreach(Metric metric in metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
        }

        public bool CreateMetric(Metric metric) {
            if(!metrics.Contains(metric)) {
                metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool DeleteMetric(Metric metric) {
            if(metrics.Contains(metric))
                return metrics.Remove(metric);
            return false;
        }

        public void SetUpdateFrequency(float updateFrequency) {
            if(updateFrequency > 0)
                    this.updateFrequency = updateFrequency / 1000;
            else
                throw new NegativeNullUpdateFrequencyException();
        }
    }
}
