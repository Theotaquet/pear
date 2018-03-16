const express = require('express');
const router = express.Router();
const sessionController = require('../controllers/session-controller');

router

.get('/:sessionID?', setHeader, sessionController.get)

.post('/', setHeader, sessionController.post);

function setHeader(req, res, next) {
    res.set('Content-Type', 'application/json');
    return next();
}

module.exports = router;
