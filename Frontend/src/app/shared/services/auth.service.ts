import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserForRegister } from '../models/user';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

    baseUrl = environment.baseUrl;
    constructor(private http: HttpClient) { }

    registerUser(user: UserForRegister){
      return this.http.post(this.baseUrl + '/account/register',user);
    }
}
