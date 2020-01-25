declare class CookieWorker {
    setCookie(name: string, value: string, days: number): void;
    getCookie(name: string): string;
    eraseCookie(name: string): void;
}
