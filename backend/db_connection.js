const MongoClient = require('mongodb').MongoClient;
const assert = require('assert');

    function connect(url, dbName, next) {
    MongoClient.connect(url, function(err, client) {
        assert.equal(null, err);
        console.log(`Connected successfully to MongoDB server`);

        next(client.db(dbName));

        client.close();
    });
}

module.exports.connect = connect;
