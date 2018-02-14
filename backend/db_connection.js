function connect(url, dbName, next) {
    const MongoClient = require('mongodb').MongoClient;
    const assert = require('assert');

    MongoClient.connect(url, function(err, client) {
        assert.equal(null, err);
        console.log(`Connected successfully to server`);

        next(client.db(dbName));

        client.close();
    });
}

module.exports.connect = connect;
