import { CrocoTypeDescriptionResult } from "../typings/CrocoTypeDescriptionResult";
import { GenericInterfaceModel } from "./GenericInterfaceModel";

export interface GenerateGenericUserInterfaceModel {
    interface: GenericInterfaceModel; 
    customDataJson: string; 
    valueJson: string; 
    typeDescription: CrocoTypeDescriptionResult; 
}