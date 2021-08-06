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
import {MatSliderModule} from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatGridListModule, MatGridTile } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Resolve } from '@angular/router';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgSelectModule } from '@ng-select/ng-select';
import { ClipboardModule } from 'ngx-clipboard';

import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CommonModule, registerLocaleData } from '@angular/common';
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
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MAIN_COMPONENTS } from './components/main-components';
const avatarSourcesOrder = [AvatarSource.CUSTOM, AvatarSource.INITIALS];

import { NouisliderModule } from 'ng2-nouislider';
import {NgxMaterialTimepickerModule} from 'ngx-material-timepicker';
import { TimePlainRecordsComponent } from './components/time-plain-records/time-plain-records.component';
@NgModule({
  declarations: [
    ...MAIN_COMPONENTS,
    TimePlainRecordsComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    NgxMaterialTimepickerModule,
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
    MatSliderModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    NotifierModule,
    MatSidenavModule,
    MatButtonToggleModule,
    MatIconModule,
    AvatarModule.forRoot({
      sourcePriorityOrder: avatarSourcesOrder,
    }),
    FontAwesomeModule,
    NouisliderModule
  ],
  providers: [
    FormBuilder,
    { provide: MAT_DATE_LOCALE, useValue: 'ru' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
