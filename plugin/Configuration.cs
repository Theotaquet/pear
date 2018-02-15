using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IniParser;
using IniParser.Model;

namespace Pear {

    public class Configuration : MonoBehaviour {

        public static string ServerURL;

        public static bool FpsEnabled;
        public static float UpdateFrequency;

        void Awake() {
            LoadConfig();
        }

        public static void LoadConfig() {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Assets/pear/config.ini");

            ServerURL = data["ServerConfiguration"]["serverURL"];

            FpsEnabled = bool.Parse(data["FpsConfiguration"]["fpsEnabled"]);
            UpdateFrequency = float.Parse(data["FpsConfiguration"]["updateFrequency"]) / 1000;
        }
    }
}
