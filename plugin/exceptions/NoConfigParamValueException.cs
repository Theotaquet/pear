using System;

namespace Pear {

    public class NoConfigParamValueException : Exception {

        private static readonly string message =
                "No value was specified for one of the configuration parameters. " +
                "Please repair your config.json: ";

        public NoConfigParamValueException(string configParams) : base(message + configParams) {
        }
    }
}
