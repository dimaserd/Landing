export interface CrocoGenericTypeDescription {
    typeNameWithoutGenericArgs: string;
    genericArgumentTypeFullNames: Array<string>;
    genericArgumentTypeNames: Array<string>;
    fullTypeName: string;
    cSharpAliasTypeDisplayName: string;
    cSharpAliasTypeDisplayFullName: string;
    genericTypeNameWithUndefinedArgs: string;
    implementedGenericDisplayName: string;
}