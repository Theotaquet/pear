const sessionDAO = require('../dao/session-dao');
const Session = require('../models/session');
const mongoose = require('mongoose');

function get(req, res, next)  {
    if(req.params.sessionID)
        sessionDAO.getSession(req, function(err, session) {
            if(err)
                return next(err);
            res.status(200).json(session);
        });

    else
        sessionDAO.getAllSessions(req, function(err, sessions) {
            if(err)
                return next(err);
            res.status(200).json(sessions);
        });
}

function post(req, res, next)  {
    const session = new Session( {
        _id: new mongoose.Types.ObjectId(),
        game: req.body.game,
        build: req.body.build,
        scene: req.body.scene,
        startDate: req.body.startDate,
        duration: req.body.duration,
        metrics: req.body.metrics
    } );

    sessionDAO.createSession(session, function(err, session) {
        if(err)
            return next(err);
        res.status(201).json(session);
    });
}

module.exports.get = get;
module.exports.post = post;
