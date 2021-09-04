import { Pipe, PipeTransform } from '@angular/core';
import { getDay } from 'date-fns';

@Pipe({
  name: 'getDay',
})
export class GetDayPipe implements PipeTransform {
  transform(value: Date | number): number {
    return getDay(value);
  }
}
