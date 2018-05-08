import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatMetricsManagerName'
})
export class FormatMetricsManagerNamePipe implements PipeTransform {

  transform(metricsManagerName: string): string {
    let formatedName =
        metricsManagerName
            .substring(0, 1)
            .toUpperCase() +
        metricsManagerName
            .substring(1)
            .replace(/([A-Z0-9])/g, ' $1')
            .toLowerCase();
    return formatedName;
  }
}
