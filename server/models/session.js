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
            if(metricsManager.enabled) {
                metricsManager.validated = true;

                this.calculateStatistics(metricsManager);

                this.validateStatistics(metricsManager);

                if(!metricsManager.validated) {
                    this.validated = false;
                }
            }
        }
    }

    calculateStatistics(metricsManager) {
        let average = 0.;
        const firstRelevantMetric = 3 / metricsManager.updateFrequency - 1;
        for(let i = firstRelevantMetric ; i < metricsManager.metrics.length ; i++) {
            average += metricsManager.metrics[i].value;
        }
        average /= metricsManager.metrics.length - firstRelevantMetric;

        metricsManager.statistics = [
            {
                name: 'average',
                value: average
            }
        ];
    }

    validateStatistics(metricsManager) {
        const thresholds = configFile.metricsManagersConfiguration
            .find(x => x.name == metricsManager.name).thresholds;
        for(let threshold of thresholds) {
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
