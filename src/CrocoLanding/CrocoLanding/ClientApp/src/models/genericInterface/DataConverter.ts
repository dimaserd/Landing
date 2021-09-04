import { DataBlockConverter } from "./DataBlockConverter";
import { GenerateGenericUserInterfaceModel } from "./GenerateGenericUserInterfaceModel";

export class DataConverter{

    _model: GenerateGenericUserInterfaceModel;

    constructor(model: GenerateGenericUserInterfaceModel){
        this._model = model;
    }

    ProccessValues(): GenerateGenericUserInterfaceModel{

        var blocks = this._model.interface.blocks;
        for (let index = 0; index < blocks.length; index++) {
            blocks[index] = new DataBlockConverter(blocks[index]).ConvertBlock();
        }

        return this._model;
    }

}
