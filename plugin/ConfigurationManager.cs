using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Pear {

    public static class ConfigurationManager {

        private static readonly string ConfigFilePath = "Assets/pear/config.json";
        public static readonly string SessionLogsPath = "sessionLogs.txt";

        public static SessionConfiguration session;
        public static MetricsManagerConfiguration[] metricsManagers;

        public static void ReadConfigFile() {
            string rawConfig = File.ReadAllText(ConfigFilePath);
            Configuration config = JsonUtility.FromJson<Configuration>(rawConfig);

            config.CheckEmptyParameters();

            session = config.sessionConfiguration;
            metricsManagers = config.metricsManagersConfiguration;
        }
    }





    [Serializable]
    public class Configuration {

        public SessionConfiguration sessionConfiguration;
        public MetricsManagerConfiguration[] metricsManagersConfiguration;

        public void CheckEmptyParameters() {
            bool noParamValue = false;
            ArrayList emptyParameters = new ArrayList();

            if(HasEmptyParameters(sessionConfiguration, emptyParameters)) {
                noParamValue = true;
            }

            foreach(MetricsManagerConfiguration config in metricsManagersConfiguration) {
                if(HasEmptyParameters(config, emptyParameters)) {
                    noParamValue = true;
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
        }

        private bool HasEmptyParameters(object obj, ArrayList emptyParameters) {
            foreach(FieldInfo field in obj.GetType().GetFields()) {
                if(field.GetValue(obj) == null) {
                    emptyParameters.Add(
                        field.Name.Substring(0, 1).ToLower() + field.Name.Substring(1));
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class SessionConfiguration {

        public string apiServerUrl;
        public int duration;
    }

    [Serializable]
    public class MetricsManagerConfiguration {

        public string name;
        public string enabled;
        public string updateFrequency;
    }
}
