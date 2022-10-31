import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, Observable, Subject, takeUntil } from "rxjs";
import { ClientInfo } from "src/app/models/clientInfo";
import { ClientInfoDataService } from "../transient/clientInfo-data.service";
import { ClientInfoSignalRService } from "./clientInfo-signalR.service";

@Injectable()
export class ClientInfoService implements OnDestroy {

    private onDestroyed: Subject<void> = new Subject<void>();
    private clientInfo: ClientInfo[] = [];
    public clientInfoSubject$: BehaviorSubject<ClientInfo[]>;
    private clientInfoSignalRService!: ClientInfoSignalRService

    constructor(private clientInfoDataService: ClientInfoDataService) {
        this.clientInfoSubject$ = new BehaviorSubject<ClientInfo[]>(this.clientInfo);
        this.clientInfoDataService.getAllClientInfo().pipe(takeUntil(this.onDestroyed)).subscribe((clientInfo: ClientInfo[]) => {
            this.clientInfo = clientInfo;
            this.clientInfoSubject$.next(this.clientInfo);
        });
    }

    public setClientInfoSignalRService(clientInfoSignalRService: ClientInfoSignalRService): void {
        this.clientInfoSignalRService = clientInfoSignalRService;
    }

    public createClient(clientInfo: ClientInfo, virtual: boolean = false): void {
        if(virtual) {
            this.clientInfo.push(clientInfo);
            this.clientInfoSubject$.next(this.clientInfo);
            return;
        }
        this.clientInfoDataService.createClientInfo(clientInfo).subscribe((clientInfo: ClientInfo) => {
            this.clientInfo.push(clientInfo);
            this.clientInfoSubject$.next(this.clientInfo);
            this.clientInfoSignalRService.sendCreateClient(clientInfo);
        });
    }

    public updateClient(clientInfo: ClientInfo, virtual: boolean = false): void {
        if(virtual) {
            const index = this.clientInfo.findIndex((ci: ClientInfo) => ci.id === clientInfo.id);
            this.clientInfo[index] = clientInfo;
            this.clientInfoSubject$.next(this.clientInfo);
            return;
        }
        this.clientInfoDataService.updateClientInfo(clientInfo).subscribe((res: ClientInfo) => {
            const index = this.clientInfo.findIndex((ci: ClientInfo) => ci.id === res.id);
            this.clientInfo[index] = res;
            this.clientInfoSubject$.next(this.clientInfo);
            this.clientInfoSignalRService.sendUpdateClient(res);
        });
    }

    public deleteClient(clientInfo: ClientInfo, virtual: boolean = false): void {
        if(virtual) {
            const index = this.clientInfo.findIndex((ci: ClientInfo) => ci.id === clientInfo.id);
            this.clientInfo = this.clientInfo.filter(x => x.id != clientInfo.id);
            this.clientInfoSubject$.next(this.clientInfo);
            return;
        }
        this.clientInfoDataService.deleteClientInfo(clientInfo.id).subscribe((clientInfo: ClientInfo) => {
            const index = this.clientInfo.findIndex((clientInfo: ClientInfo) => clientInfo.id === clientInfo.id);
            this.clientInfo = this.clientInfo.filter(x => x.id != clientInfo.id);
            this.clientInfoSubject$.next(this.clientInfo);
            this.clientInfoSignalRService.sendDeleteClient(clientInfo);
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