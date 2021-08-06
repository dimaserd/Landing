import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import {
  CourtDescription,
  CourtRecord,
  CourtRecordService,
  CourtSettings,
  TimeRange,
} from "src/app/services/CourtRecordService";
import { FormControl } from "@angular/forms";
import { MatSliderChange } from "@angular/material/slider";
import { Title } from "@angular/platform-browser";
import { CourtDescriptionWithOptionValue, HomeComponentFilter, OptionValue } from "./models";
import { TimeFunctions } from "src/app/services/TimeFunctions";
import { TimePlainRecord, TimePlainRecordAction, TimePlainRecordWithAction } from "src/app/models/TimePlainRecord";
import { AddCourtRecordComponent } from "../add-court-record/add-court-record.component";
import { MatSnackBar } from "@angular/material/snack-bar";


@Component({
  selector: "app-home",
  styleUrls: ["./home.component.scss"],
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  dataSource: CourtRecord[] = [];
  wholeDataSource: CourtRecord[] = [];

  courtTypesString: string = "Зал;Грунт";
  courtTypes: OptionValue[] = [
    { value: this.courtTypesString, viewValue: "Все корты" },
    { value: "Зал", viewValue: "Зал" },
    { value: "Грунт", viewValue: "Грунт" },
  ];

  todayDate:string = new Date().toISOString().split("T")[0];
  selectedCourts: CourtDescription[] = [];
  days: OptionValue[] = [];

  courtIdsString:string = "";
  courtNumbers: CourtDescriptionWithOptionValue[] = [];

  busyTypesString: string = "true;false";
  busyTypes: OptionValue[] = [
    { value: this.busyTypesString, viewValue: "Показывать все" },
    { value: "true", viewValue: "Показывать занятые" },
    { value: "false", viewValue: "Показывать свободные" },
  ];

  defaultTimeRange: TimeRange = {
    start: "06:00",
    finish: "23:00"
  }
  _defaultCourt: CourtDescription = null;

  filter: HomeComponentFilter = {
    courtTypes: ["Зал", "Грунт"],
    courts: [],
    busyTypes: [true, false],
    dayShift: "0",
    hoursFrom: 6,
  };

  _settings: CourtSettings = null;

  fontStyleControl = new FormControl();
  fontStyle?: string;

  constructor(
    public dialog: MatDialog,
    title: Title,
    private _snackBar: MatSnackBar,
    private _courtRecordService: CourtRecordService
  ) {
    title.setTitle("Теннисные корты");
    this.days = [
      { value: "-1", viewValue: "Вчера" },
      { value: "0", viewValue: "Сегодня" },
      { value: "+1", viewValue: "Завтра" },
    ];
    this.fontStyleControl.setValue("0");
    _courtRecordService.GetSettings().subscribe((data) => {
      this._settings = data;
      
      this.courtTypeChanged();
      this.getRecords(+this.filter.dayShift);
    });
  }

  formatLabel(value: number) {
    var strValue = value.toString();

    if (strValue.length === 1) {
      strValue = `0${strValue}`;
    }

    return `${strValue}:00`;
  }

  courtTypeChanged() {
    this.valuesToFilter();
    this.courtNumbers = [];

    let selectAllValue = this._settings.courts
      .filter(x => this.filter.courtTypes.includes(x.type))
      .map(x => (x.id))
      .join(";");

    let allOption = {
      court: this._defaultCourt,
      option: { value: selectAllValue, viewValue: "Показывать все" },
    };
    this.courtIdsString = selectAllValue;
    this.courtNumbers.push(allOption);

    let courtsToFilter = this._settings.courts;

    courtsToFilter = courtsToFilter.filter(
      x => this.filter.courtTypes.includes(x.type) 
    );

    var data: CourtDescriptionWithOptionValue[] = courtsToFilter.map(x => ({
      court: x,
      option: {
        value: x.id,
        viewValue: `${x.type} ${x.number}`,
      },
    }));

    this.courtNumbers = this.courtNumbers.concat(data);    
    this.filterChanged();
  }

  dayChanged() {
    var dayShift = +this.filter.dayShift;

    this.getRecords(dayShift);
  }

  getRecords(dayShift: number) {
    this._courtRecordService.GetRecordsByDay(dayShift).subscribe((data) => {
      this.wholeDataSource = data;
      this.dataSource = data;
      this.filterChanged();
    });
  }

  openCreateRecordDialog(record: TimePlainRecord) {
    console.log(record);
    let dialogRef = this.dialog.open(AddCourtRecordComponent, {
      data: record,
      width: "600px",
    });

    dialogRef.afterClosed().subscribe(res => {

      console.log("dialogRef.afterClosed()", res);
      var record = res.data as TimePlainRecord;

      if (record !== null && record.tenant !== null) {

        const court = this.selectedCourts.find(x => x.id === record.placeDescription);

        this.onRecordActionHandler({
          action: TimePlainRecordAction.Add,
          record:{
            todayDate: record.todayDate,
            placeDescription: record.placeDescription,
            tenant: record.tenant,
            timeRange:record.timeRange,
            type: record.type
          }
        });
        console.log("dialogRef.afterClosed(); emitted");
        
        this._snackBar.open("Запись добавлена", "Закрыть", {
          duration: 1500,
        });
      }
    });
  }

  onRecordActionHandler(data: TimePlainRecordWithAction){
    
    console.log("onRecordActionHandler()", data);

    if(data.action === TimePlainRecordAction.Add){
      this.wholeDataSource.push({
        todayDate: this.todayDate,
        id:data.record.placeDescription,
        timeRange:data.record.timeRange,
        court: this.selectedCourts.find(x => x.id == data.record.placeDescription),
        tenant: data.record.tenant
      });
      this.filterChanged();
    }
    else if(data.action === TimePlainRecordAction.Delete){
      let record = data.record;
      let indexToRemove = this.wholeDataSource.findIndex(x => x.todayDate === record.todayDate 
        && x.timeRange.start === record.timeRange.start
        && x.timeRange.finish === record.timeRange.finish);
      this.wholeDataSource.splice(indexToRemove, 1);
      this.filterChanged();
    }
    else if(data.action === TimePlainRecordAction.StartAdding){
      this.openCreateRecordDialog(data.record);
    }
  }

  sliderChanged(data: MatSliderChange) {
    this.valuesToFilter();
    this.filter.hoursFrom = data.value;
    this.defaultTimeRange.start = TimeFunctions.plusHours("00:00", this.filter.hoursFrom);
    this.filterChanged();
  }

  valuesToFilter(){
    this.filter.busyTypes = this.busyTypesString.split(";").map(x => x.toLocaleLowerCase() == "true");
    this.filter.courtTypes = this.courtTypesString.split(";");
    
    var courtIds = this.courtIdsString.split(";");
    this.filter.courts = this._settings.courts.filter(x => courtIds.includes(x.id)); 
  }

  filterChanged() {
    this.valuesToFilter();

    let dataSource = this.wholeDataSource.slice();

    if (this.filter.busyTypes === [true]) {
      dataSource = dataSource.filter(x => x.tenant !== null);
    } else if (this.filter.busyTypes === [false]) {
      dataSource = dataSource.filter(x => x.tenant === null);
    }

    dataSource = dataSource
      .filter(x => this.filter.courtTypes.includes(x.court.type)) //фильтровать по типу корта
      .filter(x => this.filter.courts.findIndex(t => t.id === x.court.id) >= 0); //фильтровать по корту

    this.dataSource = dataSource;
    this.selectedCourts = this.filter.courts;
  }
}