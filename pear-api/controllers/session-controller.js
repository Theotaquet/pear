const mongoose = require('mongoose');
const _ = require('underscore');
const sessionDAO = require('../dao/session-dao');
const Session = require('../models/session');

function get(req, res, next)  {
    if(!req.params.sessionID) {
        sessionDAO.getAllSessions(req, processResult);
    }
    else {
        sessionDAO.getSession(req, processResult);
    }

    function processResult(err, result) {
        if(err) {
            return next(err);
        }
        if(result && !_.isArray(result)) {
            result.applyProcessings();
            if(result._doc.status) {
                console.log('The session was successful.\n\n');
            }
            else {
                console.log('The session doesn\'t meet the specified requirements.\n\n');
            }
            res.status(200).json(result._doc);
        }
        else {
            res.status(200).json(result);
        }
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

    sessionDAO.createSession(session, function(err, session) {
        if(err)
            return next(err);
        res.status(201).json(session);
    });
}

module.exports.get = get;
module.exports.post = post;
