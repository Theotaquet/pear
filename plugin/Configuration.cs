using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IniParser;
using IniParser.Model;

namespace Pear {

    public class Configuration : MonoBehaviour {

        public static readonly string ServerURL;
        public static readonly string SessionLogsPath;

        public static readonly bool FpsEnabled;
        public static readonly float UpdateFrequency;

        static Configuration() {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Assets/pear/config.ini");

            ServerURL = data["ServerConfiguration"]["serverURL"];
            SessionLogsPath = data["ServerConfiguration"]["sessionLogsPath"];

            FpsEnabled = bool.Parse(data["FpsConfiguration"]["fpsEnabled"]);
            UpdateFrequency = float.Parse(data["FpsConfiguration"]["updateFrequencyInMs"]) / 1000;
        }
    }
}
