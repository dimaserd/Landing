import { Component } from '@angular/core';
import { ScrollService } from '../../services/scroll.service';
import { distinctUntilChanged, map } from 'rxjs/operators';
import { scrollButtonAnimation } from './scroll-button.animation';

@Component({
  selector: 'app-scroll-button',
  templateUrl: 'scroll-button.component.html',
  styleUrls: ['scroll-button.component.scss'],
  animations: [scrollButtonAnimation],
})
export class ScrollButtonComponent {
  public scrollTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  public showScrollButton$ = this.scrollService.scroll$.pipe(
    map(() => window.scrollY > 200),
    distinctUntilChanged()
  );
  constructor(private scrollService: ScrollService) {}
}
