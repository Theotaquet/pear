import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart } from 'chart.js';

import { Session } from '../session';
import { SessionService } from '../session.service';
import { FormatMetricsManagerNamePipe } from '../format-metrics-manager-name.pipe';
import { FormatChartNamePipe } from '../format-chart-name.pipe';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.css']
})
export class SessionComponent implements OnInit {

  session: any;

  constructor(
      private sessionService: SessionService,
      private route: ActivatedRoute,
      private formatMetricsManagerNamePipe: FormatMetricsManagerNamePipe,
      private formatChartNamePipe: FormatChartNamePipe
  ) { }

  ngOnInit() {
    this.sessionService.getSession(this.route.snapshot.params['sessionId'])
        .subscribe(session => {
          this.session = session;
          this.loadCharts();
        }
    );
  }

  loadCharts() {
    for(const metricsManager of this.session.metricsManagers) {
      for(const statistic of metricsManager.statistics) {
        if(statistic.thresholds) {
            this.createChart(metricsManager, statistic);
        }
      }
    }
  }

  createChart(metricsManager, statistic) {
    const thresholds = statistic.thresholds;
    const metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
    const chartName = this.formatChartNamePipe.transform(metricsManager.name, statistic.name);

    const content = {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                data: [],
                pointBackgroundColor: [],
                borderColor: '#ffdc32',
            }]
        },
        options: {
            title: {
                display: true,
                text: `${metricName} chart`,
            },
            scales: {
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: metricName,
                        fontColor: '#c3dc3c'
                    },
                    gridLines: {
                        color: '#4b4a4a'
                    },
                    ticks: {
                        fontColor: '#919191'
                    }
                }],
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Seconds',
                        fontColor: '#c3dc3c'
                    },
                    gridLines: {
                        color: '#919191'
                    },
                    ticks: {
                        fontColor: '#919191'
                    }
                }]
            },
            annotation: {
                annotations: []
            }
        }
    };

    let minFound = false;
    let maxFound = false;
    let color;
    let label;

    for(const metric of metricsManager.metrics) {
      if(metric.value >= thresholds.minimum && metric.value <= thresholds.maximum) {
        color = '#ccff00';
      }
      else {
        color = '#ff3f00';
      }

      if(!minFound && metric.value == metricsManager.statistics.find(x => x.name == 'minimum').value) {
        label = 'min';
        minFound = true;
      }
      else if(!maxFound && metric.value == metricsManager.statistics.find(x => x.name = 'maximum').value) {
        label = 'max';
        maxFound = true;
      }

      content.data.labels.push(metric.recordTime);
      content.data.datasets[0].data.push(metric.value);
      content.data.datasets[0].pointBackgroundColor.push(color);
    }

    for(const threshold of thresholds) {
        const annotation = {
            type: 'line',
            mode: 'horizontal',
            value: threshold,
            label: {
                content: `${threshold.maximum ? 'Maximum' : 'Minimum'} ${threshold}`
            }
        };
        content.options.annotation.annotations.push(annotation);
    }

    // const ctx = document.getElementById(chartName);
    const chart = new Chart(chartName, content);
  }
}
