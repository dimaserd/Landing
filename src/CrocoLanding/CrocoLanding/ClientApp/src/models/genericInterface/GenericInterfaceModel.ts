import { UserInterfaceBlock } from "./UserInterfaceBlock";

export interface GenericInterfaceModel {
    prefix: string; 
    blocks: Array<UserInterfaceBlock>; 
}