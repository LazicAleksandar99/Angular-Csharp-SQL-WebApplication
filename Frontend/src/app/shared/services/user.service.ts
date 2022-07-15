import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { DelivererDetails, UserDetails, UserUpdate, VerifyUser } from '../models/user';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  baseUrl = environment.baseUrl;
  constructor( private http: HttpClient,private storageService: StorageService) { }

  getUserDetails(id: number): Observable<UserDetails> {
    return this.http.get<UserDetails>(this.baseUrl + '/account/details/'+ id.toString(), this.storageService.getHttpHeader());
  }

  updateUserDetails(id:number, user: UserUpdate){
    return this.http.post(this.baseUrl + '/account/update/' + id.toString(), user, this.storageService.getHttpHeader());
  }

  updateUserPhoto(id:number,file: FormData ) {
    return this.http.post(this.baseUrl + '/account/photo/' + id.toString(), file, this.storageService.getHttpHeader());
  }

  getAllDeliverers(): Observable<DelivererDetails[]>{
    return this.http.get<DelivererDetails[]>(this.baseUrl + '/account/delivers/', this.storageService.getHttpHeader());
  }

  verifyDeliverer(user:VerifyUser) {
    return this.http.post(this.baseUrl + '/account/verify' , user,this.storageService.getHttpHeader());
  }

  denyDeliverer(user:VerifyUser) {
    return this.http.post(this.baseUrl + '/account/deny' ,user, this.storageService.getHttpHeader());
  }

  getDelivererStatus(id: number): Observable<number>{
    return this.http.get<number>(this.baseUrl + 'account/status/' + id.toString(),this.storageService.getHttpHeader());
  }
}

