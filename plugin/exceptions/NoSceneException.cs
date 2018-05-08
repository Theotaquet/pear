using System;

namespace Pear {

    public class NoSceneException : Exception {

        private static string ExceptionMessage { get; } =
                "No scene argument was specified or the scene was already the active one. " +
                "PeAR has been added to the default scene.";

        public NoSceneException() : base(ExceptionMessage) {
        }
    }
}
