class FormDataUtils {

    GetStartUrlNoParams(startUrl: string = null): string {
        startUrl = startUrl == null ? window.location.href : startUrl;

        if (!startUrl.includes('?')) {
            return startUrl;
        }

        return startUrl.split('?')[0];
    }

    /*
     * Получить объект, который будет содержать все поля
     * */
    GetUrlParamsObject(startUrl: string = null): object {

        startUrl = startUrl == null ? window.location.href : startUrl;

        let url = unescape(startUrl);

        let obj = {};

        if (!url.includes('?')) {
            return obj;
        }

        let paramsUrl = url.split('?')[1].split('&');

        for (let i = 0; i < paramsUrl.length; i++) {

            const para = paramsUrl[i];

            if (!para.includes('=')) {
                continue;
            }

            let bits = paramsUrl[i].split('=');

            obj[bits[0]] = bits[1];
        }

        return obj;
    }


    ProccessStringPropertiesAsDateTime(obj: object, propNames: Array<string>): object {
        if (Array.isArray(obj))
        {
            return (obj as Array<object>).map(x => this.ProccessStringPropertiesAsDateTime(x, propNames));
        }

        for (var i in obj) {

            let oldValue = obj[i];

            if (Array.isArray(oldValue)) {
                obj[i] = (oldValue as Array<object>).map(x => this.ProccessStringPropertiesAsDateTime(x, propNames));
                continue;
            }

            if (oldValue instanceof Object && oldValue.constructor === Object) {
                obj[i] = this.ProccessStringPropertiesAsDateTime(oldValue, propNames);
                continue;
            }

            if (propNames.findIndex(t => t === i) > -1 && obj[i] != null) {
                obj[i] = new Date(oldValue);
            }
        }

        return obj;
    }

    ProccessAllDateTimePropertiesAsString(obj: object): object {
        for (var i in obj) {
            if (Object.prototype.toString.call(obj[i]) === '[object Date]') {
                obj[i] = (obj[i] as Date).toISOString();
            }
        }

        return obj;
    }
}