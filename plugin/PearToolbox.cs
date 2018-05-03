using System;
using System.IO;
using UnityEngine;

namespace Pear {

    public static class PearToolbox {

        private static string StartMessage { get; } =
                DateTime.Now + " - Pe.A.R. activated in " +
                (Application.isEditor ? "editor" : "build") + " mode.\n" +
                "You can find the full console output " +
                "in the default Unity log files folder.";
        private static string StopMessage { get; } =
                "Pe.A.R. hasn't been initialised.";
        private static bool loggedSession { get; set; } = false;
        private static string log { get; set; } = "";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ActivatePear() {
            Action<Exception> criticalError = (e) => {
                Debug.LogException(e);
                AddToLog(e.Message + "\n\n" + StopMessage);
                PearToolbox.WriteLogInFile();
            };
            try {
                try {
                    if(HasArg("-pear")) {
                        if(HasArg("-log")) {
                            loggedSession = true;
                        }
                        AddToLog(StartMessage);
                        ConfigurationManager.ReadConfigFile();
                        GameObject sceneLoader = new GameObject("SceneLoader");
                        sceneLoader.AddComponent<SceneLoader>();
                    }
                }
                catch(NoConfigParamValueException e) {
                    criticalError(e);
                }
                catch(NegativeNullUpdateFrequencyException e) {
                    criticalError(e);
                }
                catch(NegativeNullDurationException e) {
                    criticalError(e);
                }
                catch(ArgumentException e) {
                    throw new WrongConfigStructureException(e);
                }
                catch(NullReferenceException e) {
                    throw new WrongConfigStructureException(e);
                }
            }
            catch(WrongConfigStructureException e) {
                criticalError(e);
            }
        }

        public static string GetArg(string name) {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            if(index > -1 && args.Length > index + 1) {
                return args[index + 1];
            }
            return null;
        }

        public static bool HasArg(string name) {
            string[] args = Environment.GetCommandLineArgs();
            name.Replace(".unity", string.Empty);
            int index = Array.IndexOf(args, name);
            return index > -1;
        }

        public static void AddToLog(string info) {
            log += info + "\n\n";
        }

        public static void WriteLogInFile() {
            if(loggedSession) {
                StreamWriter writer = new StreamWriter(ConfigurationManager.SessionLogsPath, true);
                writer.WriteLine(log + "--------------------\n");
                writer.Close();
            }
        }
    }
}
