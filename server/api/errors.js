function NotFound(message) {
    Error.captureStackTrace(this, this.constructor);

    this.name = this.constructor.name;
    this.message = message || 'The requested resource or page could not be found';
    this.statusCode = 404;
}

function BadGateway(message) {
    Error.captureStackTrace(this, this.constructor);

    this.name = this.constructor.name;
    this.message = message || 'The server was acting as a gateway or proxy' +
            'and received an invalid response from the upstream server';
    this.statusCode = 502;
}

module.exports.NotFound = NotFound;
module.exports.BadGateway = BadGateway;
