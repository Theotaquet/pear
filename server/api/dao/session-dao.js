const http = require('http');
const NotFound = require('../errors').NotFound;


function getAllSessions(req, next) {
    sendGetRequest('sessions', req, next);
}

function getSession(req, next) {
    sendGetRequest(`sessions/${req.params.sessionID}`, req, next);
}

function sendGetRequest(url, req, next) {
    const fullUrl = `${req.protocol}://${req.get('host')}/api/${url}`;

    var queryString = '?';
    for(var param in req.query) {
        queryString += `${param}=${req.query[param]}&`;
    }

    http.get(fullUrl + queryString, function(res) {
        var rawData = '';
        res.on('data', function(data) {
            rawData += data;
        });
        res.on('end', function() {
            var parsedData = JSON.parse(rawData);
            if(!parsedData) {
                console.log('**WEB-APP log**');
                return next(new NotFound());
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
