const mongoose = require('mongoose');
const configFile = require('../config.json');

function connect(next) {
    const url = configFile.mongoDBServerURL;
    const dbName = configFile.dbName;
    mongoose.connect(`${url}/${dbName}`);

    const db = mongoose.connection;
    db.on('error', function(err) {
        return next(err, db);
    });
    db.once('open', function() {
        console.log('Connected successfully to MongoDB server');
        return next(null, db);
    });
}

module.exports.connect = connect;
