using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

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
        }

        void OnDisable() {
            session.Duration = (uint) (lastFrameTime * 1000);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void CalculateFrameRate() {
            int frameRate;
            updateFrameCounter++;
            updateTimeCounter += Time.time - lastFrameTime;
            lastFrameTime = Time.time;

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

                    switch(request.responseCode) {
                        case 0:
                            response = noCode;
                            break;
                        case 201:
                            response = code201;
                            break;
                        case 401:
                            response = code401;
                            PostMetrics(JSONString);
                            break;
                        default:
                            response = otherCode + " (status:" + request.responseCode + ").";
                            break;
                    }

                    PearToolbox.AddToLog(response);
                    requestDone = true;
                }
            }
        }

        public IEnumerator SendRequest(UnityWebRequest request) {
            AsyncOperation async = request.SendWebRequest();
            yield return async;

            string response;
            if(request.isNetworkError)
                response = request.error + "";

            switch(request.responseCode) {
                case 0:
                    response = noCode;
                    break;
                case 201:
                    response = code201;
                    break;
                case 401:
                    response = code401;
                    StartCoroutine(SendRequest(request));
                    break;
                default:
                    response = otherCode + " (status:" + request.responseCode + ").";
                    break;
            }

            PearToolbox.AddToLog(response);
        }
    }
}
