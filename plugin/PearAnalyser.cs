using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Pear {

    public class PearAnalyser : MonoBehaviour {

        private static readonly string noCode =
                "Unable to connect to the server. " +
                "Check the URL or the server status.";
        private static readonly string code201 =
                "Code 201: Post request complete!";
        private static readonly string code401 =
                "Error 401: Unauthorized. Resubmitted request!";
        private static readonly string otherCode =
                "Request failed";

        private Session session;

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
        }

        void Update() {
            foreach(MetricsManager metricsManager in session.metricsManagers) {
                if(metricsManager.enabled) {
                    metricsManager.CollectMetrics();
                }
            }

            if(Time.time >= session.duration) {
                Application.Quit();
            }
        }

        void OnDisable() {
            session.duration = (uint) Math.Min(session.duration, Time.time) * 1000;
            string sessionJsonString = JsonUtility.ToJson(session);
            PostMetrics(sessionJsonString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void PostMetrics(string jsonString) {
            UnityWebRequest request = new UnityWebRequest(ConfigurationManager.session.apiServerUrl, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");

            AsyncOperation async = request.SendWebRequest();

            bool requestDone = false;
            while(!requestDone) {
                if(async.isDone) {
                    string response;
                    if(request.isNetworkError)
                        response = request.error + "";

                    var responses = new Dictionary<long, string> ();
                    responses.Add(0, noCode);
                    responses.Add(201, code201);
                    responses.Add(401, code401);

                    string value;
                    if(responses.TryGetValue(request.responseCode, out value))
                        response = value;
                    else
                        response = otherCode + " (status:" + request.responseCode + ").";

                    PearToolbox.AddToLog(response);
                    requestDone = true;
                }
            }
        }
    }
}
