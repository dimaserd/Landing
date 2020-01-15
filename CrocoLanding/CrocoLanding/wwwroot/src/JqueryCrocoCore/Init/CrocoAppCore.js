var CrocoAppCore = /** @class */ (function () {
    function CrocoAppCore() {
    }
    CrocoAppCore.InitFields = function () {
        CrocoAppCore.Application = new MyCrocoJsApplication();
        CrocoAppCore.AjaxLoader = new AjaxLoader();
        CrocoAppCore.ToastrWorker = new ToastrWorker();
        CrocoAppCore.AjaxLoader.InitAjaxLoads();
    };
    return CrocoAppCore;
}());
