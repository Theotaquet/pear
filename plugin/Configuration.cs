using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Pear {

    public static class Configuration {

        private static readonly string ConfigFilePath = "Assets/pear/config.json";
        public static readonly string SessionLogsPath = "sessionLogs.txt";
        public static string ServerURL {get; set;}

        public static bool FpsEnabled {get; set;}
        public static float UpdateFrequency {get; set;}

        public static void ReadConfigFile() {
            string JSONConfig = File.ReadAllText(ConfigFilePath);
            ConfigurationModel config = JsonUtility.FromJson<ConfigurationModel>(JSONConfig);

            bool noParamValue = false;
            ArrayList emptyParameters = new ArrayList();
            foreach(FieldInfo field in config.GetType().GetFields()) {
                foreach(FieldInfo subField in field.FieldType.GetFields()) {
                    if(subField.GetValue(field.GetValue(config)) == null) {
                        noParamValue = true;
                        emptyParameters.Add(
                            subField.Name.Substring(0, 1).ToLower() + subField.Name.Substring(1));
                    }
                }
            }
            if(noParamValue) {
                string str = "";
                int i;
                for(i = 0 ; i < emptyParameters.Count - 1 ; i++) {
                    str += emptyParameters[i] + ", ";
                }
                str += emptyParameters[i];
                throw new NoConfigParamValueException(str);
            }

            ServerURL = config.serverConfiguration.serverURL;
            FpsEnabled = Boolean.Parse(config.fpsConfiguration.fpsEnabled);
            UpdateFrequency = float.Parse(config.fpsConfiguration.updateFrequencyInMs);
            if(UpdateFrequency > 0)
                UpdateFrequency /= 1000;
            else
                throw new NegativeNullUpdateFrequencyException();
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
    }

    [Serializable]
    public class FpsConfigurationModel {

        public string fpsEnabled;
        public string updateFrequencyInMs;
    }
}
