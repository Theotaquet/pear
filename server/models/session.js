const mongoose = require('mongoose');
const configFile = require('../config.json');

const metricSchema = mongoose.Schema({
    value: Number,
    recordTime: Number
});

const metricsManagerSchema = mongoose.Schema({
    name: String,
    enabled: Boolean,
    updateFrequency: Number,
    metrics: [metricSchema]
})

const sessionSchema = mongoose.Schema({
    _id: mongoose.Schema.Types.ObjectId,
    game: String,
    build: String,
    scene: String,
    startDate: Date,
    duration: Number,
    metricsManagers: [metricsManagerSchema]
});

sessionSchema.method({
    applyProcessings: applyProcessings,
    validateFrameRateAverage: validateFrameRateAverage,
    validateGarbageCollectionCount: validateGarbageCollectionCount
});

function applyProcessings() {
    var session = this._doc;
    session.validated = true;
    for(var i = 0 ; i < session.metricsManagers.length ; i++) {
        var metricsManager = session.metricsManagers[i]._doc;
        if(metricsManager.enabled) {
            metricsManager.validated = true;
            metricsManager.processings = {};
            var config = configFile.metricsManagersConfiguration.find(
                    x => x.name == metricsManager.name);

            switch(metricsManager.name) {
                case 'Frame rate':
                    this.validateFrameRateAverage(metricsManager, config.thresholds.average);
                    break;
                case 'Garbage collection':
                    this.validateGarbageCollectionCount(metricsManager, config.thresholds.count);
                    break;
            }

            if(!metricsManager.validated) {
                session.validated = false;
            }
        }
    }
}

function validateFrameRateAverage(metricsManager, averageThreshold) {
    var average = 0.;
    metricsManager.metrics.forEach(function(metric) {
        average += metric.value;
    });
    average /= metricsManager.metrics.length;
    var validated = average >= averageThreshold;

    metricsManager.processings.average = {
        name: 'Frame rate average',
        validated: validated,
        value: average,
        threshold: averageThreshold
    };

    if(!validated) {
        metricsManager.validated = false;
    }
}

function validateGarbageCollectionCount(metricsManager, countThreshold) {
    var validated = true;
    metricsManager.metrics.forEach(function(metric) {
        if(validated && metric.value > countThreshold) {
            validated = false;
        }
    });

    metricsManager.processings.count = {
        name: 'Garbage collection count',
        validated: validated,
        threshold: countThreshold
    }

    if(!validated) {
        metricsManager.validated = false;
    }
}

module.exports = mongoose.model('Session', sessionSchema);
