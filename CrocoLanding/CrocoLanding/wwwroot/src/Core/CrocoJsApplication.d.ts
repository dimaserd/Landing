declare class CrocoJsApplication<TRequester extends ICrocoRequester> {
    CookieWorker: CookieWorker;
    FormDataUtils: FormDataUtils;
    FormDataHelper: IFormDataHelper;
    Requester: TRequester;
    Logger: ICrocoLogger;
    ModalWorker: IModalWorker;
}
