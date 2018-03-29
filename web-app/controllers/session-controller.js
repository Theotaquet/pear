const pug = require('pug');
const sessionDAO = require('../dao/session-dao');

const sessionsListView = 'sessions-list-view';
const sessionView = 'session-view';

function get(req, res, next) {
    if(!req.params.sessionID) {
        sessionDAO.getAllSessions(req, function(err, sessions) {
            if(err) {
                return next(err);
            }
            sessions.forEach(function(session) {
                session.startDate = new Date(session.startDate).toLocaleString();
            });
            res.status(200).render(sessionsListView, {sessions: sessions});
        })
    }
    else {
        sessionDAO.getSession(req, function(err, session) {
            if(err) {
                return next(err);
            }
            session.startDate = new Date(session.startDate).toLocaleString();
            res.status(200).render(sessionView, {session: session});
        });
    }
}

module.exports.get = get;
