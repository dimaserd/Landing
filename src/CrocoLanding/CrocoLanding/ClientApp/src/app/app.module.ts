import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BigLogoComponent } from './components/big-logo/big-logo.component';
import { ScrollService } from './services/scroll.service';
import { DevelopmentComponent } from './components/development/development.component';
import { TechnologiesUsedComponent } from './components/technologies-used/technologies-used.component';
import { OrderDevelopmentComponent } from './components/order-development/order-development.component';
import { NewsLetterComponent } from './components/news-letter/news-letter.component';
import { ContactsComponent } from './components/ contacts/contacts.component';
import { FooterComponent } from './components/footer/footer.component';
import { ScrollButtonComponent } from './components/scroll-button/scroll-button.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    BigLogoComponent,
    DevelopmentComponent,
    TechnologiesUsedComponent,
    OrderDevelopmentComponent,
    NewsLetterComponent,
    ContactsComponent,
    FooterComponent,
    ScrollButtonComponent,
  ],
  imports: [BrowserModule, BrowserAnimationsModule],
  providers: [ScrollService],
  bootstrap: [AppComponent],
})
export class AppModule {}
