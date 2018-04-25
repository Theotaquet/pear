using System;

namespace Pear {

    public class WrongConfigStructureException : Exception {

        private static readonly string message =
                "Your config.json is empty or isn't well-formed. " +
                "Please repair it.";

        public WrongConfigStructureException(Exception innerException) :
                base(innerException.Message + "\n" + message) {
        }
    }
}