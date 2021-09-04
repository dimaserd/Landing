import { Pipe, PipeTransform } from '@angular/core';
import { getDate } from 'date-fns';

@Pipe({
  name: 'getDate'
})
export class GetDatePipe implements PipeTransform {

  transform(value: number | Date): number {
    return getDate(value);
  }

}
