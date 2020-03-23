interface BaseApiResponse {
    IsSucceeded: boolean;
    Message: string;
}

class IndexHandlers {

    static Prefix: string = "callbackReq.";

    static isEmptyOrSpaces(str: string): boolean {
        return str === null || str.match(/^ *$/) !== null;
    }

    static SendRequest(): void {

        let data = {
            EmailOrPhoneNumber: ""
        };

        CrocoAppCore.Application.FormDataHelper.CollectDataByPrefix(data, IndexHandlers.Prefix);

        if (IndexHandlers.isEmptyOrSpaces(data.EmailOrPhoneNumber)) {

            CrocoAppCore.ToastrWorker.ShowError("Необходимо указать адрес электронной почты или номер телефона");
            return;
        }

        console.log("SendRequest", data);

        CrocoAppCore.Application.Requester.PostWithAnimation("/api/Ecc/SendCallBackRequest", data, (x: BaseApiResponse) => {
            if (x.IsSucceeded) {
                
                CrocoAppCore.Application.FormDataHelper.FillDataByPrefix({
                    EmailOrPhoneNumber: ""
                }, IndexHandlers.Prefix);
            }
            console.log(x);
        }, null);
    }

    static SetHandlers() {
        document.getElementById("send-callback-request-btn").addEventListener("click", () => {
            IndexHandlers.SendRequest();
        });
    }
}

IndexHandlers.SetHandlers();