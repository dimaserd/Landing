var AjaxLoader = /** @class */ (function () {
    function AjaxLoader() {
    }
    AjaxLoader.prototype.InitAjaxLoads = function () {
        var elems = document.getElementsByClassName("ajax-load-html");
        for (var i = 0; i < elems.length; i++) {
            this.LoadInnerHtmlToElement(elems[i], null);
        }
    };
    AjaxLoader.prototype.LoadInnerHtmlToElement = function (element, onSuccessFunc) {
        var link = $(element).data('ajax-link');
        var method = $(element).data('ajax-method');
        var data = $(element).data('request-data');
        var onSuccessScript = $(element).data('on-finish-script');
        if (method == null) {
            method = "GET";
        }
        $.ajax({
            type: method,
            url: link,
            cache: false,
            data: data,
            success: function (response) {
                $(element).html(response);
                $(element).removeClass("ajax-load-html");
                if (onSuccessScript) {
                    eval(onSuccessScript);
                }
                if (onSuccessFunc) {
                    onSuccessFunc();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("There is an execption while executing request " + link);
                console.log(xhr);
            }
        });
    };
    return AjaxLoader;
}());

var FormDrawImplementation = /** @class */ (function () {
    function FormDrawImplementation(model) {
        this._datePickerPropNames = [];
        this._selectClass = 'form-draw-select';
        this._model = model;
        this._htmlDrawHelper = new HtmlSelectDrawHelper(CrocoAppCore.Application.FormDataHelper.NullValue);
    }
    FormDrawImplementation.prototype.BeforeFormDrawing = function () {
        //TODO Init calendar or some scripts
    };
    FormDrawImplementation.prototype.AfterFormDrawing = function () {
        //Красивые селекты
        $("." + this._selectClass).selectpicker('refresh');
        //Иницилизация календарей
        for (var i = 0; i < this._datePickerPropNames.length; i++) {
            var datePickerPropName = this._datePickerPropNames[i];
            FormDrawImplementation.InitCalendarForPrefixedProperty(this._model.Prefix, datePickerPropName);
        }
    };
    FormDrawImplementation.prototype.GetPropertyName = function (propName) {
        return FormDrawHelper.GetPropertyValueName(propName, this._model.Prefix);
    };
    FormDrawImplementation.prototype.GetPropertyBlock = function (propertyName) {
        return this._model.Blocks.find(function (x) { return x.PropertyName === propertyName; });
    };
    FormDrawImplementation.GetElementIdForFakeCalendar = function (modelPrefix, propName) {
        var result = modelPrefix + "_" + propName + "FakeCalendar";
        //в айдишниках не поддерживаются точки поэтому их все заменяю на нижние подчеркивания
        return result.replace(new RegExp(/\./, 'g'), '_');
    };
    FormDrawImplementation.GetElementIdForRealCalendarBackProperty = function (modelPrefix, propName) {
        var result = modelPrefix + "_" + propName + "RealCalendar";
        //в айдишниках не поддерживаются точки поэтому их все заменяю на нижние подчеркивания
        return result.replace(new RegExp(/\./, 'g'), '_');
    };
    FormDrawImplementation.InitCalendarForPrefixedProperty = function (modelPrefix, propName) {
        var calandarElementId = FormDrawImplementation.GetElementIdForFakeCalendar(modelPrefix, propName);
        var backPropElementId = FormDrawImplementation.GetElementIdForRealCalendarBackProperty(modelPrefix, propName);
        DatePickerUtils.SetDatePicker(calandarElementId, backPropElementId);
    };
    FormDrawImplementation.prototype.GetPropValue = function (typeDescription) {
        return ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
    };
    FormDrawImplementation.prototype.RenderDatePicker = function (typeDescription, wrap) {
        var propName = typeDescription.PropertyDescription.PropertyName;
        var renderPropName = this.GetPropertyName(propName);
        this._datePickerPropNames.push(propName);
        var id = FormDrawImplementation.GetElementIdForFakeCalendar(this._model.Prefix, propName);
        var hiddenProps = new Map()
            .set("id", FormDrawImplementation.GetElementIdForRealCalendarBackProperty(this._model.Prefix, propName))
            .set(CrocoAppCore.Application.FormDataHelper.DataTypeAttributeName, CSharpType.String.toString());
        return this.RenderTextBoxInner(typeDescription, wrap, id, renderPropName + "Fake")
            + FormDrawHelper.GetInputTypeHidden(renderPropName, "", hiddenProps);
    };
    FormDrawImplementation.prototype.RenderHidden = function (typeDescription, wrap) {
        var value = this.GetPropValue(typeDescription);
        var html = FormDrawHelper.GetInputTypeHidden(FormDrawHelper.GetPropertyValueName(typeDescription.PropertyDescription.PropertyName, this._model.Prefix), value, null);
        return html;
    };
    FormDrawImplementation.prototype.RenderTextBox = function (typeDescription, wrap) {
        return this.RenderTextBoxInner(typeDescription, wrap, null, this.GetPropertyName(typeDescription.PropertyDescription.PropertyName));
    };
    FormDrawImplementation.prototype.RenderTextBoxInner = function (typeDescription, wrap, id, propName) {
        var _a;
        var value = ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
        var idAttr = id == null ? "" : " id=\"" + id + "\"";
        var propBlock = this.GetPropertyBlock(typeDescription.PropertyDescription.PropertyName);
        var cSharpType = ((_a = propBlock.TextBoxData) === null || _a === void 0 ? void 0 : _a.IsInteger) ? CSharpType.Int : CSharpType.String;
        var dataTypeAttr = CrocoAppCore.Application.FormDataHelper.DataTypeAttributeName + "=" + cSharpType.toString();
        var typeAndStep = propBlock.TextBoxData.IsInteger ? "type=\"number\" step=\"" + propBlock.TextBoxData.IntStep + "\"" : "type=\"text\"";
        var html = "<label for=\"" + typeDescription.PropertyDescription.PropertyName + "\">" + propBlock.LabelText + "</label>\n                <input" + idAttr + " autocomplete=\"off\" class=\"form-control m-input\" name=\"" + propName + "\" " + dataTypeAttr + " " + typeAndStep + " value=\"" + value + "\" />";
        if (!wrap) {
            return html;
        }
        return this.WrapInForm(typeDescription, html);
    };
    FormDrawImplementation.prototype.RenderTextArea = function (typeDescription, wrap) {
        var value = ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
        var styles = "style=\"margin-top: 0px; margin-bottom: 0px; height: 79px;\"";
        var propBlock = this.GetPropertyBlock(typeDescription.PropertyDescription.PropertyName);
        var html = "<label for=\"" + typeDescription.PropertyDescription.PropertyName + "\">" + propBlock.LabelText + "</label>\n            <textarea autocomplete=\"off\" class=\"form-control m-input\" name=\"" + this.GetPropertyName(typeDescription.PropertyDescription.PropertyName) + "\" rows=\"3\" " + styles + ">" + value + "</textarea>";
        if (!wrap) {
            return html;
        }
        return this.WrapInForm(typeDescription, html);
    };
    FormDrawImplementation.prototype.RenderGenericDropList = function (typeDescription, selectList, isMultiple, wrap) {
        var rawValue = ValueProviderHelper.GetRawValueFromValueProvider(typeDescription, this._model.ValueProvider);
        this._htmlDrawHelper.ProccessSelectValues(typeDescription, rawValue, selectList);
        var _class = this._selectClass + " form-control m-input m-bootstrap-select m_selectpicker";
        var dict = isMultiple ? new Map().set("multiple", "") : null;
        var select = HtmlDrawHelper.RenderSelect(_class, this.GetPropertyName(typeDescription.PropertyDescription.PropertyName), selectList, dict);
        var propBlock = this.GetPropertyBlock(typeDescription.PropertyDescription.PropertyName);
        var html = "<label for=\"" + typeDescription.PropertyDescription.PropertyName + "\">" + propBlock.LabelText + "</label>" + select;
        if (!wrap) {
            return html;
        }
        return this.WrapInForm(typeDescription, html);
    };
    FormDrawImplementation.prototype.WrapInForm = function (prop, html) {
        return "<div class=\"form-group m-form__group\" " + FormDrawHelper.GetOuterFormAttributes(prop.PropertyDescription.PropertyName, this._model.Prefix) + ">\n                    " + html + "\n                </div>";
    };
    FormDrawImplementation.prototype.RenderDropDownList = function (typeDescription, selectList, wrap) {
        return this.RenderGenericDropList(typeDescription, selectList, false, wrap);
    };
    FormDrawImplementation.prototype.RenderMultipleDropDownList = function (typeDescription, selectList, wrap) {
        return this.RenderGenericDropList(typeDescription, selectList, true, wrap);
    };
    return FormDrawImplementation;
}());

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

var ModalWorker = /** @class */ (function () {
    function ModalWorker() {
    }
    /** Показать модальное окно по идентификатору. */
    ModalWorker.prototype.ShowModal = function (modalId) {
        if (modalId === "" || modalId == null || modalId == undefined) {
            modalId = ModalWorker.LoadingModal;
        }
        $("#" + modalId).modal('show');
    };
    ModalWorker.prototype.ShowLoadingModal = function () {
        this.ShowModal(ModalWorker.LoadingModal);
    };
    ModalWorker.prototype.HideModals = function () {
        $('.modal').modal('hide');
        $(".modal-backdrop.fade").remove();
        $('.modal').on('shown.bs.modal', function () {
        });
    };
    ModalWorker.prototype.HideModal = function (modalId) {
        $("#" + modalId).modal('hide');
    };
    /**
     * идентификатор модального окна с загрузочной анимацией
     */
    ModalWorker.LoadingModal = "loadingModal";
    return ModalWorker;
}());

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

var Requester_Resx = /** @class */ (function () {
    function Requester_Resx() {
        this.YouPassedAnEmtpyArrayOfObjects = "Вы подали пустой объект в запрос";
        this.ErrorOccuredWeKnowAboutIt = "Произошла ошибка! Мы уже знаем о ней, и скоро с ней разберемся!";
        this.FilesNotSelected = "Файлы не выбраны";
    }
    return Requester_Resx;
}());
var Requester = /** @class */ (function () {
    function Requester() {
    }
    Requester.prototype.DeleteCompletedRequest = function (link) {
        Requester.GoingRequests = Requester.GoingRequests.filter(function (x) { return x !== link; });
    };
    Requester.prototype.SendPostRequestWithAnimation = function (link, data, onSuccessFunc, onErrorFunc) {
        this.SendAjaxPostInner(link, data, onSuccessFunc, onErrorFunc, true);
    };
    Requester.prototype.UploadFilesToServer = function (inputId, link, onSuccessFunc, onErrorFunc) {
        var _this = this;
        var file_data = $("#" + inputId).prop("files");
        var form_data = new FormData();
        if (file_data.length === 0) {
            CrocoAppCore.ToastrWorker.ShowError(Requester.Resources.FilesNotSelected);
            if (onErrorFunc) {
                onErrorFunc();
            }
            return;
        }
        for (var i = 0; i < file_data.length; i++) {
            form_data.append("Files", file_data[i]);
        }
        $.ajax({
            url: link,
            type: "POST",
            data: form_data,
            async: true,
            cache: false,
            dataType: "json",
            contentType: false,
            processData: false,
            success: (function (response) {
                _this.DeleteCompletedRequest(link);
                if (onSuccessFunc) {
                    onSuccessFunc(response);
                }
            }).bind(this),
            error: (function (jqXHR, textStatus, errorThrown) {
                //Логгирую ошибку
                CrocoAppCore.Application.Logger.LogException(textStatus.toString(), "ErrorOnApiRequest", link);
                _this.DeleteCompletedRequest(link);
                CrocoAppCore.Application.ModalWorker.HideModals();
                CrocoAppCore.ToastrWorker.ShowError(Requester.Resources.ErrorOccuredWeKnowAboutIt);
                //Вызываю внешнюю функцию-обработчик
                if (onErrorFunc) {
                    onErrorFunc(jqXHR, textStatus, errorThrown);
                }
            }).bind(this)
        });
    };
    Requester.OnSuccessAnimationHandler = function (data) {
        CrocoAppCore.Application.ModalWorker.HideModals();
        CrocoAppCore.ToastrWorker.HandleBaseApiResponse(data);
    };
    Requester.OnErrorAnimationHandler = function () {
        CrocoAppCore.Application.ModalWorker.HideModals();
        CrocoAppCore.ToastrWorker.ShowError(Requester.Resources.ErrorOccuredWeKnowAboutIt);
    };
    Requester.prototype.Get = function (link, data, onSuccessFunc, onErrorFunc) {
        var _this = this;
        var params = {
            type: "GET",
            data: data,
            url: link,
            async: true,
            cache: false,
            success: (function (response) {
                _this.DeleteCompletedRequest(link);
                if (onSuccessFunc) {
                    onSuccessFunc(response);
                }
            }).bind(this),
            error: (function (jqXHR, textStatus, errorThrown) {
                //Логгирую ошибку
                CrocoAppCore.Application.Logger.LogException(textStatus.toString(), "Error on Api Request", link);
                _this.DeleteCompletedRequest(link);
                //Вызываю внешнюю функцию-обработчик
                if (onErrorFunc) {
                    onErrorFunc(jqXHR, textStatus, errorThrown);
                }
            }).bind(this)
        };
        $.ajax(params);
    };
    Requester.prototype.SendAjaxPostInner = function (link, data, onSuccessFunc, onErrorFunc, animations) {
        var _this = this;
        if (data == null) {
            alert(Requester.Resources.YouPassedAnEmtpyArrayOfObjects);
            return;
        }
        var params = {};
        params.type = "POST";
        params.data = data;
        params.url = link;
        params.async = true;
        params.cache = false;
        params.success = (function (response) {
            _this.DeleteCompletedRequest(link);
            if (animations) {
                Requester.OnSuccessAnimationHandler(response);
            }
            if (onSuccessFunc) {
                onSuccessFunc(response);
            }
        }).bind(this);
        params.error = (function (jqXHR, textStatus, errorThrown) {
            //Логгирую ошибку
            CrocoAppCore.Application.Logger.LogException(textStatus.toString(), "Error on Api Request", link);
            _this.DeleteCompletedRequest(link);
            if (animations) {
                Requester.OnErrorAnimationHandler();
            }
            //Вызываю внешнюю функцию-обработчик
            if (onErrorFunc) {
                onErrorFunc(jqXHR, textStatus, errorThrown);
            }
        }).bind(this);
        var isArray = data.constructor === Array;
        if (isArray) {
            params.contentType = "application/json; charset=utf-8";
            params.dataType = "json";
            params.data = JSON.stringify(data);
        }
        Requester.GoingRequests.push(link);
        $.ajax(params);
    };
    Requester.prototype.Post = function (link, data, onSuccessFunc, onErrorFunc) {
        this.SendAjaxPostInner(link, data, onSuccessFunc, onErrorFunc, false);
    };
    Requester.Resources = new Requester_Resx();
    Requester.GoingRequests = new Array();
    return Requester;
}());

var TabFormDrawImplementation = /** @class */ (function () {
    function TabFormDrawImplementation(model) {
        this._datePickerPropNames = [];
        this._selectClass = 'form-draw-select';
        this._model = model;
        this._drawHelper = new HtmlSelectDrawHelper(CrocoAppCore.Application.FormDataHelper.NullValue);
    }
    TabFormDrawImplementation.prototype.BeforeFormDrawing = function () {
        //TODO Init calendar or some scripts
    };
    TabFormDrawImplementation.prototype.AfterFormDrawing = function () {
        //Красивые селекты
        $("." + this._selectClass).selectpicker('refresh');
        //Иницилизация календарей
        for (var i = 0; i < this._datePickerPropNames.length; i++) {
            var datePickerPropName = this._datePickerPropNames[i];
            FormDrawImplementation.InitCalendarForPrefixedProperty(this._model.Prefix, datePickerPropName);
        }
    };
    TabFormDrawImplementation.prototype.RenderTextBox = function (typeDescription) {
        var value = ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
        return "<div class=\"form-group m-form__group row\">\n                    <label class=\"col-xl-3 col-lg-3 col-form-label\">\n                        " + typeDescription.PropertyDescription.PropertyDisplayName + ":\n                    </label>\n                    <div class=\"col-xl-9 col-lg-9\">\n                        <div class=\"input-group\">\n                            <input type=\"text\" name=\"" + this.GetPropertyValueName(typeDescription.PropertyDescription.PropertyName) + "\" class=\"form-control m-input\" placeholder=\"\" value=\"" + value + "\">\n                        </div>\n                    </div>\n                </div>";
    };
    TabFormDrawImplementation.prototype.RenderTextArea = function (typeDescription) {
        var value = ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
        return "<div class=\"form-group m-form__group row\">\n                    <label class=\"col-xl-3 col-lg-3 col-form-label\">" + typeDescription.PropertyDescription.PropertyDisplayName + "</label>\n                    <div class=\"col-xl-9 col-lg-9\">\n                        <textarea class=\"form-control m-input\" name=\"" + this.GetPropertyValueName(typeDescription.PropertyDescription.PropertyName) + "\" rows=\"3\">" + value + "</textarea>\n                    </div>\n                </div>";
    };
    TabFormDrawImplementation.prototype.RenderGenericDropList = function (typeDescription, selectList, isMultiple) {
        var rawValue = ValueProviderHelper.GetRawValueFromValueProvider(typeDescription, this._model.ValueProvider);
        this._drawHelper.ProccessSelectValues(typeDescription, rawValue, selectList);
        var _class = this._selectClass + " form-control m-input m-bootstrap-select m_selectpicker";
        var dict = isMultiple ? new Map().set("multiple", "") : null;
        var select = HtmlDrawHelper.RenderSelect(_class, this.GetPropertyValueName(typeDescription.PropertyDescription.PropertyName), selectList, dict);
        return "<div class=\"form-group m-form__group row\">\n                    <label class=\"col-xl-3 col-lg-3 col-form-label\">" + typeDescription.PropertyDescription.PropertyDisplayName + ":</label>\n                    <div class=\"col-xl-9 col-lg-9\">\n                        " + select + "\n                    </div>\n                </div>";
    };
    TabFormDrawImplementation.prototype.RenderDropDownList = function (typeDescription, selectList) {
        return this.RenderGenericDropList(typeDescription, selectList, false);
    };
    TabFormDrawImplementation.prototype.RenderMultipleDropDownList = function (typeDescription, selectList) {
        return this.RenderGenericDropList(typeDescription, selectList, true);
    };
    TabFormDrawImplementation.prototype.RenderHidden = function (typeDescription) {
        var value = ValueProviderHelper.GetStringValueFromValueProvider(typeDescription, this._model.ValueProvider);
        return "<input type=\"hidden\" name=\"" + this.GetPropertyValueName(typeDescription.PropertyDescription.PropertyName) + "\" value=\"" + value + "\">";
    };
    TabFormDrawImplementation.prototype.RenderDatePicker = function (typeDescription) {
        this._datePickerPropNames.push(typeDescription.PropertyDescription.PropertyName);
        return this.RenderTextBox(typeDescription);
    };
    TabFormDrawImplementation.prototype.GetPropertyValueName = function (propName) {
        return "" + this._model.Prefix + propName;
    };
    return TabFormDrawImplementation;
}());

var ToastrWorker = /** @class */ (function () {
    function ToastrWorker() {
    }
    ToastrWorker.prototype.ShowError = function (text) {
        var data = {
            IsSucceeded: false,
            Message: text
        };
        this.HandleBaseApiResponse(data);
    };
    ToastrWorker.prototype.ShowSuccess = function (text) {
        var data = {
            IsSucceeded: true,
            Message: text
        };
        this.HandleBaseApiResponse(data);
    };
    ToastrWorker.prototype.HandleBaseApiResponse = function (data) {
        if (data.IsSucceeded === undefined || data.Message === undefined) {
            alert("Произошла ошибка. Объект не является типом BaseApiResponse");
            return;
        }
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        if (data.IsSucceeded) {
            toastr.success(data.Message);
        }
        else {
            toastr.error(data.Message);
        }
    };
    return ToastrWorker;
}());