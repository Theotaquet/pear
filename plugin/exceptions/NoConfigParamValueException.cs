using System;
using System.Collections;

namespace Pear {

    public class NoConfigParamValueException : Exception {

        private static readonly string message =
                "No value was specified for a configuration parameter. " +
                "Please repair your config.json: ";

        public NoConfigParamValueException(string configParams) : base(message + configParams) {
        }
    }
}
