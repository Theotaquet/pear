const db_connection = require('./db_connection');
const assert = require('assert');
const ObjectID = require('mongodb').ObjectID;

const mongoDBServerURL = 'mongodb://localhost:27017';
const dbName = 'pearDB';

function getAllSessions(next) {
    db_connection.connect(mongoDBServerURL, dbName, function(db) {
        const collection = db.collection('sessions');

        collection.find().toArray(function(err, docs) {
            assert.equal(err, null);
            console.log(`${docs.length} document(s) returned from ${collection.collectionName}`);
            console.log(docs);
            next(err, docs);
        });
    });
}

function getSession(id, next) {
    db_connection.connect(mongoDBServerURL, dbName, function(db) {
        const collection = db.collection('sessions');

        collection.find( { "_id": ObjectID(id) } ).toArray(function(err, docs) {
            assert.equal(err, null);
            assert.equal(docs.length, 1);
            console.log(`1 document returned from ${collection.collectionName}`);
            console.log(docs);
            next(err, docs);
        });
    });
}

function createSession(body, next) {
    db_connection.connect(mongoDBServerURL, dbName, function(db) {
        const collection = db.collection('sessions');

        collection.insert(body, function(err, result) {
            assert.equal(err, null);
            assert.equal(result.result.n, 1);
            console.log(`1 document inserted into ${collection.collectionName}`);
            console.log(result.ops);
            next(err, result);
        });
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
module.exports.createSession = createSession;
