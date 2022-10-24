import { Component, ElementRef, ViewChild } from '@angular/core';
import { ClientInfo } from './models/clientInfo';
import { ClientInfoService } from './services/singleton/clientInfo.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ClientInfoService]
})
export class AppComponent {
  public clients: ClientInfo[] = [];
  public selectedClient!: ClientInfo;
  public editClientInfo!: ClientInfo;
  @ViewChild('addClientModal') addClientModal!: ElementRef;

  constructor(private clientInfoService: ClientInfoService, private modalService: NgbModal) {
    this.clientInfoService.clientInfoSubject$.subscribe((clientInfo: ClientInfo[]) => {
      this.clients = clientInfo.sort((a, b) => a.id - b.id);
      var selectedClientId = sessionStorage.getItem('selectedClientId');
      if(selectedClientId) {
        var sel = this.clients.find(x => x.id.toString() == selectedClientId);
        if(sel)
          sel._selected = true;
      }
    });
  }

  editClient(client: ClientInfo, event: any) {
    event.stopPropagation();
    this.editClientInfo = new ClientInfo();
    Object.assign(this.editClientInfo, client);
    this.modalService.open(this.addClientModal).result.then((result) => {
      console.log(result);
      if(result == 'save') {
        console.log(this.editClientInfo);
        this.clientInfoService.updateClient(this.editClientInfo);
      }
    });
  }

  deleteClient(client: ClientInfo, event: any) {
    event.stopPropagation();
    if(client._selected) {
      sessionStorage.removeItem('selectedClientId');
      client._selected = false;
    }
    console.log(client,"deleting client");
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
}
