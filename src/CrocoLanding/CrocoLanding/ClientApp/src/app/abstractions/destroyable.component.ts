import { Component, OnDestroy } from '@angular/core';
import { AsyncSubject } from 'rxjs';
@Component({
  template: ''
})
export abstract class DestroyableComponent implements OnDestroy {
  public ngUnsubscribe = new AsyncSubject<void>();
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
