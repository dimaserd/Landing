import { ExternalJsComponent } from "./ExternalJsComponent";
import { JsOpenApiWorkerMethodDocumentation } from "./JsOpenApiWorkerMethodDocumentation";

export interface JsOpenApiDocs {
    workers: Array<JsOpenApiWorkerMethodDocumentation>;
    externalJsComponents: Array<ExternalJsComponent>; 
}