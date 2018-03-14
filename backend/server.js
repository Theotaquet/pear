const express = require('express');
const bodyParser = require('body-parser');
const router = require('./router');

const app = express();
const port = process.env.PORT || 3000;

app
.use(bodyParser.json())
.get('/', function(req, res, next) {
    res.set('Content-Type', 'text/plain');

    res.status(200).end('Hello World!');
})
.use('/sessions', router)
.listen(port, function() {
    console.log(`Pe.A.R. RESTful API server started on: ${port}\n`);
});
