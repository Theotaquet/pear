/* this file configures the connexion to the database */

//connects the express server to the mongoDB server url
//and calls the next function using the specified database
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
