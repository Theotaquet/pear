using System;

namespace Pear {

    public class NegativeNullUpdateFrequencyException : Exception {

        private static readonly string message =
                "The update frequency can't be negative or equal to zero. " +
                "Please repair your config.json.";

        public NegativeNullUpdateFrequencyException() : base(message) {
        }
    }
}
