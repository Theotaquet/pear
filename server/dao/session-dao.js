const http = require('http');
const NotFound = require('../errors').NotFound;

const apiUrl = 'http://localhost:3000/api/sessions';

function getAllSessions(req, next) {
    sendGetRequest(apiUrl, req.query, next);
}

function getSession(req, next) {
    sendGetRequest(`${apiUrl}/${req.params.sessionID}`, req.query, next);
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
