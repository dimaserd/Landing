import { JsSerializedVariable } from "./JsSerializedVariable";

export interface JsLogggedVariables {
    loggedOnUtc: Date;
    serializedVariables: Array<JsSerializedVariable>;
}