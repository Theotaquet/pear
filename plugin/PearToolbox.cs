using System;
using System.IO;
using UnityEngine;

namespace Pear {

    public static class PearToolbox {

        private static readonly string StartMessage =
                DateTime.Now + " - PeAR activated in " +
                (Application.isEditor ? "editor" : "build") + " mode.\n" +
                "You can find the full console output " +
                "in the default Unity log files folder.";
        private static readonly string StopMessage =
                "PeAR hasn't been initialised.";
        private static bool LoggedSession = false;
        private static string Log = "";

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
                        if(HasArg("-log"))
                            LoggedSession = true;
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
            if(index > -1 && args.Length > index + 1)
                return args[index + 1];
            return null;
        }

        public static bool HasArg(string name) {
            string[] args = Environment.GetCommandLineArgs();
            name.Replace(".unity", string.Empty);
            int index = Array.IndexOf(args, name);
            return index > -1;
        }

        public static void AddToLog(string info) {
            Log += info + "\n\n";
        }

        public static void WriteLogInFile() {
            if(LoggedSession) {
                StreamWriter writer = new StreamWriter(ConfigurationManager.SessionLogsPath, true);
                writer.WriteLine(Log + "--------------------\n");
                writer.Close();
            }
        }
    }
}
