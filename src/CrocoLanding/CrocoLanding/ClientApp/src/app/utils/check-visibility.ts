import { ElementRef } from '@angular/core';

export function checkVisibility(imgRef: ElementRef): boolean {
  const element = imgRef?.nativeElement;
  const { top, bottom } = element?.getBoundingClientRect();
  return (
    (top >= 0 && bottom <= window.innerHeight) ||
    (top + 8 < window.innerHeight && bottom - 70 >= 0)
  );
}
