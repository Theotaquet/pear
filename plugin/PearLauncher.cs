using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pear {

    public class PearLauncher {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ActivatePear() {
            if(GetArg("-pear") == "pear") {
                GameObject sceneLoader = new GameObject("SceneLoader");
                sceneLoader.AddComponent<SceneLoader>();
            }
        }

        public static string GetArg(string name) {
            string[] args = System.Environment.GetCommandLineArgs();
            for(int i = 0 ; i < args.Length ; i++) {
                if(args[i] == name) {
                    if(name == "-pear")
                        return "pear";
                    else if(args.Length > i + 1)
                        return args[i + 1];
                }
            }
            return null;
        }
    }
}
