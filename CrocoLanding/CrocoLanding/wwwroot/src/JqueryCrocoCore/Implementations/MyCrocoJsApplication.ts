class MyCrocoJsApplication implements CrocoJsApplication<ICrocoRequester>{
    
    constructor() {
        this.CookieWorker = new CookieWorker();
        this.FormDataUtils = new FormDataUtils();
        this.FormDataHelper = new FormDataHelper();
        this.Logger = new Logger();
        this.Requester = new Requester();
        this.ModalWorker = new ModalWorker();
    }

    public CookieWorker: CookieWorker;
    public FormDataUtils: FormDataUtils;
    public FormDataHelper: IFormDataHelper;
    public Logger: ICrocoLogger;
    public Requester: Requester;
    public ModalWorker: IModalWorker;
}