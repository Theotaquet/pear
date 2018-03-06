using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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

        private int updateFrameCounter;
        private float updateTimeCounter;
        private float lastFrameTime;

        void Start() {
            updateFrameCounter = 0;
            updateTimeCounter = 0.0f;
            lastFrameTime = 0.0f;
            session = new Session(
                    Application.productName,
                    Application.version,
                    SceneManager.GetActiveScene().name
            );
        }

        void Update() {
            if(Configuration.FpsEnabled)
                CalculateFrameRate();
            lastFrameTime = Time.time;
        }

        void OnDisable() {
            session.duration = (uint) (lastFrameTime * 1000);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void CalculateFrameRate() {
            int frameRate;
            updateFrameCounter++;
            updateTimeCounter += Time.time - lastFrameTime;

            //test if the limit of updates per second is respected
            while(updateTimeCounter > Configuration.UpdateFrequency) {
                frameRate = (int) (updateFrameCounter / updateTimeCounter);

                updateFrameCounter = 0;
                //the overflow is kept in memory
                //if updateTimeCounter has exceeded updatesPerSecond
                updateTimeCounter -= Configuration.UpdateFrequency;

                session.createMetric(new Metric("fps", frameRate, lastFrameTime));
            }
        }

        private void PostMetrics(string JSONString) {
            UnityWebRequest request = new UnityWebRequest(Configuration.ServerURL, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(JSONString);
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
