import { CrocoTypeDescriptionResult } from "../typings";

export interface JsOpenApiWorkerMethodDocumentation {
    methodName: string;
    description: string;
    resultDescription: string;
    parameterDescriptions: string[];
    response: CrocoTypeDescriptionResult;
    parameters: Array<CrocoTypeDescriptionResult>;
}