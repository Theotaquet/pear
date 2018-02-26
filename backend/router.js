const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
const dao = require('./dao');

router
.use(bodyParser.json())

.get('/', function(req, res) {
    res.set('Content-Type', 'text/plain');
    res.statusCode = 200;

    res.end('Hello World!');
})

.get('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');
    res.statusCode = 200;

    dao.getAllSessions(function(err, docs) {
        res.json(docs);
    });
})

.get('/sessions/:session_id', function(req, res) {
    res.set('Content-Type', 'application/json');
    res.statusCode = 200;

    dao.getSession(req.params.session_id, function(err, docs) {
        res.json(docs);
    });
})

.post('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');
    res.statusCode = 201;

    dao.createSession(req.body, function(err, result) {
        res.json(result.ops);
    });
});

module.exports = router;
