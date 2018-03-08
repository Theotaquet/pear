using System;

namespace Pear {

    public class NoSceneException : Exception {

        private static readonly string message =
                "No scene argument was specified or the scene was already the active one. " +
                "PeAR has been added to the default scene.";

        public NoSceneException() : base(message) {
        }
    }
}