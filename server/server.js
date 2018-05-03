const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const apiSessionRouter = require('./routers/api-session-router');
const NotFound = require('./errors').NotFound;

const port = process.env.PORT || 3000;
const allowedExt = [
    '.js',
    '.ico',
    '.css',
    '.png',
    '.jpg',
    '.woff2',
    '.woff',
    '.ttf',
    '.svg'
];

app

    .use(bodyParser.json())

    .get('/favicon.ico', (req, res) => {
        res.set('Content-Type', 'image/x-icon');
        res.end();
    })

    .use('/api/sessions', apiSessionRouter)

    .use('/api/*', (req, res, next) => {
        next(new NotFound());
    })

    .use((req, res) => {
        if(allowedExt.filter(ext => req.url.indexOf(ext) > 0).length > 0) {
            res.sendFile(`${req.url}`, {root: 'web-app/dist/'});
        }
        else {
            res.sendFile('index.html', {root: 'web-app/dist/'});
        }
    })

    .use((err, req, res) => {
        console.error(err);
        console.error(err.stack);
        res.status(err.status || 500).json(err);
    })

    .listen(port, () => {
        console.log(`\nPe.A.R. RESTful API and web application server started on: ${port}\n`);
    });
