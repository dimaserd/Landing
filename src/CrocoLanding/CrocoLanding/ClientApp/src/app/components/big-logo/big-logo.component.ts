import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ScrollService } from '../../services/scroll.service';
import {
  debounceTime,
  delay,
  distinctUntilChanged,
  map,
  tap,
} from 'rxjs/operators';
import { merge, Subject } from 'rxjs';
import { checkVisibility } from '../../utils/check-visibility';

@Component({
  selector: 'app-big-logo',
  templateUrl: 'big-logo.component.html',
  styleUrls: ['big-logo.component.scss'],
})
export class BigLogoComponent implements AfterViewInit {
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
