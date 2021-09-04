import { HttpClient } from '@angular/common/http';
import { EventEmitter, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { BaseApiResponse } from 'src/models/BaseApiResponse';

import { Injectable } from '@angular/core';
import { shareReplay, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  loginData: CurrentLoginData = null;
  eventEmitter: EventEmitter<CurrentLoginData> = new EventEmitter<CurrentLoginData>();

  private loginData$: Observable<CurrentLoginData>;

  constructor(
    private _httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  login(data: LoginModel, handler?: (x: BaseApiResponse) => void): void {
    this.loginApi(data).subscribe((res) => {
      if (handler) {
        handler(res);
      }
    });
  }

  public loginApi(data: LoginModel): Observable<BaseApiResponse> {
    return this._httpClient
      .post<BaseApiResponse>(this.baseUrl + 'api/Account/Login', data)
      .pipe(
        tap((res) => {
          if (res.isSucceeded) {
            this.getLoginData();
          }

          this.loginData$ = undefined;
        })
      );
  }

  logOut(): void {
    this._httpClient
      .post<BaseApiResponse>(this.baseUrl + 'api/Account/LogOut', {})
      .subscribe((res) => {
        if (res.isSucceeded) {
          this.getLoginData();
        }
      });
  }

  getLoginData(): void {
    this.getLoginDataApi().subscribe((data) => {
      this.loginData = data;
      this.loginData$ = undefined;
      this.eventEmitter.emit(this.loginData);
    });
  }

  public getLoginDataHelfly(): Observable<CurrentLoginData> {
    if (this.loginData$) {
      return this.loginData$;
    }

    this.loginData$ = this.getLoginDataApi().pipe(shareReplay(1));

    return this.loginData$;
  }

  public getLoginDataApi(): Observable<CurrentLoginData> {
    return this._httpClient
      .get<CurrentLoginData>(this.baseUrl + 'api/Account/User')
      .pipe((data) => {
        data.subscribe((x) => this.eventEmitter.emit(x));
        return data;
      });
  }
}

export interface CurrentLoginData {
  userId: string;
  isAuthenticated: boolean;
  email: string;
  roles: string[];
  avatarFileId: number | null;
  name: string;
  surname: string;
  patronymic: string;
}

export interface LoginModel {
  email: string;
  /* Пароль */
  password: string;
  /* Запомнить меня */
  rememberMe: boolean;
}
