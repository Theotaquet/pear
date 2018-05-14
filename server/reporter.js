const slack = require('slack');

const token = 'xoxp-3579858503-306200827237-346661601910-7048addf0badba59f2ab1ecf592fc0ab';
const channel = '@theo.constant';

function report(req, session) {
    const pearLink = `${req.protocol}://${req.get('host')}`;
    const sessionLink = `${pearLink}/sessions/${session._id}`;
    let sessionColor;
    let status;
    let statusIcon;
    let buttonStyle;

    if(session.validated) {
        status = 'VALIDATED!';
        statusIcon = 'https://image.flaticon.com/icons/png/512/169/169780.png';
        sessionColor = 'good';
        buttonStyle = 'primary';
    }
    else {
        status = 'FAILED!';
        statusIcon = 'https://image.flaticon.com/icons/png/512/169/169779.png';
        sessionColor = 'danger';
        buttonStyle = 'danger';
    }

    slack.chat.postMessage({
        token: token,
        channel: channel,
        text: 'A new *Pe.A.R session* has been recorded!',
        attachments: [
            {
                fallback: 'Session status',
                color: sessionColor,
                title: `${session.game} - ${session._id}`,
                title_link: sessionLink,
                text: status,
                thumb_url: statusIcon
            },
            {
                fallback: 'Session details',
                color: sessionColor,
                fields: [
                    {
                        title: 'Build',
                        value: session.build,
                        short: true
                    },
                    {
                        title: 'Scene',
                        value: session.scene,
                        short: true
                    },
                    {
                        title: 'Device',
                        value: session.device,
                        short: true
                    },
                    {
                        title: 'Platform',
                        value: session.platform,
                        short: true
                    },
                    {
                        title: 'Duration',
                        value: session.duration,
                        short: true
                    }
                ],
                footer: 'Pe.A.R',
                ts: Math.floor(new Date(session.startDate).getTime() / 1000)
            },
            {
                fallback: 'Button to go to the report',
                color: sessionColor,
                actions: [
                    {
                        type: 'button',
                        text: 'Go to the report',
                        url: sessionLink,
                        style: buttonStyle
                    }
                ]
            }
        ]
    }, console.log);
}

module.exports.report = report;
