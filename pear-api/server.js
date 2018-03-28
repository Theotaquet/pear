const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const sessionRouter = require('./routers/session-router');
const NotFound = require('./errors').NotFound;

const port = process.env.PORT || 3000;

app

.use(bodyParser.json())

.use('/sessions', sessionRouter)

.use('*', function(req, res, next) {
    next(new NotFound());
})

.use(function(err, req, res, next) {
    console.error(err);
    console.error(err.stack);
    res.status(err.status || 500).json(err);
})

.listen(port, function() {
    console.log(`Pe.A.R. RESTful API server started on: ${port}\n`);
});
