const mongoose = require('mongoose');
const configFile = require('../config.json');

const metricSchema = mongoose.Schema({
    val: Number,
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
            metricsManager.thresholds = config.thresholds;

            if(metricsManager.name == 'Frame rate') {
                this.validateFrameRateAverage(metricsManager);
            }

            if(!metricsManager.validated) {
                session.validated = false;
            }
        }
    }
}

function validateFrameRateAverage(metricsManager) {
    var average = 0.;
    metricsManager.metrics.forEach(function(metric) {
        average += metric.val;
    });
    average /= metricsManager.metrics.length;
    var validated = average >= metricsManager.thresholds.average;
    metricsManager.processings.average = {
        name: 'Frame rate average',
        validated: validated,
        value: average
    };
    if(!validated) {
        metricsManager.validated = false;
    }
}

module.exports = mongoose.model('Session', sessionSchema);
