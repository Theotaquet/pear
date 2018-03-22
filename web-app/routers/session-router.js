const express = require('express');
const router = express.Router();
const sessionController = require('../controllers/session-controller');

router

.get('/:sessionID?', setHeader, sessionController.get)

function setHeader(req, res, next) {
    res.set('Content-Type', 'text/html');
    return next();
}

module.exports = router;
