using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Pear {

    public static class ConfigurationManager {

        public static string SessionLogsPath { get; } = "sessionLogs.txt";
        public static SessionConfiguration Session { get; set; }
        public static MetricsManagerConfiguration[] MetricsManagers { get; set; }

        private static string ConfigFilePath { get; } = "Assets/pear/config.json";

        public static void ReadConfigFile() {
            JsonSerializer ser = new JsonSerializer();
            JsonTextReader reader = new JsonTextReader(new StreamReader(File.OpenRead(ConfigFilePath)));
            Configuration config = ser.Deserialize<Configuration>(reader);
            config.CheckEmptyParameters();

            Session = config.sessionConfiguration;
            MetricsManagers = config.metricsManagersConfiguration;
        }
    }





    public class Configuration {

        public SessionConfiguration sessionConfiguration { get; set; }
        public MetricsManagerConfiguration[] metricsManagersConfiguration { get; set; }

        public void CheckEmptyParameters() {
            bool noParamValue = false;
            ArrayList emptyParameters = new ArrayList();

            if(sessionConfiguration.duration <= 0) {
                throw new NegativeNullDurationException();
            }
            if(HasEmptyParameters(sessionConfiguration, emptyParameters)) {
                noParamValue = true;
            }

            foreach(MetricsManagerConfiguration config in metricsManagersConfiguration) {
                if(config.updateFrequency <= 0) {
                    throw new NegativeNullUpdateFrequencyException();
                }
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

    public class SessionConfiguration {

        public string apiServerUrl { get; set; }
        public float duration { get; set; }
    }

    public class MetricsManagerConfiguration {

        public string name { get; set; }
        public bool enabled { get; set; }
        public float updateFrequency { get; set; }
    }
}
