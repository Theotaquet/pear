var slack = require('slack');

function report(session) {
    var sessionColor;
    var status;
    var buttonStyle;

    if(session.validated) {
        status = "validated";
        sessionColor = "good";
        buttonStyle = "primary";
    }
    else {
        status = "failed";
        sessionColor = "danger";
        buttonStyle = "danger";
    }

    var gameId = session._id;
    var gameName = session.game;
    var gameBuild = session.build;
    var gameScene = session.scene;
    var device = session.device;
    var platform = session.platform;
    var duration = session.duration;
    var sessionLink = `http://localhost:3000/sessions/${session._id}`;

    slack.chat.postMessage({
            token: 'xoxp-3579858503-306200827237-346661601910-7048addf0badba59f2ab1ecf592fc0ab',
            channel: '@theo.constant',
            text: 'A new *Pe.A.R session* has been recorded!',
            attachments: [
                {
                    fallback: "Session details",
                    color: sessionColor,
                    author_name: gameId,
                    title: gameName + " - " + status,
                    title_link: sessionLink,
                    fields: [
                        {
                            title: "Build",
                            value: gameBuild,
                            short: true
                        },
                        {
                            title: "Scene",
                            value: gameScene,
                            short: true
                        },
                        {
                            title: "Device",
                            value: device,
                            short: true
                        },
                        {
                            title: "Platform",
                            value: platform,
                            short: true
                        },
                        {
                            title: "Duration",
                            value: duration,
                            short: true
                        }
                    ],
                    actions: [
                        {
                            type: "button",
                            text: "Go to the report",
                            url: sessionLink,
                            style: "primary"
                        }
                    ],
                    footer: "Pe.A.R",
                    ts: new Date(session.startDate).getTime()
                }
            ]
    }, console.log);
}

module.exports.report = report;
