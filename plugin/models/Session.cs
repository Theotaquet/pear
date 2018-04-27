using System;
using System.Collections.Generic;

namespace Pear {

    [Serializable]
    public class Session {

        public string game;
        public string build;
        public string scene;
        public string platform;
        public string unityVersion;
        public string device;
        public string processorType;
        public int systemMemory;
        public string gpu;
        public int gpuMemory;
        public string startDate;
        public float duration;
        public List<MetricsManager> metricsManagers;

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
            SetDuration(ConfigurationManager.session.duration);
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

        public void SetDuration(float duration) {
            if(duration > 0) {
                this.duration = duration / 1000;
            }
            else {
                throw new NegativeNullDurationException();
            }
        }
    }
}
