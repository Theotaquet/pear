/* this file starts the express server and listens requests from the specified port */

const express = require('express');
const router = require('./router');
const bodyParser = require('body-parser');

const app = express();
port = process.env.PORT || 3000;

app
.use(router)
.listen(port, function() {
	console.log(`Pe.A.R. RESTful API server started on: ${port}`);
});
