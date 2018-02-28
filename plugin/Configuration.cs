using System;
using System.IO;
using UnityEngine;

namespace Pear {

    public class Configuration {

        private static readonly string ConfigFilePath = "Assets/pear/config.json";

        public static string ServerURL;
        public static string SessionLogsPath;

        public static bool FpsEnabled;
        public static float UpdateFrequency;

        public static void ReadConfigFile() {
            string JSONConfig = File.ReadAllText(ConfigFilePath);
            ConfigurationModel config =
                JsonUtility.FromJson<ConfigurationModel>(JSONConfig);
            ServerURL = config.serverConfiguration.serverURL;
            SessionLogsPath = config.serverConfiguration.sessionLogsPath;
            FpsEnabled = config.fpsConfiguration.fpsEnabled;
            UpdateFrequency = config.fpsConfiguration.updateFrequencyInMs / 1000;
        }
    }

    [Serializable]
    public class ConfigurationModel {

        public ServerConfigurationModel serverConfiguration;
        public FpsConfigurationModel fpsConfiguration;

    }

    [Serializable]
    public class ServerConfigurationModel {

        public string serverURL;
        public string sessionLogsPath;
    }

    [Serializable]
    public class FpsConfigurationModel {

        public bool fpsEnabled;
        public float updateFrequencyInMs;
    }
}
