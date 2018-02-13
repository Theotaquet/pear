const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
const dao = require('./dao');

router
.use(bodyParser.json())
.get('/', function(req, res) {
    res.set('Content-Type', 'text/plain');

    res.end('Hello World!');
})
.get('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');

    dao.getAllSessions(function(err, docs) {
        res.json(docs);
    });
})
.get('/sessions/:session_id', function(req, res) {
    res.set('Content-Type', 'application/json');

    dao.getSession(req.params.session_id, function(err, docs) {
        res.json(docs);
    });
})
.post('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');
    
    dao.createSession(req.body, function(err, result) {
        res.json(result.ops);
    });
});

module.exports = router;
