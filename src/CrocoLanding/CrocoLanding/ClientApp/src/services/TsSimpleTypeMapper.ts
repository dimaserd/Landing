import { CrocoTypeDescription } from "src/models/typings";

export class TsSimpleTypeMapper {
    static simpleTypes = ["String", "Int32", "Int64", "Decimal", "Boolean", "DateTime"];
    static typesDictionary = new Map<string, string>()
        .set("String", "string")
        .set("Int32", "number")
        .set("Int64", "number")
        .set("Decimal", "number")
        .set("Double", "number")
        .set("Boolean", "boolean")
        .set("DateTime", "Date");

    static GetPropertyType(typeDescription: CrocoTypeDescription): string {
        return TsSimpleTypeMapper.GetPropertyTypeByTypeDisplayName(typeDescription.typeDisplayFullName);
    }
    static GetPropertyTypeByTypeDisplayName(typeDisplayName: string): string {
        
        let name = TsSimpleTypeMapper.ExtractName(typeDisplayName);

        let isNullable = name.endsWith('?');

        name = name.replace('?', '');

        var result = TsSimpleTypeMapper.getJsTypeName(name);

        if(isNullable){
            result += " | null";
        }
        return result;
    }

    static getJsTypeName(cSharpName: string) : string{

        console.log("getJsTypeName", cSharpName);
        if(cSharpName === "Guid"){
            return "string";
        }

        return TsSimpleTypeMapper.typesDictionary.get(cSharpName);
    }

    static ExtractName(name: string): string {
        
        const d = name.split('.');

        return d[d.length - 1];
    }
}