using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using UnityEngine;

namespace Pear {

    [DataContract]
    [KnownType(typeof(FrameRateManager))]
    [KnownType(typeof(GarbageCollectionManager))]
    public abstract class MetricsManager : ICollector {

        [DataMember]
        public string name { get; set; }
        [DataMember]
        public bool enabled { get; set; }
        [DataMember]
        public float updateFrequency {
            get {
                return _updateFrequency;
            }
            set {
                if(value > 0) {
                    _updateFrequency = value / 1000;
                }
                else {
                    throw new NegativeNullUpdateFrequencyException();
                }
            }
        }
        [DataMember]
        public List<Metric> metrics { get; set; }

        protected float timer { get; set; }

        private float _updateFrequency;

        public MetricsManager(MetricsManagerConfiguration metricsManagerConfig) {
            this.name = metricsManagerConfig.name;
            this.enabled = Boolean.Parse(metricsManagerConfig.enabled);
            this.updateFrequency = float.Parse(metricsManagerConfig.updateFrequency);
            this.metrics = new List<Metric>();

            this.timer = 0.0f;
        }

        public override string ToString() {
            string formatedName =
                    this.name.Substring(0, 1).ToUpper() +
                    new Regex(@"([A-Z]+)").Replace(this.name.Substring(1), " $1");
            string str = formatedName + " - update frequency: " + this.updateFrequency + " s\n";
			foreach(Metric metric in this.metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
        }

        public void CollectMetrics() {
            this.timer += Time.deltaTime;
            int metric;

            Update();

            //test if the limit of updates per second is respected
            while(this.timer >= this.updateFrequency) {
                metric = CalculateMetric();
                CreateMetric(new Metric(metric, Time.time));

                //the overflow is kept in memory
                //if timer has exceeded updateFrequency
                this.timer -= this.updateFrequency;
            }
        }

        public virtual void Update() {

        }

        public abstract int CalculateMetric();

        public bool CreateMetric(Metric metric) {
            if(!this.metrics.Contains(metric)) {
                this.metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool DeleteMetric(Metric metric) {
            if(this.metrics.Contains(metric)) {
                return this.metrics.Remove(metric);
            }
            return false;
        }
    }
}
