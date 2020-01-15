declare class FormDataHelper implements IFormDataHelper {
    readonly NullValue: string;
    readonly DataTypeAttributeName: string;
    FillDataByPrefix(object: Object, prefix: string): void;
    CollectDataByPrefix(object: object, prefix: string): void;
    private GetRawValueFromElement;
    /**
     * Собрать данные с сопоставлением типов
     * @param modelPrefix префикс модели
     * @param typeDescription описание типа
     */
    CollectDataByPrefixWithTypeMatching(modelPrefix: string, typeDescription: CrocoTypeDescription): object;
    private ValueMapper;
    private GetInitValue;
    private CheckData;
    private BuildObject;
}
