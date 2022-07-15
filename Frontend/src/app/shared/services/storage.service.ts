import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserTokenModel } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  user: UserTokenModel;
  token: any;

constructor() { }

  setStorage(token: string, id: number){
    localStorage.setItem('token', token);
    localStorage.setItem('id', id.toString());
    this.token = localStorage.getItem('token');
    this.user = this.getUser(this.token);
  }

  private getUser(token: string): UserTokenModel{
    return JSON.parse(atob(token.split('.')[1])) as UserTokenModel;;
  }

  hasRole(role: string): boolean {
    this.token = localStorage.getItem('token');
    this.user = this.getUser(this.token);
    return this.user.role.includes(role) || false;
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


