import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { merge, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { checkVisibility } from '../../utils/check-visibility';
import { ScrollService } from '../../services/scroll.service';

@Component({
  selector: 'app-order-development',
  templateUrl: 'order-development.component.html',
  styleUrls: ['order-development.component.scss'],
})
export class OrderDevelopmentComponent implements AfterViewInit {
  @ViewChild('img') public imgRef!: ElementRef;
  private checkImgVisible$ = new Subject<void>();
  public logoVisible$ = merge(
    this.scrollService.scroll$,
    this.checkImgVisible$
  ).pipe(
    map(() => checkVisibility(this.imgRef)),
    distinctUntilChanged(),
    debounceTime(0)
  );
  constructor(private scrollService: ScrollService) {}

  ngAfterViewInit(): void {
    this.checkImgVisible$.next();
  }
}
