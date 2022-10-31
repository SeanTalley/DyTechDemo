export class UserInfo
{
    userName: string = "";
    cursorX: number = 0;
    cursorY: number = 0;
    _display: boolean = true;
    constructor(init?: Partial<UserInfo>) {
        Object.assign(this, init);
    }
}