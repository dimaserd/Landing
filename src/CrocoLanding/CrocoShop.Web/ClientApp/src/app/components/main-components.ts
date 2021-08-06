import {
  HomeComponent,
} from '.';
import { AppComponent } from '../app.component';
import { AddCourtRecordComponent } from './add-court-record/add-court-record.component';
import { CourtTableComponent } from './court-table/court-table.component';
import { TimePlainRecordCardBusyComponent } from './time-plain-record-card-busy/time-plain-record-card-busy.component';
import { TimePlainRecordCardFreeComponent } from './time-plain-record-card-free/time-plain-record-card-free.component';
import { TimePlainRecordCardComponent } from './time-plain-record-card/time-plain-record-card.component';

export const MAIN_COMPONENTS = [
  HomeComponent,
  AppComponent,
  AddCourtRecordComponent,
  CourtTableComponent,
  TimePlainRecordCardComponent,
  TimePlainRecordCardBusyComponent,
  TimePlainRecordCardFreeComponent
];