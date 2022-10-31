import { Injectable } from '@angular/core';
declare let alertify: any;
@Injectable({
    providedIn: 'root'
})
export class AlertifyService {

    constructor() { }
    confirm(message: string, okCallBack: () => any) {
        alertify.confirm(message, function (e: any) {
            if (e) { okCallBack() } else { }
        });
    }

    success(message: string) {
        alertify.success(message).dismissOthers();
    }

    error(message: string) {
        alertify.error(message).dismissOthers();
    }
    warning(message: string) {
        alertify.warning(message).dismissOthers();
    }
    message(message: string) {
        alertify.message(message).dismissOthers();
    }

}