const mongoose = require('mongoose');
const http = require('http');
const apiSessionDAO = require('../dao/api-session-dao');
const Session = require('../models/session');
const reporter = require('../reporter');

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
            if(session._doc.validated) {
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
        platform: req.body.platform,
        unityVersion: req.body.unityVersion,
        device: req.body.device,
        processorType: req.body.processorType,
        systemMemory: req.body.systemMemory,
        GPU: req.body.GPU,
        GPUMemory: req.body.GPUMemory,
        startDate: new Date(req.body.startDate),
        duration: req.body.duration,
        metricsManagers: req.body.metricsManagers
    } );

    apiSessionDAO.createSession(session, function(err, session) {
        if(err) {
            return next(err);
        }
        res.status(201).json(session);

        session.applyProcessings();
        reporter.report(session);
    });
}

module.exports.get = get;
module.exports.post = post;
