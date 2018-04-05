const mongoose = require('mongoose');
const configFile = require('../config.json');
const BadGateway = require('../errors').BadGateway;

function connect(next) {
    const url = configFile.mongoDBServerURL;
    const dbName = configFile.dbName;
    mongoose.connect(`${url}/${dbName}`);
    console.log('**API log**');

    const db = mongoose.connection;
    db.on('error', function(err) {
        return next(new BadGateway('The connection to MongoDB server has failed'));
    });
    db.once('open', function() {
        console.log('Connected successfully to MongoDB server.');
        return next(null, db);
    });
}

module.exports.connect = connect;
