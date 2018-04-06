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
        public string GPU;
        public int GPUMemory;
        public string startDate;
        public float duration;
        public List<MetricsManager> metricsManagers;

        public Session(string game, string build, string scene, string platform,
                string unityVersion, string device, string processorType,
                int systemMemory, string GPU, int GPUMemory) {
            this.game = game;
            this.build = build;
            this.scene = scene;
            this.platform = platform;
            this.unityVersion = unityVersion;
            this.device = device;
            this.processorType = processorType;
            this.systemMemory = systemMemory;
            this.GPU = GPU;
            this.GPUMemory = GPUMemory;
            this.startDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            SetDuration(ConfigurationManager.session.duration);
            this.metricsManagers = new List<MetricsManager>();

            foreach(MetricsManagerConfiguration metricsManager in ConfigurationManager.metricsManagers) {
                CreateMetricsManager(new MetricsManager(metricsManager));
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
                    systemMemory, GPU, GPUMemory, DateTime.Parse(startDate), duration
            );

            foreach(MetricsManager metricManager in metricsManagers) {
                if(metricManager.enabled) {
                    str += metricManager.ToString() + "\n";
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

        public void SetDuration(float duration) {
            if(duration > 0)
                    this.duration = duration / 1000;
            else
                throw new NegativeNullDurationException();
        }
    }
}
