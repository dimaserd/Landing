import { UserInterfaceNumberBoxData } from "./UserInterfaceNumberBoxData";
import { UserInterfaceType } from "./UserInterfaceType";
import { GenericInterfaceModel } from "./GenericInterfaceModel";
import { DropDownListData } from "./DropDownListData";
import { AutoCompleteData } from "./AutoCompleteData";
export interface UserInterfaceBlock {
    propertyName: string; 
    labelText: string; 
    isVisible: boolean; 
    interfaceType: UserInterfaceType; 
    customUserInterfaceType: string; 
    dropDownData: DropDownListData; 
    numberBoxData: UserInterfaceNumberBoxData; 
    customDataJson: string; 
    innerGenericInterface: GenericInterfaceModel; 
    autoCompleteData: AutoCompleteData; 
    typeDisplayFullName: string; 
}