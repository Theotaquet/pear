function NotFound(message) {
    Error.captureStackTrace(this, this.constructor);

    this.name = this.constructor.name;
    this.message = message || 'The requested page could not be found';
    this.statusCode = 404;
}

module.exports.NotFound = NotFound;
