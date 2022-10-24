export class ClientInfo
{
    id!: number;
    firstName: string = "";
    lastName: string = "";
    email: string = "";
    created?: Date;
    lastUpdated?: Date;
    _selected: boolean = false;

    constructor(init?: Partial<ClientInfo>) {
        Object.assign(this, init);
    }
}