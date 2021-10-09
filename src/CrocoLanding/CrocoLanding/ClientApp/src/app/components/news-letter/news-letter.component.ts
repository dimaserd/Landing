import { Component } from '@angular/core';

@Component({
  selector: 'app-news-letter',
  templateUrl: 'news-letter.component.html',
  styleUrls: ['news-letter.component.scss'],
})
export class NewsLetterComponent {
  phoneNumberOrEmail: string = "";

  send() {
    console.log("send()", this.phoneNumberOrEmail);
  }
}
