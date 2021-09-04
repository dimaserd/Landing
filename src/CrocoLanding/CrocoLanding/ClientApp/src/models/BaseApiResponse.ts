
export interface BaseApiResponse {
    isSucceeded: boolean;
    message: string;
}

export interface GenericBaseApiResponse<T> extends BaseApiResponse {
    responseObject: T;
}