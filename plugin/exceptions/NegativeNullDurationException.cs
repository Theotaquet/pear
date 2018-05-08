using System;

namespace Pear {

    public class NegativeNullDurationException : Exception {

        private static string ExceptionMessage { get; } =
                "The session’s duration can't be negative or equal to zero. " +
                "Please repair your config.json.";

        public NegativeNullDurationException() : base(ExceptionMessage) {
        }
    }
}
