import { HttpClient } from '@angular/common/http';
import { EventEmitter } from '@angular/core';
import { Inject } from '@angular/core';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class AccountClientNotificationsService {

    eventEmitter: EventEmitter<object> = new EventEmitter<object>();

    constructor(private _httpClient: HttpClient,
        @Inject('BASE_URL') private baseUrl: string) {
        setInterval(() => {
            this.GetClientEvent().subscribe(data => {
            })
        }, 1200);
    }

    public GetClientEvent() : Observable<object>{
        return this._httpClient.get<object>(this.baseUrl + 'api/account/GetLastEvent')
    }
}