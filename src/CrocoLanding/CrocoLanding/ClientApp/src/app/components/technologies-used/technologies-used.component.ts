import { Component } from '@angular/core';

@Component({
  selector: 'app-technologies-used',
  templateUrl: 'technologies-used.component.html',
  styleUrls: ['technologies-used.component.scss'],
})
export class TechnologiesUsedComponent {
  public images: string[] = [
    './assets/images/techIcons/angular.png',
    './assets/images/techIcons/c-plus-plus.png',
    './assets/images/techIcons/css3.png',
    './assets/images/techIcons/microsoft.png',
    './assets/images/techIcons/github.png',
    './assets/images/techIcons/html5.png',
    './assets/images/techIcons/microsoft-sql-server.png',
    './assets/images/techIcons/docker.png',
    './assets/images/techIcons/csharp.png',
    './assets/images/techIcons/my-sql.png',
    './assets/images/techIcons/net-core.png',
    './assets/images/techIcons/postgre-sql.png',
  ];
}
