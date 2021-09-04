import { CrocoTypeDescriptionResult, CrocoTypeDescription, CrocoPropertyReferenceDescription } from "src/models/typings";
import {TsSimpleTypeMapper} from "./TsSimpleTypeMapper"
import { TsEnumTypeDescriptor } from "./TsEnumTypeDescriptor";

interface GeneratedData{
    IsGenerated: boolean;
    GeneratedText: string;
}

export class TSClassGenerator {

    _useJsNamingStyle: boolean
    _useGenerics: boolean;

    constructor(useJsNamingStyle: boolean, useGenerics: boolean){
        this._useJsNamingStyle = useJsNamingStyle;
        this._useGenerics = useGenerics;
    }

    public static GetDescription(propReference: CrocoPropertyReferenceDescription): string {
        
        if (propReference.propertyDescription.descriptions.length > 0) {
            return `\t/* ${propReference.propertyDescription.descriptions[0]} */\n`;
        }

        return "";
    }

    public static GetUniqueTypes(typeDescription: CrocoTypeDescriptionResult): Array<CrocoTypeDescription> {
        
        const typeNamesToIgnore = ["System.Object"];
        
        return typeDescription.types.filter(x => !x.isPrimitive).filter(x => !typeNamesToIgnore.includes(x.typeDisplayFullName));
    }

    static GetTypeDisplayName(typeDescription: CrocoTypeDescription, isDeclaration: boolean, useGenerics: boolean): string {

        if (!useGenerics) {
            return TsSimpleTypeMapper.ExtractName(typeDescription.typeDisplayFullName);
        }

        if (isDeclaration) {
            if (!typeDescription.isGeneric) {
                return TsSimpleTypeMapper.ExtractName(typeDescription.typeDisplayFullName);
            }

            return typeDescription.genericDescription.genericTypeNameWithUndefinedArgs;
        }

        if (typeDescription.isGeneric) {

            let genDescr = typeDescription.genericDescription;

            let genTypeTsNames = genDescr.genericArgumentTypeNames.map(x => {
                if (TsSimpleTypeMapper.simpleTypes.indexOf(x) >= 0) {
                    return TsSimpleTypeMapper.GetPropertyTypeByTypeDisplayName(x);
                }

                return x;
            });

            return `${genDescr.typeNameWithoutGenericArgs}<${genTypeTsNames.join(',')}>`;
        }

        return TsSimpleTypeMapper.ExtractName(typeDescription.typeDisplayFullName);
    }

    GetPropName(propName: string): string{
        if(this._useJsNamingStyle){
            return propName[0].toLowerCase() + propName.substr(1);
        }

        return propName;
    }

    GenerateTypeInterface(typeDescription: CrocoTypeDescription, wholeModel: CrocoTypeDescriptionResult): GeneratedData {

        if (typeDescription.isEnumeration) {
            return {
                IsGenerated: true,
                GeneratedText: TsEnumTypeDescriptor.GetEnum(typeDescription)
            }
        }

        if (typeDescription.isClass) {

            let result: string = "";

            result += `interface ${TSClassGenerator.GetTypeDisplayName(typeDescription, true, this._useGenerics)} {\n`;

            for (let i = 0; i < typeDescription.properties.length; i++) {

                const prop = typeDescription.properties[i];

                var propTypeDescription = wholeModel.types.find(x => x.typeDisplayFullName === prop.typeDisplayFullName);
                if (propTypeDescription.arrayDescription.isArray) {

                    var enumeratedType = wholeModel.types.find(x => x.typeDisplayFullName === propTypeDescription.arrayDescription.elementDiplayFullTypeName)
                    result += `${TSClassGenerator.GetDescription(prop)}\t ${this.GetPropName(prop.propertyDescription.propertyName)}: Array<${TSClassGenerator.GetEnumeratedDisplayTypeName(enumeratedType)}>; \n`;

                    continue;
                }

                if (propTypeDescription.isClass || propTypeDescription.isEnumeration) {
                    result += `${TSClassGenerator.GetDescription(prop)}\t ${this.GetPropName(prop.propertyDescription.propertyName)}: ${TSClassGenerator.GetTypeDisplayName(propTypeDescription, false, this._useGenerics)}; \n`;

                    continue;
                }

                result += `${TSClassGenerator.GetDescription(prop)}\t ${this.GetPropName(prop.propertyDescription.propertyName)}: ${TsSimpleTypeMapper.GetPropertyType(propTypeDescription)}; \n`;
            }

            result += "}";

            return { IsGenerated: true, GeneratedText: result };
        }

        return { IsGenerated: false, GeneratedText: null};
    }

    private static GetEnumeratedDisplayTypeName(typeDescription: CrocoTypeDescription) : string {
        if (typeDescription.isClass || typeDescription.isEnumeration) {
            return TsSimpleTypeMapper.ExtractName(typeDescription.typeDisplayFullName);
        }

        return TsSimpleTypeMapper.ExtractName(TsSimpleTypeMapper.GetPropertyType(typeDescription));
    }

    public GenerateClassesForType(typeDescriptionResult: CrocoTypeDescriptionResult): string {

        const uniqueTypes = TSClassGenerator.GetUniqueTypes(typeDescriptionResult);
        console.log("GenerateClassesForType uniqueTypes", uniqueTypes);
        return uniqueTypes.map(x => this.GenerateTypeInterface(x, typeDescriptionResult)).filter(x => x.IsGenerated)
            .map(x => x.GeneratedText).join("\n\n\n");
    }
}