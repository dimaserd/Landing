interface CrocoGenericTypeDescription {
    TypeNameWithoutGenericArgs: string;

    GenericArgumentTypeFullNames: Array<string>;
    /*
     * Простые названия типов
     **/
    GenericArgumentTypeNames: Array<string>;
    /**
     * Название полного типа данных
     * */
    FullTypeName: string;
    /**
     * Имя типа для показа
     * */
    GenericDisplayName: string;
    /**
     * Обобщенное название типа
     * */
    GenericTypeNameWithUndefinedArgs: string;
    /**
     * Имплементированный тип данных
     * */
    ImplementedGenericDisplayName: string;
}