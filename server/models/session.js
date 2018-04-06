const mongoose = require('mongoose');
const configFile = require('../config.json');

const metricSchema = mongoose.Schema({
    value: Number,
    recordTime: Number
});

const metricsManagerSchema = mongoose.Schema({
    id: String,
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
    platform: String,
    unityVersion: String,
    device: String,
    processorType: String,
    systemMemory: Number,
    GPU: String,
    GPUMemory: Number,
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
                    x => x.id == metricsManager.id);

            switch(metricsManager.id) {
                case 'frameRate':
                    this.validateFrameRateAverage(metricsManager, config.thresholds.average);
                    break;
                case 'garbageCollection':
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
    var firstRelevantMetric = 3 / metricsManager.updateFrequency - 1;
    for(var i = firstRelevantMetric ; i < metricsManager.metrics.length ; i++) {
        average += metricsManager.metrics[i].value;
    }
    average /= metricsManager.metrics.length - firstRelevantMetric;
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
    var firstRelevantMetric = 3 / metricsManager.updateFrequency - 1;
    for(var i = firstRelevantMetric ; i < metricsManager.metrics.length ; i++) {
        if(metricsManager.metrics[i].value > countThreshold) {
            validated = false;
            i = metricsManager.metrics.length;
        }
    }

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
