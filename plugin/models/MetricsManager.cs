using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Pear {

    public abstract class MetricsManager : ICollector {

        public string name { get; set; }
        public bool enabled { get; set; }
        public float updateFrequency {
            get {
                return _updateFrequency;
            }
            set {
                if(value > 0) {
                    _updateFrequency = value;
                }
                else {
                    throw new NegativeNullUpdateFrequencyException();
                }
            }
        }
        public List<Metric> metrics { get; set; }

        protected float timer { get; set; }

        private float _updateFrequency;

        public MetricsManager(MetricsManagerConfiguration metricsManagerConfig) {
            name = metricsManagerConfig.name;
            enabled = metricsManagerConfig.enabled;
            updateFrequency = metricsManagerConfig.updateFrequency;
            metrics = new List<Metric>();

            timer = 0.0f;
        }

        public override string ToString() {
            string formatedName =
                    name.Substring(0, 1).ToUpper() +
                    new Regex(@"([A-Z0-9]+)").Replace(name.Substring(1), " $1").ToLower();
            string str = formatedName + " - update frequency: " + updateFrequency + " s\n";
            foreach(Metric metric in metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
        }

        public void CollectMetrics() {
            timer += Time.deltaTime;
            float metric;

            Update();

            //test if the limit of updates per second is respected
            while(timer >= updateFrequency) {
                metric = CalculateMetric();
                CreateMetric(new Metric(metric, Time.time));

                //the overflow is kept in memory
                //if timer has exceeded updateFrequency
                timer -= updateFrequency;
            }
        }

        public virtual void Update() {

        }

        public abstract float CalculateMetric();

        public bool CreateMetric(Metric metric) {
            if(!metrics.Contains(metric)) {
                metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool DeleteMetric(Metric metric) {
            if(metrics.Contains(metric)) {
                return metrics.Remove(metric);
            }
            return false;
        }
    }
}
