import { Observable } from 'rxjs';
import { Resolve } from '@angular/router';
import { CurrentLoginData, LoginService } from 'src/services/LoginService';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class IsAdminDataResolver implements Resolve<CurrentLoginData> {
  constructor(private loginService: LoginService) {}
  resolve(): Observable<CurrentLoginData> {
    return this.loginService.getLoginDataHelfly();
  }
}