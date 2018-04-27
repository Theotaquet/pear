const dbConnection = require('./db-connection');
const Session = require('../models/session');

function getAllSessions(req, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            Session.find(req.query).sort('-startDate').exec(processResult);

            function processResult(err, sessions) {
                db.close();
                if(sessions.length == 0) {
                    console.log('No document returned from the database.');
                }
                else if(!err) {
                    console.log(`${sessions.length} document(s) returned` +
                            ` from ${Session.collection.name} in ${Session.db.name}:`);
                    console.log(`${sessions}\n`);
                }
                return next(err, sessions);
            }
        }
    });
}

function getSession(req, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            const id = req.params.sessionID;
            if(id == 'last') {
                Session.findOne(req.query).sort('-startDate').exec(processResult);
            }
            else {
                Session.findById(id, processResult);
            }

            function processResult(err, session) {
                db.close();
                if(!session) {
                    console.log('No document returned from the database.');
                }
                else if(!err) {
                    console.log(`1 document returned from ${session.collection.name}` +
                            ` in ${session.db.name}:`);
                    console.log(`${session}\n`);
                }
                return next(err, session);
            }
        }
    })
}

function createSession(session, next) {
    dbConnection.connect((err, db) => {
        if(err) {
            return next(err);
        }
        else {
            session.save((err, session) => {
                db.close();
                if(!err) {
                    console.log(`1 document inserted into ${session.collection.name}` +
                            ` in ${session.db.name}:`);
                    console.log(`${session}\n`);
                }
                return next(err, session);
            });
        }
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
module.exports.createSession = createSession;
