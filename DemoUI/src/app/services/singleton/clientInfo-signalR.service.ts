import { EventEmitter, Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { BehaviorSubject } from "rxjs";
import { ClientInfo } from "src/app/models/clientInfo";
import { UserInfo } from "src/app/models/userInfo";
import { environment } from "src/environments/environment";
import { AlertifyService } from "./alertifyService";
import { ClientInfoService } from "./clientInfo.service";

@Injectable({
    providedIn: "root",
})
export class ClientInfoSignalRService {
    private hubConnection!: signalR.HubConnection;
    public hubConnectionId: string | null = null;
    public hubUserName: string | null = null;
    public hubUserList: UserInfo[] = [];
    public hubUserSubject$: BehaviorSubject<UserInfo[]>;
    public lastMouseEvent: any = null;

    constructor(private clientInfoService: ClientInfoService, private alertifyService: AlertifyService) {
        this.hubUserSubject$ = new BehaviorSubject<UserInfo[]>(this.hubUserList);
    }
    
    public startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${environment.apiRoot}/clientHub`)
            .build();
        this.hubConnection
            .start()
            .then(() => { 
                this.hubConnectionId = this.hubConnection.connectionId;
                console.log("ClientHub Service started");
                this.hubConnection.on("UserName", (userName: string) => {
                    this.hubUserName = userName;
                });
                this.hubConnection.on("UserList",(userList: UserInfo[]) => {
                    this.hubUserList = userList;
                    this.hubUserSubject$.next(this.hubUserList);
                });
                this.hubConnection.on("UserJoined", (user: UserInfo) => {
                    console.log(user, "User joined");
                    if(this.hubUserList.find(u => u.userName == user.userName) == undefined) {
                        this.hubUserList.push(user);
                        this.hubUserSubject$.next(this.hubUserList);
                    }
                });
                this.hubConnection.on("UserLeft", (user: UserInfo) => {
                    this.hubUserList = this.hubUserList.filter(x => x.userName != user.userName);
                    this.hubUserSubject$.next(this.hubUserList);
                });
                this.hubConnection.on("MouseMove", (user: UserInfo) => {
                    var id = this.hubUserList.findIndex(x => x.userName == user.userName);
                    this.hubUserList[id].cursorX = user.cursorX;
                    this.hubUserList[id].cursorY = user.cursorY;
                    this.hubUserSubject$.next(this.hubUserList);
                });
                this.hubConnection.on("StartEditing", (clientInfo: ClientInfo, userInfo: UserInfo) => {
                    clientInfo._isBeingEdited = true;
                    clientInfo._editedBy = userInfo.userName;
                    this.clientInfoService.updateClient(clientInfo,true);
                });
                this.hubConnection.on("StopEditing", (clientInfo: ClientInfo) => {
                    clientInfo._isBeingEdited = false;
                    clientInfo._editedBy = null;
                    this.clientInfoService.updateClient(clientInfo,true);
                });
                this.hubConnection.on("UpdateClient", (clientInfo: ClientInfo, userInfo: UserInfo) => {
                    this.alertifyService.warning(`Client ${clientInfo.firstName} ${clientInfo.lastName} updated by ${userInfo.userName}`);
                    this.clientInfoService.updateClient(clientInfo,true);
                });
                this.hubConnection.on("DeleteClient", (clientInfo: ClientInfo, userInfo: UserInfo) => {
                    this.alertifyService.warning(`Client ${clientInfo.firstName} ${clientInfo.lastName} deleted by ${userInfo.userName}`);
                    this.clientInfoService.deleteClient(clientInfo,true);
                });
                this.hubConnection.on("CreateClient", (clientInfo: ClientInfo, userInfo: UserInfo) => {
                    this.alertifyService.warning(`Client ${clientInfo.firstName} ${clientInfo.lastName} created by ${userInfo.userName}`);
                    this.clientInfoService.createClient(clientInfo,true);
                });
            })
            .catch(err => console.log("Error while starting connection: " + err));
    }

    public sendMouseEvent(event: any) {
        if(this.hubConnection.connectionId) {
            //Only send event if the mouse has moved
            if(this.lastMouseEvent == null || this.lastMouseEvent.x != event.x || this.lastMouseEvent.y != event.y) {
                this.lastMouseEvent = event;
                this.hubConnection.invoke("MouseMove", event.x, event.y);
            }
        }
    }

    public sendStartEditing(clientInfo: ClientInfo) {
        if(this.hubConnection.connectionId)
            this.hubConnection.invoke("StartEditing", clientInfo);
    }

    public sendStopEditing(clientInfo: ClientInfo) {
        if(this.hubConnection.connectionId)
            this.hubConnection.invoke("StopEditing", clientInfo);
    }

    public sendUpdateClient(clientInfo: ClientInfo) {
        if(this.hubConnection.connectionId)
            this.hubConnection.invoke("UpdateClient", clientInfo);
    }

    public sendDeleteClient(clientInfo: ClientInfo) {
        if(this.hubConnection.connectionId)
            this.hubConnection.invoke("DeleteClient", clientInfo);
    }

    public sendCreateClient(clientInfo: ClientInfo) {
        if(this.hubConnection.connectionId)
            this.hubConnection.invoke("CreateClient", clientInfo);
    }

    public stopConnection = () => {
        this.hubConnection
            .stop()
            .then(() => { 
                console.log("ClientHub Service stopped") 
            })
            .catch(err => console.log("Error while stopping connection: " + err));
    }
}