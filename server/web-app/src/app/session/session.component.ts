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
  private colors = [
    '#919191',
    '#96AF00',
    '#FFB711',
    '#B5FFE1',
    '#71E8E8',
    '#C64C27'
  ];

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
      const chartName = this.formatChartNamePipe.transform(metricsManager.name);
      const canvas = <HTMLCanvasElement> document.getElementById(chartName);
      const content = this.generateChart(metricsManager);
      const chart = new Chart(canvas, content);
    }
  }

  generateChart(metricsManager) {
    const metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
    const data = [];
    const labels = [];

    for(const metric of metricsManager.metrics) {
      data.push(metric.value.toFixed(2));
      labels.push(metric.recordTime.toFixed(2));
    }

    const content = {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: metricName,
          data: data,
          pointRadius: 0,
          pointHitRadius: 7,
          pointBackgroundColor: '#4b4a4a',
          pointHoverBorderWidth: 1.5,
          borderColor: this.getRandomColor(),
          borderWidth: 2,
          lineTension: 0.15,
          backgroundColor: '#00000033'
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
              fontColor: '#c3dc3c',
              fontSize: 15
            },
            gridLines: {
              color: '#606060',
            },
            ticks: {
                fontColor: '#919191',
                suggestedMax: metricsManager.statistics.find(x => x.name == 'maximum').value + 50
            }
          }],
          xAxes: [{
            scaleLabel: {
              display: true,
              labelString: 'Seconds',
              fontColor: '#c3dc3c',
              fontSize: 15
            },
            gridLines: {
              display: false,
            },
            ticks: {
              fontColor: '#919191',
              beginAtZero: true
            }
          }]
        }
      }
    };
    return content;
  }

  getRandomColor() {
    const randomIndex = Math.floor(Math.random() * this.colors.length);
    const color = this.colors[randomIndex];
    this.colors.splice(randomIndex, 1);
    return color;
  }
}
