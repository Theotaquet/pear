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

    dao.getAllSessions(req, function(err, docs) {
        res.json(docs);
    });
})

.get('/sessions/:session_id', function(req, res) {
    res.set('Content-Type', 'application/json');
    res.statusCode = 200;

    dao.getSession(req, function(err, doc) {
        res.json(doc);
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
