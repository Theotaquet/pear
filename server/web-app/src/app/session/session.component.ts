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
          google.charts.load('current', { packages: ['corechart'] });
          google.charts.setOnLoadCallback(() => this.createChart(metricsManager, statistic));
        }
      }
    }
  }

//   createChart(metricsManager, statistic): any {
//     var thresholds = statistic.thresholds;
//     var metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
//     var chartName = this.formatChartNamePipe.transform(metricsManager.name, statistic.name);

//     var data = new google.visualization.DataTable();
//     data.addColumn('number', 'x');
//     data.addColumn('number', metricName);
//     data.addColumn( {'type': 'string', 'role': 'style'} );
//     data.addColumn( {'type': 'string', 'role': 'annotation'} );

//     var minFound = false;
//     var maxFound = false;

//     for(let metric of metricsManager.metrics) {
//       if(metric.value >= thresholds.minimum && metric.value <= thresholds.maximum) {
//         var color = '#ccff00';
//       }
//       else {
//         var color = '#ff3f00';
//       }

//       var annotation = null;
//       if(!minFound && metric.value == metricsManager.statistics.find(x => x.name == 'minimum')) {
//         annotation = 'min';
//         minFound = true;
//       }
//       else if(!maxFound && metric.value == metricsManager.statistics.find(x => x.name = 'maximum')) {
//         annotation = 'max';
//         maxFound = true;
//       }
//       data.addRow([metric.recordTime, metric.value, `{fill-color: ${color}}`, annotation]);
//     }

//     var chart = new google.visualization.LineChart(document.getElementById(chartName));
//     chart.draw(data, null);
//   }

  createChart(metricsManager, statistic): any {
    const thresholds = statistic.thresholds;
    const metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
    const chartName = this.formatChartNamePipe.transform(metricsManager.name, statistic.name);

    const data = {
      cols: [
        { label: 'time', type: 'number' },
        { label: 'metric', type: 'number' },
        { label: 'point-color', type: 'string', p: { role: 'style' } },
        { label: 'min-max', type: 'string', p: { role: 'annotation' } },
        { label: 'min-max-desc', type: 'string', p: { role: 'annotationText' } }
      ],
      rows: []
    };

    let minFound = false;
    let maxFound = false;
    let color;
    let annotation;
    let annotationText;

    for(const metric of metricsManager.metrics) {
      if(metric.value >= thresholds.minimum && metric.value <= thresholds.maximum) {
        color = '#ccff00';
      }
      else {
        color = '#ff3f00';
      }

      if(!minFound && metric.value == metricsManager.statistics.find(x => x.name == 'minimum').value) {
        annotation = 'min';
        annotationText = 'Minimum value';
        minFound = true;
      }
      else if(!maxFound && metric.value == metricsManager.statistics.find(x => x.name = 'maximum').value) {
        annotation = 'max';
        annotationText = 'Maximum value';
        maxFound = true;
      }

      const row = {
        c: [
          { v: metric.recordTime },
          { v: metric.value },
          { v: `{ fill-color: ${color} }`},
          { v: annotation },
          { v: annotationText }
        ]
      };
      data.rows.push(row);
    }

    const googleData = new google.visualization.DataTable(data);

    const options = {
      vAxis: {
        title: metricName,
        titleTextStyle: { color: '#c3dc3c', italic: false },
        baselineColor: '#c3dc3c',
        gridlines: { color: '#919191' },
        textStyle: { color: '#919191' }
      },
      hAxis: {
        title: 'Seconds',
        titleTextStyle: { color: '#c3dc3c', italic: false },
        baselineColor: '#c3dc3c',
        gridlines: { color: '#4b4a4a' },
        textStyle: { color: '#919191'}
      },
      colors: ['#ffdc32'],
      pointsVisible: 'true',
      chartArea: { left: '5%', top: '10%', width: '90%', height: '80%' },
      height: 280,
      backgroundColor: '#4b4a4a',
    };

    const chart = new google.visualization.LineChart(document.getElementById(chartName));
    chart.draw(googleData, options);
  }
}
