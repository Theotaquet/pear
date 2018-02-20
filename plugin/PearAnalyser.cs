using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace Pear {
    
    public class PearAnalyser : MonoBehaviour {

        private Session session;

        private int updateFrameCounter;
        private float updateTimeCounter;
        private float lastFrameTime;

        void Start() {
            updateFrameCounter = 0;
            updateTimeCounter = 0.0f;
            lastFrameTime = 0.0f;
            session = new Session(Application.productName, Application.version,
                SceneManager.GetActiveScene().name);
        }
        
        void Update() {
            if(Configuration.FpsEnabled)
                CalculateFrameRate();
        }

        void OnDisable() {
            session.duration = (uint) (lastFrameTime * 1000);
            WriteSession(session);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
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
                //the overflow is kept in memory if updateTimeCounter has exceeded updatesPerSecond
                updateTimeCounter -= Configuration.UpdateFrequency;

                session.createMetric(new Metric("fps", frameRate, lastFrameTime));
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

        private static void WriteSession(Session session) {
            StreamWriter writer = new StreamWriter(Configuration.SessionLogsPath, true);
            writer.WriteLine(session);
            writer.Close();
        }
    }
}
