var Logger_Resx = /** @class */ (function () {
    function Logger_Resx() {
        this.LoggingAttempFailed = "Произошла ошибка в логгировании ошибки, срочно обратитесь к разработчикам приложения";
        this.ErrorOnApiRequest = "Ошибка запроса к апи";
        this.ActionLogged = "Action logged";
        this.ExceptionLogged = "Исключение залоггировано";
        this.ErrorOccuredOnLoggingException = "Произошла ошибка в логгировании ошибки, срочно обратитесь к разработчикам приложения";
    }
    return Logger_Resx;
}());
var Logger = /** @class */ (function () {
    function Logger() {
    }
    Logger.prototype.LogException = function (exceptionText, exceptionDescription, link) {
        var data = {
            ExceptionDate: new Date().toISOString(),
            Description: exceptionDescription,
            Message: exceptionText,
            Uri: link !== null ? link : location.href
        };
        CrocoAppCore.Application.Requester.Post("/Api/Log/Exception", data, function (x) { return console.log(Logger.Resources.ExceptionLogged, x); }, function () { return alert(Logger.Resources.ErrorOccuredOnLoggingException); });
    };
    Logger.prototype.LogAction = function (message, description, eventId, parametersJson) {
        var data = {
            LogDate: new Date().toISOString(),
            EventId: eventId,
            ParametersJson: parametersJson,
            Uri: window.location.href,
            Description: description,
            Message: message
        };
        CrocoAppCore.Application.Requester.Post("/Api/Log/Action", data, function (x) { return console.log(Logger.Resources.ActionLogged, x); }, function () { return alert(Logger.Resources.LoggingAttempFailed); });
    };
    Logger.Resources = new Logger_Resx();
    return Logger;
}());
