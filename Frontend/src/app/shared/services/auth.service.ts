import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserForRegister, UserForLogin, UserTokenModel } from '../models/user';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

    user: UserTokenModel;
    userId: number;
    theToken: any;

    baseUrl = environment.baseUrl;
    constructor(private http: HttpClient) { }

    getUser(token: string): UserTokenModel{
      return JSON.parse(atob(token.split('.')[1])) as UserTokenModel;
    }

    getUserId(token: string): number{
      this.user = JSON.parse(atob(token.split('.')[1]));
      this.userId = Number(this.user.nameid);
      return this.userId;
    }

    getUserVerificationStatus(token: string): string{
      this.user = JSON.parse(atob(token.split('.')[1]));
      return this.user.given_name;
    }

    hasRole(role: string): boolean {
      this.theToken = localStorage.getItem('token');
      this.user = this.getUser(this.theToken);
      return this.user.role.includes(role) || false;
    }

    authUser(user:UserForLogin) {
      return this.http.post(this.baseUrl + '/account/login', user);
    }

    registerUser(user: UserForRegister){
      return this.http.post(this.baseUrl + '/account/register',user);
    }
}
