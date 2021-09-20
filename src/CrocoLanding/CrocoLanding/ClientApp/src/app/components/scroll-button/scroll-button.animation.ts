import { animate, style, transition, trigger } from '@angular/animations';

export const scrollButtonAnimation = trigger('fadeInFadeOut', [
  transition(':enter', [style({ opacity: 0 }), animate('.25s', style({}))]),
  transition(':leave', [animate('.25s', style({ opacity: 0 }))]),
]);
