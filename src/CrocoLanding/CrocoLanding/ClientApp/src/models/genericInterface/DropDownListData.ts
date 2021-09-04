import { SelectListItem } from "./SelectListItem";

export interface DropDownListData {
    selectList: Array<SelectListItem>;
    canAddNewItem: boolean;
}