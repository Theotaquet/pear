const apiSessionDao = require('../dao/api-session-dao');
const Session = require('../models/session');

function get(req, res, next) {
    if(!req.params.sessionId) {
        apiSessionDao.getAllSessions(req, (err, sessions) => {
            if(err) {
                return next(err);
            }
            for(const session of sessions) {
                session.applyProcessings();
            }
            res.status(200).json(sessions);
        });
    }
    else {
        apiSessionDao.getSession(req, (err, session) => {
            if(err) {
                return next(err);
            }
            session.applyProcessings();
            if(session.validated) {
                console.log('**API log**\nThe session was successful.\n\n');
            }
            else {
                console.log('**API log**\nThe session doesn\'t meet the specified requirements.\n\n');
            }
            res.status(200).json(session);
        });
    }
}

function post(req, res, next)  {
    const session = new Session(
        undefined,
        req.body.game,
        req.body.build,
        req.body.scene,
        req.body.platform,
        req.body.unityVersion,
        req.body.device,
        req.body.processorType,
        req.body.systemMemory,
        req.body.gpu,
        req.body.gpuMemory,
        new Date(req.body.startDate),
        req.body.duration,
        req.body.metricsManagers
    );

    apiSessionDao.createSession(session, (err, session) => {
        if(err) {
            return next(err);
        }
        res.status(201).json(session);
    });
}

module.exports.get = get;
module.exports.post = post;
