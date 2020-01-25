var FormDataUtils = /** @class */ (function () {
    function FormDataUtils() {
    }
    FormDataUtils.prototype.GetStartUrlNoParams = function (startUrl) {
        if (startUrl === void 0) { startUrl = null; }
        startUrl = startUrl == null ? window.location.href : startUrl;
        if (!startUrl.includes('?')) {
            return startUrl;
        }
        return startUrl.split('?')[0];
    };
    /*
     * Получить объект, который будет содержать все поля
     * */
    FormDataUtils.prototype.GetUrlParamsObject = function (startUrl) {
        if (startUrl === void 0) { startUrl = null; }
        startUrl = startUrl == null ? window.location.href : startUrl;
        var url = unescape(startUrl);
        var obj = {};
        if (!url.includes('?')) {
            return obj;
        }
        var paramsUrl = url.split('?')[1].split('&');
        for (var i = 0; i < paramsUrl.length; i++) {
            var para = paramsUrl[i];
            if (!para.includes('=')) {
                continue;
            }
            var bits = paramsUrl[i].split('=');
            obj[bits[0]] = bits[1];
        }
        return obj;
    };
    FormDataUtils.prototype.ProccessStringPropertiesAsDateTime = function (obj, propNames) {
        var _this = this;
        if (Array.isArray(obj)) {
            return obj.map(function (x) { return _this.ProccessStringPropertiesAsDateTime(x, propNames); });
        }
        for (var i in obj) {
            var oldValue = obj[i];
            if (Array.isArray(oldValue)) {
                obj[i] = oldValue.map(function (x) { return _this.ProccessStringPropertiesAsDateTime(x, propNames); });
                continue;
            }
            if (oldValue instanceof Object && oldValue.constructor === Object) {
                obj[i] = this.ProccessStringPropertiesAsDateTime(oldValue, propNames);
                continue;
            }
            if (propNames.findIndex(function (t) { return t === i; }) > -1 && obj[i] != null) {
                obj[i] = new Date(oldValue);
            }
        }
        return obj;
    };
    FormDataUtils.prototype.ProccessAllDateTimePropertiesAsString = function (obj) {
        for (var i in obj) {
            if (Object.prototype.toString.call(obj[i]) === '[object Date]') {
                obj[i] = obj[i].toISOString();
            }
        }
        return obj;
    };
    return FormDataUtils;
}());
