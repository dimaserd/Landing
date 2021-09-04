import { JsOpenApiWorkerMethodDocumentation } from "./JsOpenApiWorkerMethodDocumentation";

export interface JsOpenApiWorkerDocumentation {
    workerName: string;
    description: string;
    methods: Array<JsOpenApiWorkerMethodDocumentation>;
}