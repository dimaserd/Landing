import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseApiResponse } from "src/models/BaseApiResponse";

export interface RegisterModel {
    email: string;
    name: string;
    surname: string;
    patronymic: string;
    phoneNumber: string;
    password: string;
}

@Injectable({
    providedIn: 'root',
})
export class RegistrationService {
    constructor(private _httpClient: HttpClient,
        @Inject('BASE_URL') private baseUrl: string) {
    }

    public Register(data: RegisterModel): Observable<BaseApiResponse> {
        return this._httpClient.post<BaseApiResponse>(this.baseUrl + 'Api/Account/Register', data);
    }

    public RegisterAndSignIn(data: RegisterModel): Observable<BaseApiResponse> {
        return this._httpClient.post<BaseApiResponse>(this.baseUrl + 'Api/Account/RegisterAndSignIn', data);
    }
}
