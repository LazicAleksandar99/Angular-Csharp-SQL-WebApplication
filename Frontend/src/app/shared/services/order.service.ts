import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order, PendingOrder } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.baseUrl;
  id: any;
  constructor( private http: HttpClient) { }

  makeOrder(order: Order[],comment: string){
    this.id = localStorage.getItem('id');
    return this.http.post(this.baseUrl + '/order/make/' + this.id + '/' + comment,order );
  }

  getPendingOrder(): Observable<PendingOrder[]>{
    return this.http.get<PendingOrder[]>(this.baseUrl + '/order/orders');
  }

}
