import { Component } from '@angular/core';
import { DevelopmentCard } from '../../models/development-card';

@Component({
  selector: 'app-development',
  templateUrl: 'development.component.html',
  styleUrls: ['development.component.scss'],
})
export class DevelopmentComponent {
  public developmentList: DevelopmentCard[] = [
    {
      title: 'Бизнес приложения',
      description:
        'Осуществляем разработку индивидуальных решений, а также интегрируем программные отдельные модули.',
      icons: ['fa-briefcase'],
    },
    {
      title: 'Интернет проекты',
      description:
        'Осуществляем как разработку индивидуальных решений, так и интеграцию отдельных модулей и решений в информационные системы заказчика.',
      icons: ['fa-chrome'],
    },
    {
      title: 'Десктопные решения',
      description:
        'Защищенные решения для всех распространенных операционных систем, а также реестры, системы учета, контроля, сбора и аналитики данных, ПО.',
      icons: ['fa-desktop'],
    },
    {
      title: 'Мобильные приложения',
      description:
        'Разрабатываем под операционные системы Andoid и iOS — устройства, функционирующие именно на этих операционных системах, наиболее распространены.',
      icons: ['fa-apple', 'fa-android'],
    },
  ];
}
