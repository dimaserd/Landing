class IndexHandlers {
    static SendRequest(): void {

        let data = {
            EmailOrPhoneNumber: ""
        };

        CrocoAppCore.Application.FormDataHelper.CollectDataByPrefix(data, "callbackReq.");

        console.log("SendRequest", data);

        CrocoAppCore.Application.Requester.Post("/api/Ecc/SendCallBackRequest", data, x => {
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