var CrocoAppCore = /** @class */ (function () {
    function CrocoAppCore() {
    }
    CrocoAppCore.GetFormDrawFactory = function () {
        return new FormDrawFactory({
            DefaultImplementation: function (x) { return new FormDrawImplementation(x); },
            Implementations: new Map()
                .set("Default", function (x) { return new FormDrawImplementation(x); })
                .set("Tab", function (x) { return new TabFormDrawImplementation(x); })
        });
    };
    CrocoAppCore.InitFields = function () {
        CrocoAppCore.Application = new MyCrocoJsApplication();
        CrocoAppCore.AjaxLoader = new AjaxLoader();
        CrocoAppCore.ToastrWorker = new ToastrWorker();
        CrocoAppCore.GenericInterfaceHelper = new GenericInterfaceAppHelper();
        CrocoAppCore.AjaxLoader.InitAjaxLoads();
    };
    return CrocoAppCore;
}());