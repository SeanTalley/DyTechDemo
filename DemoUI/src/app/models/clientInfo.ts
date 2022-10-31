export class ClientInfo
{
    id!: number;
    firstName: string = "";
    lastName: string = "";
    email: string = "";
    created?: Date;
    lastUpdated?: Date;
    _selected: boolean = false;
    _isBeingEdited: boolean = false;
    _editedBy: string | null = null;

    constructor(init?: Partial<ClientInfo>) {
        Object.assign(this, init);
    }
}