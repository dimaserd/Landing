interface ICrocoRequester {

    /**
     * Загрузить файл на сервер
     * @param inputId идентификатор инпута, в котором лежит файл
     * @param link  ссылка на метод апи
     * @param onSuccessFunc  обработчик успешного ответа от сервера
     * @param onErrorFunc    обработчик неуспешного ответа от сервера
     */
    UploadFilesToServer<TObject>(inputId: string, link: string, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;

    /**
     * Сделать GET запрос на сервер
     * @param link  адрес, по которому будет совершен запрос
     * @param data  данные, котороые отправятся на сервер
     * @param onSuccessFunc обработчик успешного ответа от сервера
     * @param onErrorFunc   обработчик неуспешного ответа от сервера
     */
    Get<TObject>(link: string, data: Object, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;

    /**
     * Сделать POST запрос на сервер
     * @param link  адрес, по которому будет совершен запрос
     * @param data  данные, котороые отправятся на сервер
     * @param onSuccessFunc обработчик успешного ответа от сервера
     * @param onErrorFunc   обработчик неуспешного ответа от сервера
     */
    Post<TObject>(link: string, data: Object, onSuccessFunc: (x: TObject) => void, onErrorFunc: Function): void;
}