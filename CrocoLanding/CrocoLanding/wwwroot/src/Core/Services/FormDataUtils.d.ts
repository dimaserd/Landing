declare class FormDataUtils {
    GetStartUrlNoParams(startUrl?: string): string;
    GetUrlParamsObject(startUrl?: string): object;
    ProccessStringPropertiesAsDateTime(obj: object, propNames: Array<string>): object;
    ProccessAllDateTimePropertiesAsString(obj: object): object;
}
