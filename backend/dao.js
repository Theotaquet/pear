/* this file corresponds to the DAO design pattern and communicates with the database,
 * connecting to it and sending the appropriate requests
 */

const db_connexion = require('./db_connexion');
const mongo = require('mongodb');
const assert = require('assert');

//database coordinates
const url = 'mongodb://localhost:27017';
const dbName = 'pearDB';

//calls API GET request to get all the sessions
function getAllSessions(next) {
    db_connexion.connect(url, dbName, function(db) {
        const collection = db.collection('sessions');

        collection.find().toArray(function(err, docs) {
            assert.equal(err, null);
            console.log(`${docs.length} document(s) returned from ${collection.collectionName}`);
            console.log(docs);
            next(err, docs);
        });
    });
}

//calls API GET request to get the desired session
function getSession(id, next) {
    db_connexion.connect(url, dbName, function(db) {
        const collection = db.collection('sessions');
        const o_id = new mongo.ObjectID(id);

        collection.find( { "_id": id } ).toArray(function(err, docs) {
            assert.equal(err, null);
            console.log(`${docs.length} document(s) returned from ${collection.collectionName}`);
            console.log(docs);
            next(err, docs);
        });
    });
}

//calls API POST request to post and store a new session
function createSession(body, next) {
    db_connexion.connect(url, dbName, function(db) {
        const collection = db.collection('sessions');

        collection.insert(body, function(err, result) {
            assert.equal(err, null);
            console.log(`${result.result.n} document(s) inserted into ${collection.collectionName}`);
            console.log(result.ops);
            next(err, result);
        });
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
module.exports.createSession = createSession;
