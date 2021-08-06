import { OnInit, OnDestroy, ViewChild, TemplateRef, ElementRef } from '@angular/core';
import { NavigationStart, Router, GuardsCheckEnd } from '@angular/router';
import { Component } from '@angular/core';
import { filter, map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { MatDrawer } from '@angular/material/sidenav';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, OnDestroy {
  @ViewChild('notifierWithHtmlTemplate')
  public notifierWithHtmlTemplate: TemplateRef<any>;
  @ViewChild('drawer', { static: true }) private drawer: MatDrawer;
  private ngUnsubscribe$ = new Subject<void>();
  public withoutMenuContainer$ = this.router.events.pipe(
    filter((event) => event instanceof GuardsCheckEnd),
    map(
      ({
        state: {
          root: {
            firstChild: { data },
          },
        },
      }: GuardsCheckEnd) => data
    ),
    map(({ withoutMenuContainer }) => withoutMenuContainer)
  );

  constructor(
    private router: Router,
  ) {}

  ngOnInit() {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationStart),
        takeUntil(this.ngUnsubscribe$)
      )
      .subscribe(() => {
        if (this.drawer) {
          this.drawer.close();
        }
      });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe$.next();
    this.ngUnsubscribe$.complete();
  }
}