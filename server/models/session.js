const configFile = require('../config.json');

class Session {
    constructor(session) {
        for(const prop in session) {
            this[prop] = session[prop];
        }
    }

    applyProcessings() {
        this.validated = true;
        for(const metricsManager of this.metricsManagers) {
            metricsManager.validated = true;
            const metricsManagerConfig = configFile.metricsManagersConfiguration
                .find(x => x.name == metricsManager.name);

            this.calculateStatistics(metricsManager, metricsManagerConfig);
            this.validateStatistics(metricsManager, metricsManagerConfig);

            if(!metricsManager.validated) {
                this.validated = false;
            }
        }
    }

    calculateStatistics(metricsManager, metricsManagerConfig) {
        let average = 0.;
        let max = 0;
        let min = Number.MAX_VALUE;
        metricsManager.statisticsCalculationStartupTime =
                metricsManagerConfig.statisticsCalculationStartupTime;

        const firstRelevantMetric =
                metricsManager.statisticsCalculationStartupTime / metricsManager.updateFrequency - 1;
        for(let i = firstRelevantMetric ; i < metricsManager.metrics.length ; i++) {
            const metricValue = metricsManager.metrics[i].value;
            average += metricValue;
            if(metricValue > max) {
                max = metricValue;
            }
            if(metricValue < min) {
                min = metricValue;
            }
        }
        average /= metricsManager.metrics.length - firstRelevantMetric;

        metricsManager.statistics = [
            {
                name: 'average',
                value: average
            },
            {
                name: 'maximum',
                value: max
            },
            {
                name: 'minimum',
                value: min
            }
        ];
    }

    validateStatistics(metricsManager, metricsManagerConfig) {
        for(let threshold of metricsManagerConfig.thresholds) {
            const statistic = metricsManager.statistics.find(x => x.name == threshold.statistic);
            statistic.thresholds = {
                minimum: threshold.minimum,
                maximum: threshold.maximum
            };

            if((threshold.maximum && statistic.value > threshold.maximum)
                    || (threshold.minimum && statistic.value < threshold.minimum)) {
                statistic.validated = false;
            }
            else {
                statistic.validated = true;
            }

            if(!statistic.validated) {
                metricsManager.validated = false;
            }
        }
    }
}

module.exports = Session;
