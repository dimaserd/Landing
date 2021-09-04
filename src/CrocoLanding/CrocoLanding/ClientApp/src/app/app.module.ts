import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import {
  MatPaginatorIntl,
  MatPaginatorModule,
} from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgSelectModule } from '@ng-select/ng-select';
import { ClipboardModule } from 'ngx-clipboard';
import { LoginService } from 'src/services/LoginService';
import { CodegenByPastedTypedefComponent } from './codegen-by-pasted-typedef/codegen-by-pasted-typedef.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CommonModule, registerLocaleData } from '@angular/common';
import { GetDayPipe } from './pipes/get-day.pipe';
import { GetDatePipe } from './pipes/get-date.pipe';
import localeRu from '@angular/common/locales/ru';
registerLocaleData(localeRu, 'ru');

import { MAT_DATE_LOCALE } from '@angular/material/core';
import { NotifierModule } from 'angular-notifier';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AvatarModule, AvatarSource } from 'ngx-avatar';
import { CustomMatPaginatorIntl } from './angular-material-services/CustomMatPaginatorIntl';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HomeComponent } from './components/home/home.component';
const avatarSourcesOrder = [AvatarSource.CUSTOM, AvatarSource.INITIALS];

@NgModule({
  declarations: [
    AppComponent,
    CodegenByPastedTypedefComponent,
    GetDayPipe,
    GetDatePipe,
    HomeComponent,
  ],
  entryComponents: [],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ClipboardModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatMenuModule,
    MatCardModule,
    MatSelectModule,
    MatTooltipModule,
    MatChipsModule,
    MatListModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatGridListModule,
    MatSlideToggleModule,
    NgSelectModule,
    MatNativeDateModule,
    MatAutocompleteModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    CommonModule,
    NotifierModule,
    MatSidenavModule,
    MatIconModule,
    AvatarModule.forRoot({
      sourcePriorityOrder: avatarSourcesOrder,
    }),
    FontAwesomeModule,
  ],
  providers: [
    FormBuilder,
    LoginService,
    { provide: MAT_DATE_LOCALE, useValue: 'ru' },
    {
      provide: MatPaginatorIntl,
      useClass: CustomMatPaginatorIntl,
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
