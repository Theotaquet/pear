const MongoClient = require('mongodb').MongoClient;
const assert = require('assert');
const configFile = require('./config.json');

function connect(next) {
    const url = configFile.mongoDBServerURL;
    const dbName = configFile.dbName;
    MongoClient.connect(url, function(err, client) {
        assert.equal(null, err);
        console.log(`Connected successfully to MongoDB server`);

        next(client.db(dbName));

        client.close();
    });
}

module.exports.connect = connect;
