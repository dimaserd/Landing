var MyCrocoJsApplication = /** @class */ (function () {
    function MyCrocoJsApplication() {
        this.CookieWorker = new CookieWorker();
        this.FormDataUtils = new FormDataUtils();
        this.FormDataHelper = new FormDataHelper();
        this.Logger = new Logger();
        this.Requester = new Requester();
        this.ModalWorker = new ModalWorker();
    }
    return MyCrocoJsApplication;
}());
