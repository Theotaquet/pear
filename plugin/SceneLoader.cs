using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pear {

    public class SceneLoader : MonoBehaviour {

        private static string NoSceneMessage { get; } =
                "No scene argument was specified or the scene was already the active one. " +
                "Pe.A.R. has been added to the default scene.";
        private static string WrongSceneMessage { get; }  =
                "The specified scene doesn’t exist. Please verify your command line arguments.";
        IEnumerator Start() {
            string sceneName = PearToolbox.GetArg("-scene");
            yield return LoadScene(sceneName);
        }

        private IEnumerator LoadScene(string sceneName) {
            if(sceneName == null || sceneName == SceneManager.GetActiveScene().name) {
                AddPearAnalyser();

                Debug.LogWarning(NoSceneMessage);
                PearToolbox.AddToLog(NoSceneMessage);
            }
            else if(Application.CanStreamedLevelBeLoaded(sceneName)) {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                AddPearAnalyser();

                yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            }
            else {
                Debug.LogError(WrongSceneMessage);
                PearToolbox.EndLogOnError(WrongSceneMessage);
            }
            yield return null;
        }

        private void AddPearAnalyser() {
            GameObject pear = new GameObject("Pe.A.R.");
            pear.AddComponent<PearAnalyser>();
        }
    }
}
