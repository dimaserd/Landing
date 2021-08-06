import { CourtDescription } from "src/app/services/CourtRecordService";

export interface OptionValue {
  value: string;
  viewValue: string;
}

export interface CourtDescriptionWithOptionValue {
  court: CourtDescription;
  option: OptionValue;
}

export interface HomeComponentFilter {
  courtTypes: string[];
  busyTypes: boolean[];
  courts: CourtDescription[];
  dayShift: string;
  hoursFrom: number;
}