import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

constructor() { }

  setStorage(token: string, id: number){
    sessionStorage.setItem('token', token);
    sessionStorage.setItem('id', id.toString());
  }

}


