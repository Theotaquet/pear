using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Pear {

    public static class ConfigurationManager {

        public static string SessionLogsPath { get; } = "sessionLogs.txt";
        public static SessionConfiguration session { get; set; }
        public static MetricsManagerConfiguration[] metricsManagers { get; set; }

        private static string ConfigFilePath { get; } = "Assets/pear/config.json";

        public static void ReadConfigFile() {
            FileStream stream = File.OpenRead(ConfigFilePath);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration));
            Configuration config = (Configuration) ser.ReadObject(stream);

            config.CheckEmptyParameters();

            session = config.sessionConfiguration;
            metricsManagers = config.metricsManagersConfiguration;
        }
    }





    [DataContract]
    public class Configuration {

        [DataMember]
        public SessionConfiguration sessionConfiguration { get; set; }
        [DataMember]
        public MetricsManagerConfiguration[] metricsManagersConfiguration { get; set; }

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

    [DataContract]
    public class SessionConfiguration {

        [DataMember]
        public string apiServerUrl { get; set; }
        [DataMember]
        public int duration { get; set; }
    }

    [DataContract]
    public class MetricsManagerConfiguration {

        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string enabled { get; set; }
        [DataMember]
        public string updateFrequency { get; set; }
    }
}
