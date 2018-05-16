using System;
using System.Collections.Generic;

namespace Pear {

    public class Session {

        public string game { get; set; }
        public string build { get; set; }
        public string scene { get; set; }
        public string platform { get; set; }
        public string unityVersion { get; set; }
        public string device { get; set; }
        public string processorType { get; set; }
        public int systemMemory { get; set; }
        public string gpu { get; set; }
        public int gpuMemory { get; set; }
        public string startDate { get; set; }
        public float duration { get; set; }
        public List<MetricsManager> metricsManagers { get; set; }

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
            startDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            metricsManagers = new List<MetricsManager>();

            foreach(MetricsManagerConfiguration metricsManagerConfig
                    in ConfigurationManager.MetricsManagers) {
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
                    "{10} - {11} s\n\n" +
                    "Metrics\n" +
                    "-------\n\n",
                    game, build, scene, platform, unityVersion, device, processorType,
                    systemMemory, gpu, gpuMemory, DateTime.Parse(startDate), duration
            );

            foreach(MetricsManager metricsManager in metricsManagers) {
                if(metricsManager.enabled) {
                    str += metricsManager.ToString() + "\n";
                }
            }

            return str.Remove(str.Length - 2, 2);
        }

        public bool AddMetricsManager(MetricsManager metricsManager) {
            if(!metricsManagers.Contains(metricsManager)) {
                metricsManagers.Add(metricsManager);
                return true;
            }
            return false;
        }

        public MetricsManager FindMetricsManager(string name) {
            return metricsManagers.Find(x => x.name == name);
        }

        public bool RemoveMetricsManager(MetricsManager metricsManager) {
            if(metricsManagers.Contains(metricsManager)) {
                return metricsManagers.Remove(metricsManager);
            }
            return false;
        }
    }
}
