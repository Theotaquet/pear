const http = require('http');

const pearAPI = 'http://localhost:3000';

function getAllSessions(req, next) {
    sendGetRequest(`${pearAPI}/sessions`, req.query, next);
}

function getSession(req, next) {
    sendGetRequest(`${pearAPI}/sessions/${req.params.sessionID}`, req.query, next);
}

function sendGetRequest(sessionURL, query, next) {
    var queryString = '?';
    for(var param in query) {
        queryString += `${param}=${query[param]}&`;
    }
    http.get(sessionURL + queryString, function(res) {
        var rawData = '';
        res.on('data', function(data) {
            rawData += data;
        });
        res.on('end', function() {
            var parsedData = JSON.parse(rawData);
            if(!parsedData) {
                return next(new Error('The request returned null. Check the game name argument.'));
            }
            if(res.statusCode != 200) {
                return next(parsedData)
            }
            return next(null, parsedData);
        });
    });
}

module.exports.getAllSessions = getAllSessions;
module.exports.getSession = getSession;
