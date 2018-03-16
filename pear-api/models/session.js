const mongoose = require('mongoose');

const metricSchema = mongoose.Schema( {
    type: String,
    val: Number,
    recordTime: Number
} );

const sessionSchema = mongoose.Schema( {
    _id: mongoose.Schema.Types.ObjectId,
    game: String,
    build: String,
    scene: String,
    startDate: Date,
    duration: Number,
    metrics: [metricSchema]
} );

module.exports = mongoose.model('Session', sessionSchema);
