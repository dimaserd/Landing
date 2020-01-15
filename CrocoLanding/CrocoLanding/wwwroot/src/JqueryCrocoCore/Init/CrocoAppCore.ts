class CrocoAppCore {

    static Application: MyCrocoJsApplication;

    static AjaxLoader: AjaxLoader;

    static ToastrWorker: ToastrWorker;

    static InitFields() {
        CrocoAppCore.Application = new MyCrocoJsApplication();
        CrocoAppCore.AjaxLoader = new AjaxLoader();
        CrocoAppCore.ToastrWorker = new ToastrWorker();
        CrocoAppCore.AjaxLoader.InitAjaxLoads();
    }
}