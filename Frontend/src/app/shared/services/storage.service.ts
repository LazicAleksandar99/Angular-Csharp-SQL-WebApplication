import { Injectable } from '@angular/core';
import { UserTokenModel } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  user: UserTokenModel;//ma da cim se refreshuje odma nestane... mozda nije lose u storage, ali vidjet cemo
  token: any;

constructor() { }

  setStorage(token: string, id: number){
    localStorage.setItem('token', token);
    localStorage.setItem('id', id.toString());
    this.token = localStorage.getItem('token');
    this.user = this.getUser(this.token);
  }

  private getUser(token: string): UserTokenModel{
    console.log(token);
    return JSON.parse(atob(token.split('.')[1])) as UserTokenModel;;
  }

  hasRole(role: string): boolean {
    return this.user.role.includes(role) || false;
  }
}


