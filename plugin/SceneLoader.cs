using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pear {

    public class SceneLoader : MonoBehaviour {

        void Start() {
            string sceneName = PearLauncher.GetArg("-scene");
            if(sceneName == null)
                AddPearAnalyser();
            else
                StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!asyncLoad.isDone) {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            AddPearAnalyser();
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        }

        private void AddPearAnalyser() {
            GameObject pear = new GameObject("PeAR");
            pear.AddComponent<PearAnalyser>();
        }
    }
}
