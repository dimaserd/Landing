interface GetListResult<TModel> {

    /**
     * Сколько нужно взять из списка
     * */
    Count: number;

    /**
     * Сколько нужно пропустить в списке
     * */
    OffSet: number;

    /*
     * Всего в списке
     */
    TotalCount: number;

    /**
     * Список сущностей выбраных из списка
     * */
    List: Array<TModel>;
}

