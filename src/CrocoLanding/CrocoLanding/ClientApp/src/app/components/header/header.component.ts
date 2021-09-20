import { Component, Injectable, OnInit } from '@angular/core';
import { headerAnimation } from './header.animation';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [headerAnimation],
})
export class HeaderComponent implements OnInit {
  public navBarOpened = false;

  constructor() {}

  ngOnInit(): void {}

  public scrollTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  public scrollToNewsletter(): void {
    document
      .getElementById('newsletter')!
      .scrollIntoView({ behavior: 'smooth' });
  }
  public toggleNavBar(): void {
    this.navBarOpened = !this.navBarOpened;
  }
}
