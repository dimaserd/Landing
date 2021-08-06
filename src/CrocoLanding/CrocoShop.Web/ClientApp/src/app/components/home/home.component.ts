import { Component } from "@angular/core";
import { Title } from "@angular/platform-browser";

@Component({
  selector: "app-home",
  styleUrls: ["./home.component.scss"],
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  
  constructor(
    title: Title,
  ) {
    title.setTitle("Главная CrocoSoft");
  }
}