const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const apiSessionRouter = require('./routers/api-session-router');
const NotFound = require('./errors').NotFound;

const port = process.env.PORT || 3000;

app

.use(bodyParser.json())

.get('/favicon.ico', function(req, res, next) {
    res.set('Content-Type', 'image/x-icon');
    res.end();
})

.use('/api/sessions', apiSessionRouter)

.use(function(req, res, next) {
    next(new NotFound());
})

.use(function(err, req, res, next) {
    console.error(err);
    console.error(err.stack);
    res.status(err.status || 500).json(err);
})

.listen(port, function() {
    console.log(`\nPe.A.R. RESTful API and web application server started on: ${port}\n`);
});
