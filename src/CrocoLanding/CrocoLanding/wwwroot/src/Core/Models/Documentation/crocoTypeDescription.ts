interface CrocoTypeDescription {
    /**
     * Ќазвание типа, полное название
     * */
    FullTypeName: string;

    /**
     * Ќазвание типа, короткое название
     * */
    TypeName: string;

    /**
     * явл€етс€ ли тип обобщенным
     * */
    IsGeneric: boolean;

    GenericDescription: CrocoGenericTypeDescription;
    PropertyDescription: CrocoPropertyDescription; 
    TypeDisplayName?: string;
    IsNullable: boolean;
    IsEnumerable: boolean;
    IsEnumeration: boolean;
    IsClass: boolean;
    EnumeratedType: CrocoTypeDescription;
    EnumDescription: CrocoEnumTypeDescription;
    Properties?: Array<CrocoTypeDescription>;
}

interface CrocoPropertyDescription {
    PropertyDisplayName?: string;
    PropertyName?: string;
    Description: string;
    Descriptions?: Array<string>;
}