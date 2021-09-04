import { GenerateGenericUserInterfaceModel } from "./GenerateGenericUserInterfaceModel";

export interface RenderGenericInterface {
    /**
     *  Идентификатор Html элемента куда нужно вставить данную автоформу
     * */
    ElementId: string;
    /*
     * По какому ключу отрисовывать форму
     * */
    FormDrawKey: string;
    FormModel: GenerateGenericUserInterfaceModel;
}