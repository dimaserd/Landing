import { CrocoPropertyReferenceDescription } from "./CrocoPropertyReferenceDescription";
import { CrocoEnumTypeDescription } from "./CrocoEnumTypeDescription";
import { CrocoGenericTypeDescription } from "./CrocoGenericTypeDescription";
import { CrocoArrayDescription } from "./CrocoArrayDescription";
export interface CrocoTypeDescription {
    /* Полное название типа */
	 fullTypeName: string; 
     /* Название типа, короткое название */
      typeName: string; 
     /* Является ли тип обобщенным */
      isGeneric: boolean; 
     /* Описание для обобщенного типа */
      genericDescription: CrocoGenericTypeDescription; 
     /* Описание массива */
      arrayDescription: CrocoArrayDescription; 
     /* Является ли данный тип примитивным */
      isPrimitive: boolean; 
     /* Полное название типа для показа */
      typeDisplayFullName: string; 
     /* Является ли данный тип Nullable */
      isNullable: boolean; 
     /* Является ли данный тип перечислением */
      isEnumeration: boolean; 
     /* Является ли данный тип составным */
      isClass: boolean; 
     /* Описание перечисления */
      enumDescription: CrocoEnumTypeDescription; 
     /* Свойства типа, если данный тип является классом, здесь будут перечислены свойства этого класса */
      properties: Array<CrocoPropertyReferenceDescription>; 
}