import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserDetails, UserUpdate } from '../models/user';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  baseUrl = environment.baseUrl;
  constructor( private http: HttpClient) { }

  getUserDetails(id: number): Observable<UserDetails> {
    return this.http.get<UserDetails>(this.baseUrl + '/account/details/'+ id.toString());
  }

  updateUserDetails(id:number, user: UserUpdate){
    return this.http.post(this.baseUrl + '/account/update/' + id.toString(), user);
  }

  updateUserPhoto(id:number,file: FormData ) {
    return this.http.post(this.baseUrl + '/account/photo/' + id.toString(), file);
  }
}

