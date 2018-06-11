import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart } from 'chart.js';

import { SessionService } from '../session.service';
import { FormatMetricsManagerNamePipe } from '../format-metrics-manager-name.pipe';
import { FormatChartNamePipe } from '../format-chart-name.pipe';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.css']
})
export class SessionComponent implements OnInit, AfterViewInit {

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
        }
    );
  }

  ngAfterViewInit() {
    this.loadCharts();
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

    let data = [];
    let labels = [];
    let pointBackgroundColor = [];
    for(const metric of metricsManager.metrics) {
      data.push(metric.value);
      labels.push(metric.recordTime);

      if(metric.value >= thresholds.minimum && metric.value <= thresholds.maximum) {
        pointBackgroundColor.push('#ccff00');
      }
      else {
        pointBackgroundColor.push('#ff3f00');
      }
    }

    const content = {
      type: 'line',
      data: {
        labels: [],
        datasets: [{
          data: [],
          pointBackgroundColor: [],
          pointBorderWidth: 0.5,
          pointHoverBorderWidth: 1.5,
          borderColor: '#ffdc32',
          lineTension: 0.15,
          backgroundColor: 'rgba(0, 0, 0, 0.20)'
        }]
      },
      options: {
        legend: {
          display: false
        },
        scales: {
          yAxes: [{
            scaleLabel: {
              display: true,
              labelString: metricName,
              fontColor: '#c3dc3c'
            },
            gridLines: {
              color: '#606060',
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
              display: false,
            },
            ticks: {
              fontColor: '#919191'
            }
          }]
        }
      }
    };

    var canvas = <HTMLCanvasElement> document.getElementById(chartName);
    const chart = new Chart(canvas, content);
  }
}
