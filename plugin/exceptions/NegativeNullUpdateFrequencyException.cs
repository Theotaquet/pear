using System;

namespace Pear {

    public class NegativeNullUpdateFrequencyException : Exception {

        private static string ExceptionMessage { get; } =
                "The update frequency can't be negative or equal to zero. " +
                "Please repair your config.json.";

        public NegativeNullUpdateFrequencyException() : base(ExceptionMessage) {
        }
    }
}
