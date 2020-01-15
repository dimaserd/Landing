interface GetListResult<TModel> {
    /**
     * Сколько нужно взять из списка
     * */
    Count: number;
    /**
     * Сколько нужно пропустить в списке
     * */
    OffSet: number;
    TotalCount: number;
    /**
     * Список сущностей выбраных из списка
     * */
    List: Array<TModel>;
}
