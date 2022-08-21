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

  setStorage(token: string, id: number){//ne treba ja msm al et
    localStorage.setItem('token', token);
    localStorage.setItem('id', id.toString());
    this.token = localStorage.getItem('token');//nebitno
    this.user = this.authService.getUser(this.token);//nebitno
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


