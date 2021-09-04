import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseApiResponse } from "src/models/BaseApiResponse";

export interface ForgotPasswordModel {
    email: string;
}

export interface ChangePasswordByToken {
    userId: string;
    token: string;
    newPassword: string;
}

@Injectable({
    providedIn: 'root',
})

export class UserForgotPasswordService {
    constructor(private _httpClient: HttpClient,
        @Inject('BASE_URL') private baseUrl: string){
    }

    public UserForgotPassword(data: ForgotPasswordModel) : Observable<BaseApiResponse>{
        return this._httpClient.post<BaseApiResponse>(this.baseUrl + 'Api/Account/UserForgotPassword', data);
    }

    public ChangePasswordByToken(data: ChangePasswordByToken) : Observable<BaseApiResponse>{
        return this._httpClient.post<BaseApiResponse>(this.baseUrl + 'Api/Account/UserForgotPassword/ChangePassword', data);
    }
}
