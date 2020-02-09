var IndexHandlers = /** @class */ (function () {
    function IndexHandlers() {
    }
    IndexHandlers.SendRequest = function () {
        var data = {
            EmailOrPhoneNumber: ""
        };
        CrocoAppCore.Application.FormDataHelper.CollectDataByPrefix(data, IndexHandlers.Prefix);
        console.log("SendRequest", data);
        CrocoAppCore.Application.Requester.PostWithAnimation("/api/Ecc/SendCallBackRequest", data, function (x) {
            if (x.IsSucceeded) {
                CrocoAppCore.Application.FormDataHelper.FillDataByPrefix({
                    EmailOrPhoneNumber: ""
                }, IndexHandlers.Prefix);
            }
            console.log(x);
        }, null);
    };
    IndexHandlers.SetHandlers = function () {
        document.getElementById("send-callback-request-btn").addEventListener("click", function () {
            IndexHandlers.SendRequest();
        });
    };
    IndexHandlers.Prefix = "callbackReq.";
    return IndexHandlers;
}());
IndexHandlers.SetHandlers();
