import { CrocoTypeDescription, CrocoTypeDescriptionResult } from "src/models/typings";

export class DefaultJsonGenerator {
    
    _data:CrocoTypeDescriptionResult
    
    _primitives = new Map<string, string>()
        .set("System.String", `"SomeString"`)
        .set("System.Int32", "0")
        .set("System.DateTime", new Date().toISOString())
        .set("System.Double", "3.14")
        .set("System.Boolean", "true")
        .set("System.Int64", "1")
        .set("System.Int32?", "0")
        .set("System.DateTime?", new Date().toISOString())
        .set("System.Double?", "3.14")
        .set("System.Boolean?", "true")
        .set("System.Int64?", "1");

    constructor(data: CrocoTypeDescriptionResult){
        this._data = data;
    }

    getExampleJson():string{
        const main = this._data.types.find(x => x.typeDisplayFullName === this._data.typeDisplayFullName);
        
        var json = this.applyTabbing(this.getJsonAsStrings(main, 0), 0).join('\n');

        return JSON.stringify(JSON.parse(json), null, 2);
    }

    private getTabsByLevel(level : number) : string{
        var str = "";

        for (let index = 0; index < level; index++) {
            str += "\t";
        }
        return str;
    }

    private applyTabbing(strs:string[], tabLevel: number) : string[]{
        
        return strs.map(x => this.getTabsByLevel(tabLevel) + x);
    }

    private getJsonAsStrings(data: CrocoTypeDescription, tablevel: number) : string[] {
        if(data.isPrimitive){
            return [`${this._primitives.get(data.typeDisplayFullName)}`];
        }

        if(data.isClass){

            var json = [`{`];

            for (let index = 0; index < data.properties.length; index++) {
                const prop = data.properties[index];
                
                const commaAndNewStr = index === data.properties.length-1? '' : ','; 

                let type = this._data.types.find(x => x.typeDisplayFullName === prop.typeDisplayFullName);
                let innerRes = this.getJsonAsStrings(type, tablevel + 1);

                //console.log("Inner result for Class", type, innerRes);

                if(type.isPrimitive){
                    json.push(`\t"${prop.propertyDescription.propertyName}": ${innerRes}` + commaAndNewStr);
                }
                else if(type.isClass){
                    json.push(`\t"${prop.propertyDescription.propertyName}": `);
                    json = json.concat(this.applyTabbing(innerRes, tablevel+1));
                    json.push(commaAndNewStr);
                }
                else if(type.arrayDescription.isArray){
                    json.push(`\t"${prop.propertyDescription.propertyName}": `);
                    json = json.concat(this.applyTabbing(innerRes, tablevel+1));
                    json.push(commaAndNewStr);
                }
            }

            json.push(`}`);

            return json;
        }

        if(data.arrayDescription.isArray){
            var json = [`[`];

            let type = this._data.types.find(x => x.typeDisplayFullName === data.arrayDescription.elementDiplayFullTypeName);
            let innerRes = this.getJsonAsStrings(type, tablevel + 1);

            if(type.isPrimitive){
                json = json.concat(this.applyTabbing(innerRes, tablevel));
            }
            else if(type.arrayDescription.isArray){
                json = json.concat(this.applyTabbing(innerRes, tablevel+1));
            }
            else if(type.isClass){
                json = json.concat(this.applyTabbing(innerRes, tablevel+2));
            }
            else {
                json.push(`\t${innerRes}`);
            }

            json.push(']');
            return json;
        }

        return ["notMapped"];
    }
}