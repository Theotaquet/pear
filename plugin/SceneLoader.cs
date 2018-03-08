using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pear {

    public class SceneLoader : MonoBehaviour {

        void Start() {
            string sceneName = PearToolbox.GetArg("-scene");
            StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName) {
            if(sceneName == null || sceneName == SceneManager.GetActiveScene().name) {
                AddPearAnalyser();
                try {
                    throw new NoSceneException();
                }
                catch(NoSceneException e) {
                    Debug.LogException(e);
                    PearToolbox.AddToLog(e.Message);
                }
            }
            else {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return asyncLoad;

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                AddPearAnalyser();
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            }
        }

        private void AddPearAnalyser() {
            GameObject pear = new GameObject("PeAR");
            pear.AddComponent<PearAnalyser>();
        }
    }
}
