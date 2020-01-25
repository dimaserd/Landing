declare class Requester_Resx {
    YouPassedAnEmtpyArrayOfObjects: string;
    ErrorOccuredWeKnowAboutIt: string;
    FilesNotSelected: string;
}
declare class Requester implements ICrocoRequester {
    static Resources: Requester_Resx;
    static GoingRequests: string[];
    DeleteCompletedRequest(link: string): void;
    PostWithAnimation<TObject>(link: string, data: Object, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;
    UploadFilesToServer<TObject>(inputId: string, link: string, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;
    static OnSuccessAnimationHandler(data: IBaseApiResponse): void;
    static OnErrorAnimationHandler(): void;
    Get<TObject>(link: string, data: Object, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;
    private SendAjaxPostInner;
    Post<TObject>(link: string, data: Object, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;
}
