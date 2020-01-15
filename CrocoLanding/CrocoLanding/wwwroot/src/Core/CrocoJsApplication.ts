class CrocoJsApplication<TRequester extends ICrocoRequester> {
    public CookieWorker: CookieWorker;
    public FormDataUtils: FormDataUtils;
    public FormDataHelper: IFormDataHelper;
    public Requester: TRequester;
    public Logger: ICrocoLogger;
    public ModalWorker: IModalWorker;
}