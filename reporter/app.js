const http = require('http');
const fs = require('fs');
const pug = require('pug');

const reportFilePath = 'index.html';
const serverURL = 'http://localhost:3000/';
const fpsAverageTreshold = 60;

getLastSession();



function getLastSession() {
    var sessionURL;
    var game = getArg('-game');
    if(game == null)
        sessionURL = serverURL + 'sessions/last';
    else
        sessionURL = serverURL + `sessions/last?game=${game}`;
    http.get(sessionURL, function(res) {
        res.setEncoding('utf8');
        var session = '';
        res.on('data', function(data) {
            session += data;
        });
        res.on('end', function() {
            session = JSON.parse(session);
            analyseSession(session);
        });
    });
}

function analyseSession(session) {
    var processings = [
        {
            function: isFpsLimitSuccessful,
            name: "isFpsLimitSuccessful",
            status: true
        },
        {
            function: otherProcessing,
            name: "otherProcessing",
            status: true
        }
    ]

    var sessionSuccessful = true;

    processings.forEach(function(processing) {
        if(!processing.function) {
            processing.status = false;
            sessionSuccessful = false;
        }
    });

    if(sessionSuccessful)
        console.log(`The last recorded session for ${session.game} was successful.`);
    else
        console.log(`The last recorded session for ${session.game} doesn't meet the specified requirements.`);

    var result = {
        session: session,
        status: sessionSuccessful,
        processings: processings
    };

    console.log(JSON.stringify(result, null, 4));
}

function isFpsLimitSuccessful(session) {
    var fpsEnabled = false;
    var average = 0.;
    session.metrics.forEach(function(metric) {
        if(metric.type == 'fps') {
            fpsEnabled = true;
            average += metric.val;
        }
    });

    if(fpsEnabled) {
        var fpsLimitSuccessful = false;
        average /= session.metrics.length;
        if(average < fpsAverageTreshold)
            fpsLimitSuccessful = true;
        return fpsLimitSuccessful;
    }
    return null;
}

function otherProcessing(session) {
    return true;
}

function getArg(arg) {
    var args = process.argv;
    var index = args.indexOf(arg);
    if(index > -1 && args.length > index + 1)
        return args[index + 1];
    return null;
}
