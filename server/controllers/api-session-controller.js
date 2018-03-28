const mongoose = require('mongoose');
const apiSessionDAO = require('../dao/api-session-dao');
const Session = require('../models/session');

function get(req, res, next) {
    if(!req.params.sessionID) {
        apiSessionDAO.getAllSessions(req, function (err, sessions) {
            if(err) {
                return next(err);
            }
            var processedSessions = [];
            sessions.forEach(function(session) {
                session.applyProcessings();
                processedSessions.push(session._doc);
            });
            res.status(200).json(processedSessions);
        });
    }
    else {
        apiSessionDAO.getSession(req, function(err, session) {
            if(err) {
                return next(err);
            }
            session.applyProcessings();
            if(session._doc.status) {
                console.log('**API log**\nThe session was successful.\n\n');
            }
            else {
                console.log('**API log**\nThe session doesn\'t meet the specified requirements.\n\n');
            }
            res.status(200).json(session._doc);
        });
    }
}

function post(req, res, next)  {
    const session = new Session( {
        _id: new mongoose.Types.ObjectId(),
        game: req.body.game,
        build: req.body.build,
        scene: req.body.scene,
        startDate: new Date(req.body.startDate),
        duration: req.body.duration,
        fpsEnabled: req.body.fpsEnabled,
        metrics: req.body.metrics
    } );

    apiSessionDAO.createSession(session, function(err, session) {
        if(err)
            return next(err);
        res.status(201).json(session);
    });
}

module.exports.get = get;
module.exports.post = post;
