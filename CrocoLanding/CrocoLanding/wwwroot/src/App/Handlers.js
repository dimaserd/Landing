var IndexHandlers = /** @class */ (function () {
    function IndexHandlers() {
    }
    IndexHandlers.SendRequest = function () {
        var data = {
            EmailOrPhoneNumber: ""
        };
        CrocoAppCore.Application.FormDataHelper.CollectDataByPrefix(data, "callbackReq.");
        console.log("SendRequest", data);
        CrocoAppCore.Application.Requester.Post("/api/Ecc/SendCallBackRequest", data, function (x) {
            console.log(x);
        }, null);
    };
    IndexHandlers.SetHandlers = function () {
        document.getElementById("send-callback-request-btn").addEventListener("click", function () {
            IndexHandlers.SendRequest();
        });
    };
    return IndexHandlers;
}());
IndexHandlers.SetHandlers();
