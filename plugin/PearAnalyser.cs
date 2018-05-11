using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;

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

        void Start() {
            try {
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
            }
            catch(NegativeNullDurationException e) {
                PearToolbox.CriticalError(e);
                Destroy(gameObject);
            }
            catch(NegativeNullUpdateFrequencyException e) {
                PearToolbox.CriticalError(e);
                Destroy(gameObject);
            }
        }

        void Update() {
            foreach(MetricsManager metricsManager in session.metricsManagers) {
                if(metricsManager.enabled) {
                    metricsManager.CollectMetrics();
                }
            }

            duration = Time.time;
            if(duration >= session.duration) {
                Application.Quit();
            }
        }

        void OnDisable() {
            session.duration = (uint) Math.Min(session.duration, duration) * 1000;

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
