using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Pear {

    [Serializable]
    public abstract class MetricsManager : ICollector {

        public string name;
        public bool enabled;
        public float updateFrequency;
        public List<Metric> metrics;

        protected float timer;

        public MetricsManager(MetricsManagerConfiguration metricsManagerConfig) {
            this.name = metricsManagerConfig.name;
            this.enabled = Boolean.Parse(metricsManagerConfig.enabled);
            SetUpdateFrequency(float.Parse(metricsManagerConfig.updateFrequency));
            this.metrics = new List<Metric>();

            timer = 0.0f;
        }

        public override string ToString() {
            string formatedName = new Regex(@"([A-Z]+)").Replace(name, "-$1").ToLower();
            string str = formatedName + " - update frequency: " + updateFrequency + " s\n";
			foreach(Metric metric in metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
        }

        public void CollectMetrics() {
            timer += Time.deltaTime;
            int metric;

            Update();

            //test if the limit of updates per second is respected
            while(timer > updateFrequency) {
                metric = CalculateMetric();
                CreateMetric(new Metric(metric, Time.time));

                //the overflow is kept in memory
                //if timer has exceeded updateFrequency
                timer -= updateFrequency;
            }
        }

        public virtual void Update() {

        }

        public abstract int CalculateMetric();

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
