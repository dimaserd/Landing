interface BaseApiResponse {
    IsSucceeded: boolean;
    Message: string;
}
declare class IndexHandlers {
    static Prefix: string;
    static SendRequest(): void;
    static SetHandlers(): void;
}
