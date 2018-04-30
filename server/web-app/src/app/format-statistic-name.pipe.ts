import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatStatisticName'
})
export class FormatStatisticNamePipe implements PipeTransform {

  transform(statistic: any): string {
    let thresholds = '';
    if(statistic.thresholds) {
      thresholds += '(';
      if(statistic.thresholds.minimum) {
        thresholds += `min: ${statistic.thresholds.minimum}`;
        if(statistic.thresholds.maximum) {
          thresholds += `, max: ${statistic.thresholds.maximum}`;
        }
      }
      else if(statistic.thresholds.maximum) {
        thresholds += `max: ${statistic.thresholds.maximum}`;
      }
      thresholds += ')';
    }
    return `${statistic.name}${thresholds}:`;
  }
}
