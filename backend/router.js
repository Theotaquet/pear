/* this file handles HTTP requests and pass it to the DAO */

const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
const dao = require('./dao');

//database coordinates
const url = 'mongodb://localhost:27017';
const dbName = 'pearDB';

//handles every HTTP request and calls the appropriate DAO function
router
.use(bodyParser.json())

.get('/', function(req, res) {
    res.set('Content-Type', 'text/plain');
    res.end('Hello World!');
})
.get('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');
    //calls the dao function to get and show all the sessions
    dao.getAllSessions(function(err, docs) {
        res.json(docs);
    });
})
.get('/sessions/:session_id', function(req, res) {
    res.set('Content-Type', 'application/json');
    //calls the dao function to get and show the desired session
    dao.getSession(req.params.session_id, function(err, docs) {
        res.json(docs);
    });
})
// Is it useful to have a get request to get a single metric?
// If it is, do we need to create separated documents for metrics?
// .get('/sessions/:session_id/metrics/:metric_id', function(req, res) {
//  res.set('Content-Type', 'application/json');
//  //calls API GET request to show the metric
//  db_connexion.connect(url, dbName, function(db) {
//      const collection = db.collection('sessions');

//      collection.find( { "_id": req.params.session_id }, function(err, result) {
//          assert.equal(err, null);
//          console.log(`${result.result.n} document(s) returned from ${collection.collectionName}`);
//          console.log(result.ops);
//          res.json(result.ops);
//      });
//  });
// })
.post('/sessions', function(req, res) {
    res.set('Content-Type', 'application/json');
    //calls the dao function to post and store a new session and show it
    dao.createSession(req.body, function(err, result) {
        res.json(result.ops);
    });
});

module.exports = router;
