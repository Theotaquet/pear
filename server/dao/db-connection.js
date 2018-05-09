const MongoClient = require('mongodb').MongoClient;
const configFile = require('../config.json');
const BadGateway = require('../errors').BadGateway;

function connect(next) {
    const url = configFile.serverConfiguration.mongoDbServerUrl;
    const dbName = configFile.serverConfiguration.dbName;
    MongoClient.connect(url, (err, client) => {
        if(err) {
            return next(new BadGateway('The connection to MongoDB server has failed'));
        }
        else {
            console.log('Connected successfully to MongoDB server.');
            next(null, client.db(dbName));
            client.close();
        }
    });
}

module.exports.connect = connect;
