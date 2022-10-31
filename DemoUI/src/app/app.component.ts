import { Component, ElementRef, HostListener, Inject, ViewChild } from '@angular/core';
import { ClientInfo } from './models/clientInfo';
import { ClientInfoService } from './services/singleton/clientInfo.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ClientInfoSignalRService } from './services/singleton/clientInfo-signalR.service';
import { UserInfo } from './models/userInfo';
import { AlertifyService } from './services/singleton/alertifyService';
import { timer } from 'rxjs';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ClientInfoService, ClientInfoSignalRService]
})
export class AppComponent {
  public clients: ClientInfo[] = [];
  public selectedClient!: ClientInfo;
  public editClientInfo!: ClientInfo;
  public userList: any[] = [];

  public throttleTime: number = 50;  //We will only send the last mousemove event every {throttleTime} milliseconds
  public throttleHelper!: any;       //keep track of the last mousemove events

  @ViewChild('addClientModal') addClientModal!: ElementRef;

  @HostListener('mousemove', ['$event'])
  onMousemove(event: MouseEvent) {
    this.throttleHelper = { x: event.pageX, y: event.pageY };
  }

  constructor(private clientInfoService: ClientInfoService, 
              private clientInfoSignalRService: ClientInfoSignalRService, 
              private modalService: NgbModal,
              private alertifyService: AlertifyService,
              @Inject(DOCUMENT) private document: Document)
  {
    this.clientInfoSignalRService.startConnection();
    this.clientInfoService.setClientInfoSignalRService(this.clientInfoSignalRService);
    this.clientInfoService.clientInfoSubject$.subscribe((clientInfo: ClientInfo[]) => {
      this.clients = clientInfo.sort((a, b) => a.id - b.id);
      var selectedClientId = sessionStorage.getItem('selectedClientId');
      if(selectedClientId) {
        var sel = this.clients.find(x => x.id.toString() == selectedClientId);
        if(sel)
          sel._selected = true;
      }
    });
    this.clientInfoSignalRService.hubUserSubject$.subscribe((userInfo: UserInfo[]) => {
      this.userList = userInfo;
    });
    //Send the last mousemove event, but throttle to the throttleTime
    timer(0,this.throttleTime).subscribe(() => {
      if(this.throttleHelper) {
        this.clientInfoSignalRService.sendMouseEvent(this.throttleHelper);
      }
    });
  }

  editClient(client: ClientInfo, event: any) {
    event.stopPropagation();
    if(client._isBeingEdited) {
      this.alertifyService.warning(`This client is already being edited by ${client._editedBy}`);
      return;
    }
    this.clientInfoSignalRService.sendStartEditing(client);
    this.editClientInfo = new ClientInfo();
    Object.assign(this.editClientInfo, client);
    this.modalService.open(this.addClientModal,{backdrop: 'static', keyboard: false}).result.then((result) => {
      console.log(result);
      if(result == 'save') {
        console.log(this.editClientInfo);
        this.clientInfoService.updateClient(this.editClientInfo);
      }
      this.clientInfoSignalRService.sendStopEditing(client);
    });
  }

  deleteClient(client: ClientInfo, event: any) {
    event.stopPropagation();
    if(client._isBeingEdited) {
      this.alertifyService.warning(`This client is being edited by ${client._editedBy}`);
      return;
    }
    if(client._selected) {
      sessionStorage.removeItem('selectedClientId');
      client._selected = false;
    }
    this.clientInfoService.deleteClient(client);
  }

  getSelectedClient(): string {
    var sel = this.clients.find(x => x._selected);
    if(sel) {
      return sel.firstName + " " + sel.lastName;
    }
    return "";
  }

  selectClient(client: ClientInfo) {
    var alreadySelected = client._selected;
    this.clients.map(x => x._selected = false);
    client._selected = alreadySelected ? false : true;
    sessionStorage.setItem('selectedClientId', client._selected ? client.id.toString() : '');
  }

  openAddClientModal(content: any) {
    this.editClientInfo = new ClientInfo();
    this.modalService.open(content).result.then((result) => {
      if(result == 'save') {
        this.clientInfoService.createClient(this.editClientInfo);
      }
    });
  }

  fnChange(e: any) {
    this.editClientInfo.firstName = e.target.value;
  }
  lnChange(e: any) {
    this.editClientInfo.lastName = e.target.value;
  }
  emailChange(e: any) {
    this.editClientInfo.email = e.target.value;
  }

  positionUserDiv(user: UserInfo) {
    return `top: ${user.cursorY - (this.document.defaultView?.scrollY ?? 0)}px; left: ${user.cursorX}px;`;
  }
}
