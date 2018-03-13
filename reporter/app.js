const http = require('http');
const fs = require('fs');
const pug = require('pug');
const EventEmitter = require('events').EventEmitter;

const reportFilePath = 'index.html';
const serverURL = 'http://localhost:3000/';
const fpsAverageThreshold = 150;

getLastSession();



function getLastSession() {
    var eventEmitter = new EventEmitter();
    var sessionURL;
    var game = getArg('-game');
    if(game == null)
        sessionURL = serverURL + 'sessions/last';
    else
        sessionURL = serverURL + `sessions/last?game=${game}`;
    http.get(sessionURL, function(res) {
        res.setEncoding('utf8');
        var session;
        res.on('data', function(data) {
            if(data != "null")
                session = data;
        });
        res.on('end', function() {
            if(session == undefined)
                eventEmitter.emit('error', new Error(
                        'The request returned null. Check the game name argument.'));
            session = JSON.parse(session);
            analyseSession(session);
        });
    });

    eventEmitter.on('error', function(e) {
        console.error(e.message);
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
        console.log(`The session was successful.`);
    else
        console.log(`The session doesn't meet the specified requirements.`);

    var result = {
        session: session,
        status: sessionSuccessful,
        processings: processings,
        fpsAverageThreshold: fpsAverageThreshold
    };

    console.log(JSON.stringify(result, null, 4));

    fs.writeFileSync(reportFilePath, buildHTML(result));
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
        if(average > fpsAverageThreshold)
            fpsLimitSuccessful = true;
        return fpsLimitSuccessful;
    }
    return null;
}

function otherProcessing(session) {
    return true;
}

function buildHTML(result) {
    const compiledFunction = pug.compileFile('template.pug');
    return compiledFunction(result);
}

function getArg(arg) {
    var args = process.argv;
    var index = args.indexOf(arg);
    if(index > -1 && args.length > index + 1)
        return args[index + 1];
    return null;
}
