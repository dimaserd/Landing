var FormDataHelper = /** @class */ (function () {
    function FormDataHelper() {
        /*
        * Константа для обозначения null значений и вычленения их из строки
        * */
        this.NullValue = "VALUE_NULL";
        /*
        * Константа для имени аттрибута содержащего тип данных
        * */
        this.DataTypeAttributeName = "data-type";
    }
    FormDataHelper.prototype.FillDataByPrefix = function (object, prefix) {
        for (var index in object) {
            var valueOfProp = object[index];
            if (valueOfProp === null || valueOfProp === undefined) {
                continue;
            }
            var name_1 = prefix + index;
            var element = document.getElementsByName(name_1)[0];
            if (element === null || element === undefined) {
                continue;
            }
            if (Array.isArray(valueOfProp)) {
                if (element.type !== "select-multiple") {
                    alert("An attempt to set an array to HTMLInputElement which is not a select with multiple attribute");
                }
                var select = element;
                var _loop_1 = function (i) {
                    var opt = select.options[i];
                    var value = valueOfProp.filter(function (x) { return opt.value === x; }).length > 0;
                    opt.selected = value;
                };
                for (var i = 0; i < select.options.length; i++) {
                    _loop_1(i);
                }
                var event_1 = new Event("change");
                element.dispatchEvent(event_1);
                continue;
            }
            if (element.type === "checkbox") {
                element.checked = valueOfProp;
            }
            else if (element.type === "radio") {
                document.querySelector("input[name=" + name_1 + "][value=" + valueOfProp + "]").checked = true;
            }
            else {
                element.value = valueOfProp;
            }
            //Выбрасываю событие об изменении значения
            var event_2 = new Event("change");
            element.dispatchEvent(event_2);
        }
    };
    FormDataHelper.prototype.CollectDataByPrefix = function (object, prefix) {
        for (var index in object) {
            if (object.hasOwnProperty(index)) {
                var name_2 = prefix + index;
                var element = document.getElementsByName(name_2)[0];
                if (element == null) {
                    alert("\u042D\u043B\u0435\u043C\u0435\u043D\u0442 \u043D\u0435 \u043D\u0430\u0439\u0434\u0435\u043D \u043F\u043E \u0443\u043A\u0430\u0437\u0430\u043D\u043D\u043E\u043C\u0443 \u0438\u043C\u0435\u043D\u0438 " + name_2);
                    return;
                }
                var rawValue = this.GetRawValueFromElement(element);
                object[index] = this.ValueMapper(rawValue, element.getAttribute(this.DataTypeAttributeName));
            }
        }
    };
    FormDataHelper.prototype.GetRawValueFromElement = function (htmlElement) {
        if (htmlElement.type === "select-multiple") {
            return Array.apply(null, htmlElement.options)
                .filter(function (option) { return option.selected; })
                .map(function (option) { return option.value; });
        }
        if (htmlElement.type === "radio") {
            var flag = document.querySelector("input[name=\"" + name + "\"]:checked") != null;
            if (flag) {
                var elem = document.querySelector("input[name=\"" + name + "\"]:checked");
                console.log("FormDataHelper.Radio", elem);
                return elem.value;
            }
            return null;
        }
        //Чекбоксы нужно проверять отдельно потому что у них свойство не value а почему-то checked
        return htmlElement.type === "checkbox" ? htmlElement.checked : htmlElement.value;
    };
    /**
     * Собрать данные с сопоставлением типов
     * @param modelPrefix префикс модели
     * @param typeDescription описание типа
     */
    FormDataHelper.prototype.CollectDataByPrefixWithTypeMatching = function (modelPrefix, typeDescription) {
        this.CheckData(typeDescription);
        var initData = this.BuildObject(typeDescription);
        this.CollectDataByPrefix(initData, modelPrefix);
        for (var i = 0; i < typeDescription.Properties.length; i++) {
            var prop = typeDescription.Properties[i];
            var initValue = this.GetInitValue(initData[prop.PropertyDescription.PropertyName]);
            initData[prop.PropertyDescription.PropertyName] = this.ValueMapper(initValue, prop.TypeName);
        }
        return initData;
    };
    FormDataHelper.prototype.ValueMapper = function (rawValue, dataType) {
        console.log("FormDataHelper.ValueMapper", rawValue, dataType);
        if (rawValue === this.NullValue) {
            return null;
        }
        switch (dataType) {
            case CSharpType.DateTime.toString():
                return new Date(rawValue);
            case CSharpType.Decimal.toString():
                return (rawValue !== null) ? Number((rawValue).replace(/,/g, '.')) : null;
            case CSharpType.Boolean.toString():
                return (rawValue !== null) ? rawValue.toLowerCase() === "true" : null;
        }
        return rawValue;
    };
    FormDataHelper.prototype.GetInitValue = function (propValue) {
        var strValue = propValue;
        return strValue === this.NullValue ? null : strValue;
    };
    FormDataHelper.prototype.CheckData = function (typeDescription) {
        if (!typeDescription.IsClass) {
            var mes = "Тип не являющийся классом не поддерживается";
            alert(mes);
            throw Error(mes);
        }
    };
    FormDataHelper.prototype.BuildObject = function (typeDescription) {
        var data = {};
        for (var i = 0; i < typeDescription.Properties.length; i++) {
            var prop = typeDescription.Properties[i];
            data[prop.PropertyDescription.PropertyName] = "";
        }
        return data;
    };
    return FormDataHelper;
}());
