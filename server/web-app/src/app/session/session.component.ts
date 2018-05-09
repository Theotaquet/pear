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
    for(let metricsManager of this.session.metricsManagers) {
      for(let statistic of metricsManager.statistics) {
        if(statistic.thresholds) {
          google.charts.load('current', {packages: ['corechart']});
          google.charts.setOnLoadCallback(() => this.createChart(metricsManager, statistic));
        }
      }
    }
  }

  createChart(metricsManager, statistic): any {
    var thresholds = statistic.thresholds;
    var metricName = this.formatMetricsManagerNamePipe.transform(metricsManager.name);
    var chartName = this.formatChartNamePipe.transform(metricsManager.name, statistic.name);

    var data = new google.visualization.DataTable();
    data.addColumn('number', 'x');
    data.addColumn('number', metricName);
    data.addColumn( {'type': 'string', 'role': 'style'} );
    data.addColumn( {'type': 'string', 'role': 'annotation'} );

    var minFound = false;
    var maxFound = false;

    for(let metric of metricsManager.metrics) {
      if(metric.value >= thresholds.minimum && metric.value <= thresholds.maximum) {
        var color = '#ccff00';
      }
      else {
        var color = '#ff3f00';
      }

      var annotation = null;
      if(!minFound && metric.value == metricsManager.statistics.find(x => x.name == 'minimum')) {
        annotation = 'min';
        minFound = true;
      }
      else if(!maxFound && metric.value == metricsManager.statistics.find(x => x.name = 'maximum')) {
        annotation = 'max';
        maxFound = true;
      }
      data.addRow([metric.recordTime, metric.value, `{fill-color: ${color}}`, annotation]);
    }

    var chart = new google.visualization.LineChart(document.getElementById(chartName));
    chart.draw(data, null);
  }
}
