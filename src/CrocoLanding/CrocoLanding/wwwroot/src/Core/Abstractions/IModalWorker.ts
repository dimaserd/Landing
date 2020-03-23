interface IModalWorker {
    /** Показать модальное окно по идентификатору. */
    ShowModal(modalId: string): void;
    ShowLoadingModal(): void;
    HideModal(modalId: string): void;
    HideModals(): void;
}
