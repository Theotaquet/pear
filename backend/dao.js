const db_connection = require('./db_connection');
const assert = require('assert');
const ObjectID = require('mongodb').ObjectID;

function getAllSessions(req, next) {
    db_connection.connect(function(db) {
        const collection = db.collection('sessions');

        collection.find(req.query).toArray(function(err, docs) {
            assert.equal(err, null);
            console.log(`${docs.length} document(s) returned from ${collection.collectionName}\n`);
            console.log(`${docs}\n`);
            next(err, docs);
        });
    });
}

function getSession(req, next) {

    db_connection.connect(function(db) {
        const collection = db.collection('sessions');
        var id = req.params.session_id;
        var processResult = function(err, doc) {
            assert.equal(err, null);
            console.log(`1 document returned from ${collection.collectionName}\n`);
            console.log(`${doc}\n`);
            next(err, doc);
        }

        if(id == 'last')
            collection.find(req.query).sort( { _id: -1 } ).limit(1).next(processResult);
        else
            collection.findOne( { '_id': ObjectID(id) } , null, processResult);

    });
}

function createSession(body, next) {
    db_connection.connect(function(db) {
        const collection = db.collection('sessions');

        collection.insert(body, function(err, result) {
            assert.equal(err, null);
            assert.equal(result.result.n, 1);
            console.log(`1 document inserted into ${collection.collectionName}\n`);
            console.log(`${result.ops}\n`);
            next(err, result);
        });
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
module.exports.createSession = createSession;
