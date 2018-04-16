const express = require('express');
const router = express.Router();
const apiSessionController = require('../controllers/api-session-controller');

router

.get('/:sessionID?', setHeader, apiSessionController.get)

.post('/', setHeader, apiSessionController.post);

function setHeader(req, res, next) {
    res.set('Content-Type', 'application/json');
    return next();
}

module.exports = router;
