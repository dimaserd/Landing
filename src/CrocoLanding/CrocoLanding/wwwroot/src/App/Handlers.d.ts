interface BaseApiResponse {
    IsSucceeded: boolean;
    Message: string;
}
declare class IndexHandlers {
    static Prefix: string;
    static isEmptyOrSpaces(str: string): boolean;
    static SendRequest(): void;
    static SetHandlers(): void;
}
