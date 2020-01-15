class FormDataHelper implements IFormDataHelper {

    /*
    * Константа для обозначения null значений и вычленения их из строки
    * */
    public readonly NullValue: string = "VALUE_NULL";

    /*
    * Константа для имени аттрибута содержащего тип данных
    * */
    public readonly DataTypeAttributeName: string = "data-type";


    FillDataByPrefix(object: Object, prefix: string) : void {

        for (let index in object) {

            let valueOfProp = object[index];

            if (valueOfProp === null || valueOfProp === undefined) {
                continue;
            }

            const name = prefix + index;

            const element = document.getElementsByName(name)[0] as any;

            if (element === null || element === undefined) {
                continue;
            }

            if (Array.isArray(valueOfProp)) {

                if (element.type !== "select-multiple") {
                    alert("An attempt to set an array to HTMLInputElement which is not a select with multiple attribute");
                }
                let select = element as HTMLSelectElement;
                for (let i = 0; i < select.options.length; i++) {
                    const opt = select.options[i];

                    const value = valueOfProp.filter(x => opt.value === x).length > 0;

                    opt.selected = value;

                }
                const event = new Event("change");
                element.dispatchEvent(event);

                continue;
            }

            if (element.type === "checkbox") {
                element.checked = valueOfProp;
            }
            else if (element.type === "radio") {
                (document.querySelector(`input[name=${name}][value=${valueOfProp}]`) as HTMLInputElement).checked = true;
            }
            else {
                element.value = valueOfProp;
            }

            //Выбрасываю событие об изменении значения
            const event = new Event("change");
            element.dispatchEvent(event);
        }

    }

    CollectDataByPrefix(object: object, prefix: string): void {

        for (let index in object) {
            if (object.hasOwnProperty(index)) {

                const name = prefix + index;

                const element = document.getElementsByName(name)[0] as HTMLElement;

                if (element == null) {
                    alert(`Элемент не найден по указанному имени ${name}`);
                    return;
                }

                let rawValue = this.GetRawValueFromElement(element);
                object[index] = this.ValueMapper(rawValue, element.getAttribute(this.DataTypeAttributeName));
            }
        }
    }

    private GetRawValueFromElement(htmlElement: any): string {
  
        if (htmlElement.type === "select-multiple") {

            return Array.apply(null, (htmlElement as HTMLSelectElement).options)
                .filter(option => option.selected)
                .map(option => option.value);
        }

        if (htmlElement.type === "radio") {

            var flag = document.querySelector(`input[name="${name}"]:checked`) != null

            if (flag) {
                let elem = document.querySelector(`input[name="${name}"]:checked`) as HTMLInputElement;

                console.log("FormDataHelper.Radio", elem);

                return elem.value;
            }

            return null;
        }

        //Чекбоксы нужно проверять отдельно потому что у них свойство не value а почему-то checked
        return htmlElement.type === "checkbox" ? htmlElement.checked : htmlElement.value;
    }


    /**
     * Собрать данные с сопоставлением типов
     * @param modelPrefix префикс модели
     * @param typeDescription описание типа
     */
    public CollectDataByPrefixWithTypeMatching(modelPrefix: string, typeDescription: CrocoTypeDescription): object {

        this.CheckData(typeDescription);

        const initData = this.BuildObject(typeDescription);

        this.CollectDataByPrefix(initData, modelPrefix);

        for (let i = 0; i < typeDescription.Properties.length; i++) {

            const prop = typeDescription.Properties[i];

            let initValue = this.GetInitValue(initData[prop.PropertyDescription.PropertyName]);

            initData[prop.PropertyDescription.PropertyName] = this.ValueMapper(initValue, prop.TypeName);
        }

        return initData;
    }

    private ValueMapper(rawValue: string, dataType: string): string | number | boolean | Date {

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
    }

    private GetInitValue(propValue: any): string {
        let strValue = (propValue as string);

        return strValue === this.NullValue ? null : strValue;
    }

    private CheckData(typeDescription: CrocoTypeDescription): void {
        if (!typeDescription.IsClass) {
            const mes = "Тип не являющийся классом не поддерживается";
            alert(mes);
            throw Error(mes);
        }
    }

    private BuildObject(typeDescription: CrocoTypeDescription): object {
        const data = {};

        for (let i = 0; i < typeDescription.Properties.length; i++) {

            const prop = typeDescription.Properties[i];

            data[prop.PropertyDescription.PropertyName] = "";
        }

        return data;
    }
}