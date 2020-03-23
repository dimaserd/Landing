interface IFormDataHelper {

    /*
    * Константа для обозначения null значений и вычленения их из строки
    * */
    readonly NullValue: string;

    /*
    * Константа для имени аттрибута содержащего тип данных
    * */
    readonly DataTypeAttributeName: string;

    /**
     * Заполнить данные на input элементах Html страницы для свойств объекта
     * @param object   объект, свойства которого нужно заполнить
     * @param prefix   префикс стоящий перед свойствами объекта
     */
    FillDataByPrefix(object: Object, prefix: string): void;

    /**
     *   Собрать данные с формы по префиксу
     * @param object  объект, свойства которого нужно собрать с формы
     * @param prefix  префикс для свойств объекта
     */
    CollectDataByPrefix(object: object, prefix: string): void;

    /**
     * Собрать данные с сопоставлением типов
     * @param modelPrefix префикс модели
     * @param typeDescription описание типа данных
     */
    CollectDataByPrefixWithTypeMatching(modelPrefix: string, typeDescription: CrocoTypeDescription): object;
}