using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pear {

    [DataContract]
    public class Session {

        [DataMember]
        public string game { get; set; }
        [DataMember]
        public string build { get; set; }
        [DataMember]
        public string scene { get; set; }
        [DataMember]
        public string platform { get; set; }
        [DataMember]
        public string unityVersion { get; set; }
        [DataMember]
        public string device { get; set; }
        [DataMember]
        public string processorType { get; set; }
        [DataMember]
        public int systemMemory { get; set; }
        [DataMember]
        public string gpu { get; set; }
        [DataMember]
        public int gpuMemory { get; set; }
        [DataMember]
        public string startDate { get; set; }
        [DataMember]
        public float duration {
            get {
                return _duration;
            }
            set {
                if(value > 0) {
                    _duration = value / 1000;
                }
                else {
                    throw new NegativeNullDurationException();
                }
            }
        }
        [DataMember]
        public List<MetricsManager> metricsManagers { get; set; }

		private float _duration;

        public Session(string game, string build, string scene, string platform,
                string unityVersion, string device, string processorType,
                int systemMemory, string gpu, int gpuMemory) {
            this.game = game;
            this.build = build;
            this.scene = scene;
            this.platform = platform;
            this.unityVersion = unityVersion;
            this.device = device;
            this.processorType = processorType;
            this.systemMemory = systemMemory;
            this.gpu = gpu;
            this.gpuMemory = gpuMemory;
            this.startDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            this.duration = ConfigurationManager.session.duration;
            this.metricsManagers = new List<MetricsManager>();

            foreach(MetricsManagerConfiguration metricsManagerConfig
                    in ConfigurationManager.metricsManagers) {
                string metricsManagerName =
                        metricsManagerConfig.name.Substring(0, 1).ToUpper() +
                        metricsManagerConfig.name.Substring(1) +
                        "Manager";
                Type metricsManagerType =
                        Type.GetType("Pear." + metricsManagerName + ", Assembly-CSharp");
                MetricsManager metricsManager =
                        (MetricsManager) Activator
                        .CreateInstance(metricsManagerType, metricsManagerConfig);
                AddMetricsManager(metricsManager);
            }
        }

        public override string ToString() {
            string str = String.Format(
                    "{0} - version {1}\n" +
                    "Level: {2}\n" +
                    "Running on {3} - with Unity version {4}\n" +
                    "Device: {5} {6} {7} MB\n" +
                    "GPU: {8} {9} MB\n" +
                    "{10} - {11} ms\n\n" +
                    "Metrics\n" +
                    "-------\n\n",
                    this.game, this.build, this.scene, this.platform, this.unityVersion, this.device, this.processorType,
                    this.systemMemory, this.gpu, this.gpuMemory, DateTime.Parse(this.startDate), this.duration
            );

            foreach(MetricsManager metricsManager in this.metricsManagers) {
                if(metricsManager.enabled) {
                    str += metricsManager.ToString() + "\n";
                }
            }

            return str.Remove(str.Length - 2, 2);
        }

        public bool AddMetricsManager(MetricsManager metricsManager) {
            if(!this.metricsManagers.Contains(metricsManager)) {
                this.metricsManagers.Add(metricsManager);
                return true;
            }
            return false;
        }

        public MetricsManager FindMetricsManager(string name) {
            return this.metricsManagers.Find(x => x.name == name);
        }

        public bool RemoveMetricsManager(MetricsManager metricsManager) {
            if(this.metricsManagers.Contains(metricsManager)) {
                return this.metricsManagers.Remove(metricsManager);
            }
            return false;
        }
    }
}
