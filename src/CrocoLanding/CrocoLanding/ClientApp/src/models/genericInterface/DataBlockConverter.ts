import { UserInterfaceBlock } from "./UserInterfaceBlock";
import { UserInterfaceType } from "./UserInterfaceType";



export interface IValueProccessor{
  processValue(value: string) : any;
}

export class NullableBooleanValueProccessor implements IValueProccessor{
  processValue(value: string) {
      if(value === null){
          return null;
      }
      return value.toLowerCase() === 'true';
  }
}

export class DefaultValueProccessor implements IValueProccessor{
  processValue(value: string) {
      return value;
  }

}

export class DataBlockConverter {

    _block: UserInterfaceBlock;

    constructor(block: UserInterfaceBlock) {
        this._block = block;
    }

    public ConvertBlock(): UserInterfaceBlock {

        if (this._block.interfaceType === UserInterfaceType.GenericInterfaceForArray || this._block.interfaceType === UserInterfaceType.GenericInterfaceForClass) {

            var blocks = this._block.innerGenericInterface.blocks;
            for (let index = 0; index < blocks.length; index++) {
                blocks[index] = new DataBlockConverter(blocks[index]).ConvertBlock();
            }

            return this._block;
        }

        if (this._block.interfaceType === UserInterfaceType.MultipleDropDownList || this._block.interfaceType === UserInterfaceType.DropDownList) {
            let array = this._block.dropDownData.selectList;

            let valueProcessor = this.GetValueProccessor(this._block.typeDisplayFullName);

            for (let index = 0; index < array.length; index++) {
                const element = array[index];

                element.value = valueProcessor.processValue(element.value);
            }

            return this._block;
        }

        return this._block;
    }

    private GetValueProccessor(typeDisplayFullName: string): IValueProccessor {
        if (typeDisplayFullName === "System.Boolean" || typeDisplayFullName === "System.Boolean?") {
            return new NullableBooleanValueProccessor();
        }

        return new DefaultValueProccessor();
    }
}
