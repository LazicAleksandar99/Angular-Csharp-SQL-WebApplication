import { Injectable } from '@angular/core';
declare let alertify : any;

@Injectable({
  providedIn: 'root'
})
//NO USE DELATE
export class AlertifyService {

constructor() { }
  success(message: string) {
   // this.alertyfy.success(message);
  }

  warning(message: string) {
   // this.alertyfy.warning(message);
  }

  error(message: string) {
  //  alertyfy.error(message);
  }

}
