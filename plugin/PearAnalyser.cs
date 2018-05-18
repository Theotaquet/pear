using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pear {

    public class PearAnalyser : MonoBehaviour {

        public Session session { get; set; }

        private static string NoCode { get; } =
                "Unable to connect to the server. " +
                "Check the URL or the server status.";
        private static string Code201 { get; } =
                "Code 201: Post request complete!";
        private static string Code502 { get; } =
                "Error 502: Bad Gateway. The server failed to connect to the database server. " +
                "Please check the database server status.";
        private static string OtherCode { get; } =
                "Request failed";
        private float duration { get; set; }
        private Texture pearTexture;

        void Start() {
            session = new Session(
                    Application.productName,
                    Application.version,
                    SceneManager.GetActiveScene().name,
                    Application.platform.ToString(),
                    Application.unityVersion,
                    SystemInfo.deviceName,
                    SystemInfo.processorType,
                    SystemInfo.systemMemorySize,
                    SystemInfo.graphicsDeviceName,
                    SystemInfo.graphicsMemorySize
            );

            pearTexture = Resources.Load("pear-logo") as Texture;
        }

        void OnGUI() {
            if(!pearTexture) {
                Debug.LogError("Assign a Texture in the inspector.");
                return;
            }
            GUI.DrawTexture(new Rect(Screen.width - 10 - 24, 10, 24, 37),
                    pearTexture, ScaleMode.ScaleToFit);
        }

        void Update() {
            foreach(MetricsManager metricsManager in session.metricsManagers) {
                if(metricsManager.enabled) {
                    metricsManager.CollectMetrics();
                }
            }

            duration = Time.time;
            if(duration >= ConfigurationManager.Session.duration) {
                Application.Quit();
            }
        }

        void OnDisable() {
            session.duration = Math.Min(ConfigurationManager.Session.duration, duration);

            string sessionJsonString = JsonConvert.SerializeObject(session);
            PostMetrics(sessionJsonString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void PostMetrics(string jsonString) {
            UnityWebRequest request =
                    new UnityWebRequest(ConfigurationManager.Session.apiServerUrl, "POST");
            byte[] bodyRaw = new UTF8Encoding().GetBytes(jsonString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");

            AsyncOperation async = request.SendWebRequest();

            bool requestDone = false;
            while(!requestDone) {
                if(async.isDone) {
                    string response;
                    if(request.isNetworkError) {
                        response = request.error + "";
                    }

                    var responses = new Dictionary<long, string> ();

                    responses.Add(0, NoCode);
                    responses.Add(201, Code201);
                    responses.Add(502, Code502);

                    string value;
                    if(responses.TryGetValue(request.responseCode, out value)) {
                        response = value;
                    }
                    else {
                        response = OtherCode + " (status:" + request.responseCode + ").";
                    }

                    PearToolbox.AddToLog(response);
                    requestDone = true;
                }
            }
        }
    }
}
