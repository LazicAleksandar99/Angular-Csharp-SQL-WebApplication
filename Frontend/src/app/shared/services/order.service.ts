import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.baseUrl;
  id: any;
  constructor( private http: HttpClient) { }

  makeOrder(order: Order[]){
    this.id = sessionStorage.getItem('id');
    console.log("s");
    console.log(this.id);
    console.log(sessionStorage.getItem('id'));
    return this.http.post(this.baseUrl + '/order/make/' + this.id,order);
  }
}
