import { CrocoPropertyAttribute } from "./CrocoPropertyAttribute";
export interface CrocoPropertyDescription {
    propertyName: string;
    propertyDisplayName: string;
    description: string;
    descriptions: Array<string>;
    attributes: Array<CrocoPropertyAttribute>;
}