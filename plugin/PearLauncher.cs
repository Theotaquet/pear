using System;
using UnityEngine;

namespace Pear {

    public class PearLauncher {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ActivatePear() {
            Configuration.ReadConfigFile();

            if(HasArg("-pear")) {
                GameObject sceneLoader = new GameObject("SceneLoader");
                sceneLoader.AddComponent<SceneLoader>();
            }
        }
        public static string GetArg(string name) {
            string[] args = System.Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, name);
            if(index > -1 && args.Length > index + 1)
                return args[index + 1];
            return null;
        }

        public static bool HasArg(string name) {
            string[] args = System.Environment.GetCommandLineArgs();
            name.Replace(".unity", string.Empty);
            var index = Array.IndexOf(args, name);
            return index > -1;
        }
    }
}
