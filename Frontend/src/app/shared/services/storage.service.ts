import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserTokenModel } from '../models/user';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  user: UserTokenModel;
  token: any;

constructor(private authService: AuthService) { }

  setStorage(token: string, id: number){
    localStorage.setItem('token', token);
    localStorage.setItem('id', id.toString());
    this.token = localStorage.getItem('token');
    this.user = this.authService.getUser(this.token);
  }

  setSelectedPendingOrder(id: number){
    localStorage.setItem('selectedOrder',id.toString());
  }

  getHttpHeader(): { headers: HttpHeaders; }{
    const httpOptions = {
      headers: new HttpHeaders({
          Authorization: 'Bearer '+ localStorage.getItem('token')
      })
    };
    return httpOptions;
  }
}


