using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Pear {
    
    public class PearAnalyser : MonoBehaviour {

        private Session session;

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
            if(Configuration.FpsEnabled)
                CalculateFrameRate();
        }

        void OnDisable() {
            Debug.Log(session);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
        }

        private void CalculateFrameRate() {
            int frameRate;
            frameCounter++;
            timeCounter += Time.time - lastFrameTime;
            lastFrameTime = Time.time;

            //test if the limit of updates per second is respected
            while(timeCounter > Configuration.UpdateFrequency) {
                frameRate = (int) (frameCounter / timeCounter);

                frameCounter = 0;
                //the overflow is kept in memory if timeCounter has exceeded updatesPerSecond
                timeCounter -= Configuration.UpdateFrequency;

                Debug.Log("FPS: " + frameRate);

                session.createMetric(new Metric("fps", frameRate, (uint) (Time.time * 1000)));
            }
        }

        private void PostMetrics(string JSONString) {
            UnityWebRequest request = new UnityWebRequest(Configuration.ServerURL, "POST");
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
