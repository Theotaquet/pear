﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Pear {

    public class PearAnalyser : MonoBehaviour {


        public Session session { get; set; }

		private static string NoCode { get; } =
			"Unable to connect to the server. " +
			"Check the URL or the server status.";
		private static string Code201 { get; } =
			"Code 201: Post request complete!";
		private static string Code401 { get; } =
			"Error 401: Unauthorized. Resubmitted request!";
		private static string OtherCode { get; } =
			"Request failed";
		private float duration { get; set; }

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

			duration = Time.time;
			if(duration >= session.duration) {
                Application.Quit();
            }
        }

        void OnDisable() {
			session.duration = (uint) Math.Min(session.duration, duration) * 1000;

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Session));
            ser.WriteObject(stream, session);
            stream.Position = 0;
            string sessionJsonString = new StreamReader(stream).ReadToEnd();

            PostMetrics(sessionJsonString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void PostMetrics(string jsonString) {
            UnityWebRequest request =
                    new UnityWebRequest(ConfigurationManager.session.apiServerUrl, "POST");
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
                    responses.Add(401, Code401);

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
