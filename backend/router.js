const express = require('express');
const router = express.Router();
const connect = require('./db_connection').connect;
const dao = require('./dao');

router

.get('/:session_id?', function(req, res, next)  {
    res.set('Content-Type', 'application/json');

    if(req.params.session_id)
        dao.getSession(req, function(err, doc) {
            res.status(200).json(doc);
        });
    else
        dao.getAllSessions(req, function(err, docs) {
            res.status(200).json(docs);
        });
})

.post('/', function(req, res, next) {
    res.set('Content-Type', 'application/json');

    dao.createSession(req.body, function(err, result) {
        res.status(201).json(result.ops);
    });
});

module.exports = router;
