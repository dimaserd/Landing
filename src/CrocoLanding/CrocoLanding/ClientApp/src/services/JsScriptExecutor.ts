import { HttpClient } from "@angular/common/http";
import { JsScriptExecutedResult } from "src/models/js-open-api/JsScriptExecutedResult";

export class JsScriptExecutor{
    _httpClient: HttpClient;
    _url: string;
    constructor(httpClient: HttpClient, url: string){
       this._httpClient = httpClient; 
       this._url = url;
    }

    public ExecuteScript(script: string, handler: (res: JsScriptExecutedResult) => void): void {
        
        let data = {
            script: script
        };
        
        this._httpClient.post<JsScriptExecutedResult>(this._url, data).subscribe(handler);
    }
}