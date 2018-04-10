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
    calculateStatistics: calculateStatistics,
    validateStatistics: validateStatistics
});

function applyProcessings() {
    var session = this._doc;
    session.validated = true;
    for(var i = 0 ; i < session.metricsManagers.length ; i++) {
        var metricsManager = session.metricsManagers[i]._doc;
        if(metricsManager.enabled) {
            metricsManager.validated = true;

            calculateStatistics(metricsManager);

            validateStatistics(metricsManager);

            if(!metricsManager.validated) {
                session.validated = false;
            }
        }
    }
}

function calculateStatistics(metricsManager) {
    var average = 0.;
    var firstRelevantMetric = 3 / metricsManager.updateFrequency - 1;
    for(var i = firstRelevantMetric ; i < metricsManager.metrics.length ; i++) {
        average += metricsManager.metrics[i].value;
    }
    average /= metricsManager.metrics.length - firstRelevantMetric;

    metricsManager.statistics = [
        {
            name: 'average',
            value: average
        }
    ]
}

function validateStatistics(metricsManager) {
    var thresholds = configFile.metricsManagersConfiguration.find(
        x => x.name == metricsManager.name).thresholds;
    for(threshold of thresholds) {
        var statistic = metricsManager.statistics.find(x => x.name == threshold.statistic);
        statistic.threshold = {
            minimum: threshold.minimum,
            maximum: threshold.maximum
        }

        if((threshold.maximum && statistic.value > threshold.maximum)
                || (threshold.minimum && statistic.value < threshold.minimum)) {
            statistic.validated = false;
        }
        else {
            statistic.validated = true;
        }
    }
}

module.exports = mongoose.model('Session', sessionSchema);
