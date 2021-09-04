import { NotifierService } from "angular-notifier";
import { BaseApiResponse } from "src/models/BaseApiResponse";

export class NotificaterExtensions {
    public static ShowBaseApiResponse(notifier: NotifierService, data: BaseApiResponse){
        notifier.notify(data.isSucceeded? "success" : "error", data.message);
    }
} 