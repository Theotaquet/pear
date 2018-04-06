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
        private float lastFrameTime;

        private MetricsManager frameRatesManager;
        private int framesCounter;
        private float frameRateTimer;

        private MetricsManager garbageCollectionManager;
        private float GCTimer;

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
            lastFrameTime = 0.0f;

            frameRatesManager = session.ReadMetricsManager("Frame rate");
            framesCounter = 0;
            frameRateTimer = 0.0f;

            garbageCollectionManager = session.ReadMetricsManager("Garbage collection");
            GCTimer = 0.0f;
        }

        void Update() {
            if(frameRatesManager.enabled)
                CollectFrameRate();

            if(garbageCollectionManager.enabled)
                CollectGarbageCollection();

            lastFrameTime = Time.time;

            if(lastFrameTime >= session.duration) {
                lastFrameTime = session.duration;
                Application.Quit();
            }
        }

        void OnDisable() {
            session.duration = (uint) (lastFrameTime * 1000);
            string sessionJSONString = JsonUtility.ToJson(session);
            PostMetrics(sessionJSONString);
            PearToolbox.AddToLog(session.ToString());
            PearToolbox.WriteLogInFile();
        }

        private void CollectFrameRate() {
            int frameRate;
            framesCounter++;
            frameRateTimer += Time.time - lastFrameTime;

            //test if the limit of updates per second is respected
            while(frameRateTimer > frameRatesManager.updateFrequency) {
                frameRate = (int) (framesCounter / frameRateTimer);

                framesCounter = 0;
                //the overflow is kept in memory
                //if frameRateTimer has exceeded updateFrequency
                frameRateTimer -= frameRatesManager.updateFrequency;

                frameRatesManager.CreateMetric(new Metric(frameRate, lastFrameTime));
            }
        }

        private void CollectGarbageCollection() {
            int count;
            GCTimer += Time.time - lastFrameTime;

            while(GCTimer > garbageCollectionManager.updateFrequency) {
                count = GC.CollectionCount(0);

                GCTimer -= garbageCollectionManager.updateFrequency;

                garbageCollectionManager.CreateMetric(new Metric(count, lastFrameTime));
            }
        }

        private void PostMetrics(string JSONString) {
            UnityWebRequest request = new UnityWebRequest(ConfigurationManager.session.APIServerURL, "POST");
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
