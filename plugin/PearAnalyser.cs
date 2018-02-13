using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Pear {
    
    public class PearAnalyser : MonoBehaviour {

        private static readonly string PostMetricsURL = "localhost:3000/sessions";
        private Session session;

        private static readonly bool FrameRateActivated = true;
        private static readonly int UpdatesPerSecond = 4;
        private int frameCounter;
        private float timeCounter;
        private float lastFrameTime;

        void Start() {
            frameCounter = 0;
            timeCounter = 0.0f;
            lastFrameTime = 0.0f;
            session = new Session();
        }
        
        void Update() {
            if(FrameRateActivated)
                CalculateFrameRate();
        }

        void OnDisable() {
            Debug.Log(session);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
        }

        private void CalculateFrameRate() {
            int frameRate;
            float updateFrequency = 1.0f / UpdatesPerSecond;
            frameCounter++;
            timeCounter += Time.time - lastFrameTime;
            lastFrameTime = Time.time;

            //test if the limit of updates per second is respected
            while(timeCounter > updateFrequency) {
                frameRate = (int) (frameCounter / timeCounter);

                frameCounter = 0;
                //the overflow is kept in memory if timeCounter has exceeded updatesPerSecond
                timeCounter -= updateFrequency;

                Debug.Log("FPS: " + frameRate);

                session.createMetric(new Metric("fps", frameRate, (uint) (Time.time * 1000)));
            }
        }

        private void PostMetrics(string JSONString) {
            UnityWebRequest request = new UnityWebRequest(PostMetricsURL, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(JSONString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.Send();

            if(request.isError)
                Debug.Log(request.error);
            else {
                Debug.Log("Response: " + request.downloadHandler.text);
                if(request.responseCode == 201)
                    Debug.Log("Post request complete!");
                else if(request.responseCode == 401) {
                    Debug.Log("Error 401: Unauthorized: Resubmitted request!");
                    PostMetrics(JSONString);
                }
                else Debug.Log("Request failed (status:" + request.responseCode + ").");
            }
        }
    }
}
