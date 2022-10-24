import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, Observable, Subject, takeUntil } from "rxjs";
import { ClientInfo } from "src/app/models/clientInfo";
import { ClientInfoDataService } from "../transient/clientInfo-data.service";

@Injectable()
export class ClientInfoService implements OnDestroy {

    private onDestroyed: Subject<void> = new Subject<void>();
    private clientInfo: ClientInfo[] = [];
    public clientInfoSubject$: BehaviorSubject<ClientInfo[]>;

    constructor(private clientInfoDataService: ClientInfoDataService) {
        this.clientInfoSubject$ = new BehaviorSubject<ClientInfo[]>(this.clientInfo);
        this.clientInfoDataService.getAllClientInfo().pipe(takeUntil(this.onDestroyed)).subscribe((clientInfo: ClientInfo[]) => {
            this.clientInfo = clientInfo;
            this.clientInfoSubject$.next(this.clientInfo);
        });
    }

    public createClient(clientInfo: ClientInfo): void {
        this.clientInfoDataService.createClientInfo(clientInfo).subscribe((clientInfo: ClientInfo) => {
            this.clientInfo.push(clientInfo);
            this.clientInfoSubject$.next(this.clientInfo);
        });
    }

    public updateClient(clientInfo: ClientInfo): void {
        this.clientInfoDataService.updateClientInfo(clientInfo).subscribe((res: ClientInfo) => {
            const index = this.clientInfo.findIndex((ci: ClientInfo) => ci.id === res.id);
            this.clientInfo[index] = res;
            this.clientInfoSubject$.next(this.clientInfo);
        });
    }

    public deleteClient(clientInfo: ClientInfo): void {
        this.clientInfoDataService.deleteClientInfo(clientInfo.id).subscribe((clientInfo: ClientInfo) => {
            const index = this.clientInfo.findIndex((clientInfo: ClientInfo) => clientInfo.id === clientInfo.id);
            this.clientInfo = this.clientInfo.filter(x => x.id != clientInfo.id);
            this.clientInfoSubject$.next(this.clientInfo);
        });
    }

    //Not really needed here, but I like to have it for consistency - Sean Talley
    public getClient(clientId: number): Observable<ClientInfo> {
        return this.clientInfoDataService.getClientInfo(clientId);
    }

    public ngOnDestroy(): void {
        this.onDestroyed.next();
        this.onDestroyed.complete();
    }
}