class Logger_Resx {
    LoggingAttempFailed: string = "Произошла ошибка в логгировании ошибки, срочно обратитесь к разработчикам приложения";

    ErrorOnApiRequest: string = "Ошибка запроса к апи";

    ActionLogged: string = "Action logged";

    ExceptionLogged: string = "Исключение залоггировано";

    ErrorOccuredOnLoggingException: string = "Произошла ошибка в логгировании ошибки, срочно обратитесь к разработчикам приложения";
}

class Logger implements ICrocoLogger {

    static Resources: Logger_Resx = new Logger_Resx();

    public LogException(exceptionText: string, exceptionDescription: string, link: string): void {

        let data = {
            ExceptionDate: new Date().toISOString(),
            Description: exceptionDescription,
            Message: exceptionText,
            Uri: link !== null ? link : location.href
        };

        CrocoAppCore.Application.Requester.Post("/Api/Log/Exception", data, x => console.log(Logger.Resources.ExceptionLogged, x), () => alert(Logger.Resources.ErrorOccuredOnLoggingException));
    }

    public LogAction(message: string, description: string, eventId: string, parametersJson: string) : void {

        const data = {
            LogDate: new Date().toISOString(),
            EventId: eventId,
            ParametersJson: parametersJson,
            Uri: window.location.href,
            Description: description,
            Message: message
        };    

        CrocoAppCore.Application.Requester.Post("/Api/Log/Action", data, x => console.log(Logger.Resources.ActionLogged, x), () => alert(Logger.Resources.LoggingAttempFailed));
    }
}