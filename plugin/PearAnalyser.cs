using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AssemblyCSharp {
    
    //game performance analyser, which launches recording sessions and sends them to a server
    public class PearAnalyser : MonoBehaviour {

        //the server URL on which to post the sessions
        private static readonly string PostMetricsURL = "localhost:3000/sessions";
        //the active session
        private Session session;

        //FRAMERATE
        //whether to calculate frame rate
        private static readonly bool FrameRateActivated = true;
        //the number of frame rates to calculate in one second
        private static readonly int UpdatesPerSecond = 4;
        //the frame counter since last frame rate calculation
        private int frameCounter;
        //the time counter since last frame rate calculation
        private float timeCounter;
        //the time counter since the beginning of the session
        private float globalTimeCounter;

        //OTHER METRICS
        //...

        void Start() {
            frameCounter = 0;
            timeCounter = 0.0f;
            globalTimeCounter = 0.0f;
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

        //calculates a framerate when required and records it in the active session
        private void CalculateFrameRate() {
            int frameRate;
            frameCounter++;
            timeCounter += Time.deltaTime;
            globalTimeCounter += timeCounter;
            //test if the limit of updates per second is respected
            if(timeCounter > 1.0f / UpdatesPerSecond) {
                //calculates the number of frames in 1 second
                frameRate = (int) (frameCounter / timeCounter);

                frameCounter = 0;
                //the overflow is kept in memory if timeCounter has exceeded updatesPerSecond
                timeCounter -= 1.0f / UpdatesPerSecond;

                Debug.Log("FPS: " + frameRate);

                //creates a new metric and adds it to the active session
                session.createMetric(new Metric("fps", frameRate, (uint) (globalTimeCounter * 1000)));
            }
        }

        //sends a POST request to the server url, with the JSON-formated active session
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
