import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ClientInfo } from "src/app/models/clientInfo";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class ClientInfoDataService {
    
    constructor(private http: HttpClient) { }

    public getAllClientInfo(): Observable<ClientInfo[]> {
        return this.http.get<ClientInfo[]>(`${environment.apiRoot}/ClientInfo`);
    }
    public getClientInfo(id: number): Observable<ClientInfo> {
        return this.http.get<ClientInfo>(`${environment.apiRoot}/ClientInfo/${id}`);
    }
    public createClientInfo(clientInfo: ClientInfo): Observable<ClientInfo> {
        return this.http.post<ClientInfo>(`${environment.apiRoot}/ClientInfo`, clientInfo);
    }
    public updateClientInfo(clientInfo: ClientInfo): Observable<ClientInfo> {
        return this.http.put<ClientInfo>(`${environment.apiRoot}/ClientInfo/${clientInfo.id}`, clientInfo);
    }
    public deleteClientInfo(id: number): Observable<ClientInfo> {
        return this.http.delete<ClientInfo>(`${environment.apiRoot}/ClientInfo/${id}`);
    }
}