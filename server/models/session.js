const mongoose = require('mongoose');
const configFile = require('../config.json');

const metricSchema = mongoose.Schema({
    type: String,
    val: Number,
    recordTime: Number
});

const sessionSchema = mongoose.Schema({
    _id: mongoose.Schema.Types.ObjectId,
    game: String,
    build: String,
    scene: String,
    startDate: Date,
    duration: Number,
    fpsEnabled: Boolean,
    metrics: [metricSchema]
});

sessionSchema.method({
    applyProcessings: applyProcessings,
    hasSuccessfulFpsAverage: hasSuccessfulFpsAverage,
});

function applyProcessings() {
    this._doc.status = true;
    this._doc.processings = {};
    this._doc.thresholds = configFile.thresholds;

    if(this.fpsEnabled) {
        this.hasSuccessfulFpsAverage();
    }
}

function hasSuccessfulFpsAverage() {
    var average = 0.;
    this.metrics.forEach(function(metric) {
        if(metric.type == 'fps') {
            average += metric.val;
        }
    });
    average /= this.metrics.length;
    var successfulFpsAverage = average >= this._doc.thresholds.fpsAverage;
    this._doc.processings.successfulFpsAverage = {
        name: "Successful FPS average",
        status: successfulFpsAverage,
        value: average
    };
    if(!successfulFpsAverage) {
        this._doc.status = false;
    }
}

module.exports = mongoose.model('Session', sessionSchema);
