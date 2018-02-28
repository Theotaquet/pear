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
            string sessionJSONString = JsonUtility.ToJson(session);
            string response = PostMetrics(sessionJSONString);
            WriteSession(response + "\n" + session);
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

        private string PostMetrics(string JSONString) {
            UnityWebRequest request =
                new UnityWebRequest(Configuration.ServerURL, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(JSONString);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest();

            string response;

            if(request.isNetworkError)
                response = request.error + "\n";
            else if(request.responseCode == 201)
                response = "Code 201: Post request complete!\n";
            else if(request.responseCode == 401) {
                response = "Error 401: Unauthorized: Resubmitted request!\n" +
                           PostMetrics(JSONString);
            }
            else response = "Request failed (status:" +
                            request.responseCode + ").\n";

            return response;
        }

        private static void WriteSession(string session) {
            StreamWriter writer =
                new StreamWriter(Configuration.SessionLogsPath, true);
            writer.WriteLine(session);
            writer.Close();
        }
    }
}
