import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Session } from '../session';
import { SessionService } from '../session.service';
import { FormatMetricsManagerNamePipe } from '../format-metrics-manager-name.pipe';
import { FormatChartNamePipe } from '../format-chart-name.pipe';

declare var google: any;

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

  createChart(metricsManager, statistic): any {
    const thresholds = statistic.thresholds;
    const metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
    const chartName = this.formatChartNamePipe.transform(metricsManager.name, statistic.name);

    const content = {
      theme: 'dark2',
      title: {
        text: `${metricName} chart`
      },
      data: [
        {
          type: 'line',
          lineColor: '#ffdc32',
          dataPoints: []
        }
      ],

      axisY: {
        title: metricName,
        titleFontColor: '#c3dc3c',
        lineColor: '#c3dc3c',
        gridColor: '#4b4a4a',
        labelFontColor: '#919191',
        includeZero: false,
        stripLines: []
      },
      axisX: {
        title: 'Seconds',
        titleFontColor: '#c3dc3c',
        lineColor: '#c3dc3c',
        gridColor: '#919191',
        labelFontColor: '#919191'
      }
    }

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

      const point = {
        x: metric.recordTime,
        y: metric.value,
        indexLabel: label,
        color: color,
      };
      content.data[0].dataPoints.push(point);
    }

    for(const threshold of thresholds) {
        const stripLine = {
            value: threshold,
            label: `${threshold.maximum ? 'Maximum' : 'Minimum'} ${threshold}`
        };
        content.axisY.stripLines.push(stripLine);
    }

    const chart = new CanvasJS.Chart(chartName, content);
    chart.render();
  }
}
