const mongoose = require('mongoose');
const apiSessionDao = require('../dao/api-session-dao');
const Session = require('../models/session');

function get(req, res, next) {
    if(!req.params.sessionID) {
        apiSessionDao.getAllSessions(req, (err, sessions) => {
            if(err) {
                return next(err);
            }
            const processedSessions = [];
            sessions.forEach(session => {
                session.applyProcessings();
                processedSessions.push(session._doc);
            });
            res.status(200).json(processedSessions);
        });
    }
    else {
        apiSessionDao.getSession(req, (err, session) => {
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
        gpu: req.body.gpu,
        gpuMemory: req.body.gpuMemory,
        startDate: new Date(req.body.startDate),
        duration: req.body.duration,
        metricsManagers: req.body.metricsManagers
    } );

    apiSessionDao.createSession(session, (err, session) => {
        if(err) {
            return next(err);
        }
        res.status(201).json(session);
    });
}

module.exports.get = get;
module.exports.post = post;
