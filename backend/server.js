const express = require('express');
const router = require('./router');

const app = express();
const port = process.env.PORT || 3000;

app
.use(router)
.listen(port, function() {
    console.log(`Pe.A.R. RESTful API server started on: ${port}\n`);
});
