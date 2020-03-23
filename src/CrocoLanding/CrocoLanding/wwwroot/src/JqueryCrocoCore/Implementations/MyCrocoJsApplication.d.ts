declare class MyCrocoJsApplication implements CrocoJsApplication<ICrocoRequester> {
    constructor();
    CookieWorker: CookieWorker;
    FormDataUtils: FormDataUtils;
    FormDataHelper: IFormDataHelper;
    Logger: ICrocoLogger;
    Requester: Requester;
    ModalWorker: IModalWorker;
}
