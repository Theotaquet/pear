/* this file handles HTTP requests and communicates with the database */

const express = require('express');
const router = express.Router();
const assert = require('assert');
const mongo = require('mongodb');
const bodyParser = require('body-parser');
const db_connexion = require('./db_connexion');

//database coordinates
const url = 'mongodb://localhost:27017';
const dbName = 'pearDB';

//handle every HTTP request,
//connecting to the database and sending the appropriate requests
router
.use(bodyParser.json())

.get('/', function(req, res) {
	res.set('Content-Type', 'text/plain');
	res.end('Hello World!');
})
.get('/sessions', function(req, res) {
	res.set('Content-Type', 'application/json');
	//call API GET request to show all the sessions
	db_connexion.connect(url, dbName, function(db) {
		const collection = db.collection('sessions');

		collection.find().toArray(function(err, docs) {
			assert.equal(err, null);
			console.log(`${docs.length} document(s) returned from ${collection.collectionName}`);
			console.log(docs);
			res.json(docs);
		});
	});
})
.get('/sessions/:session_id', function(req, res) {
	res.set('Content-Type', 'application/json');
	//call API GET request to show the desired session
	db_connexion.connect(url, dbName, function(db) {
		const collection = db.collection('sessions');
		const o_id = new mongo.ObjectID(req.params.session_id);

		collection.find( { "_id": o_id } ).toArray(function(err, docs) {
			assert.equal(err, null);
			console.log(`${docs.length} document(s) returned from ${collection.collectionName}`);
			console.log(docs);
			res.json(docs);
		});
	});
})
// Is it useful to have a get request to get a single metric?
// If it is, do we need to create separated documents for metrics?
// .get('/sessions/:session_id/metrics/:metric_id', function(req, res) {
//	res.set('Content-Type', 'application/json');
// 	//call API GET request to show the metric
// 	db_connexion.connect(url, dbName, function(db) {
// 		const collection = db.collection('sessions');

// 		collection.find( { "_id": req.params.session_id }, function(err, result) {
// 			assert.equal(err, null);
// 			console.log(`${result.result.n} document(s) returned from ${collection.collectionName}`);
// 			console.log(result.ops);
// 			res.json(result.ops);
// 		});
// 	});
// })
.post('/sessions', function(req, res) {
	res.set('Content-Type', 'application/json');
	//call API POST request to store a new session
	db_connexion.connect(url, dbName, function(db) {
		const collection = db.collection('sessions');

		collection.insert(req.body, function(err, result) {
			assert.equal(err, null);
			console.log(`${result.result.n} document(s) inserted into ${collection.collectionName}`);
			console.log(result.ops);
			res.json(result.ops);
		});
	});
});

module.exports = router;
