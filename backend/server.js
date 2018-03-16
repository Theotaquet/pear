const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const sessionRouter = require('./routers/session-router');
const NotFound = require('./errors').NotFound;

const port = process.env.PORT || 3000;

app

.use(bodyParser.json())

.get('/', function(req, res, next) {
    res.set('Content-Type', 'text/plain');

    res.status(200).end('Hello World!');
})

.use('/sessions', sessionRouter)

.use('*', function(req, res, next) {
    next(new NotFound());
})

.use(function(err, req, res, next) {
    console.error(err);
    res.status(err.status || 500).json(err);
})

.listen(port, function() {
    console.log(`Pe.A.R. RESTful API server started on: ${port}\n`);
});
