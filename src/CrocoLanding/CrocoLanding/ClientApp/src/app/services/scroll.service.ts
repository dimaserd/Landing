import { Injectable } from '@angular/core';
import { from, fromEvent } from 'rxjs';
import { publishReplay, refCount, take } from 'rxjs/operators';

@Injectable()
export class ScrollService {
  public readonly scroll$ = fromEvent(window, 'scroll').pipe(
    publishReplay(1),
    refCount()
  );
}
