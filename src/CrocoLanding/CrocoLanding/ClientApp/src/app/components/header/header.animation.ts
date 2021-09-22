import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';

export const headerAnimation = trigger('fadeInFadeOut', [
  transition(':enter', [style({ height: 0 }), animate('.25s', style({}))]),
  transition(':leave', [animate('.25s', style({ height: 0 }))]),
]);
