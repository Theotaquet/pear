const dbConnection = require('./db-connection');
const Session = require('../models/session');
const ObjectId = require('mongodb').ObjectId;

function getAllSessions(req, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            const collection = db.collection('sessions');
            var processResult = function(err, docs) {
                if(docs.length == 0) {
                    console.log('No document returned from the database.');
                }
                else if(!err) {
                    console.log(`${docs.length} document(s) returned` +
                        ` from ${collection.collectionName} in ${db.databaseName}:`);
                    console.log(`${docs}\n`);
                }
                const sessions = [];
                for(const doc of docs) {
                    sessions.push(new Session(doc));
                }
                console.log(`${sessions}\n`);
                return next(err, sessions);
            };

            collection.find(req.query).sort([['startDate', -1]]).toArray(processResult);
        }
    });
}

function getSession(req, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            const id = req.params.sessionId;
            const collection = db.collection('sessions');
            var processResult = function(err, doc) {
                if(!doc) {
                    console.log('No document returned from the database.');
                }
                else if(!err) {
                    console.log(`1 document returned from ${collection.collectionName}` +
                            ` in ${db.databaseName}:`);
                    console.log(`${doc}\n`);
                }
                const session = new Session(doc);
                console.log(`${session}\n`);
                return next(err, session);
            };

            if(id == 'last') {
                collection.findOne(req.query, { sort: [['startDate', -1]] }, processResult);
            }
            else {
                collection.findOne( { '_id': ObjectId(id) }, processResult);
            }
        }
    });
}

function createSession(session, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            const collection = db.collection('sessions');
            var processResult = function(err, doc) {
                if(!err) {
                    console.log(`1 document inserted into ${collection.collectionName}` +
                            ` in ${db.databaseName}:`);
                    console.log(`${doc}\n`);
                }
                return next(err, session);
            };

            collection.insert(session, processResult);
        }
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
module.exports.createSession = createSession;
