import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AcceptOrder, CurrentOrder, Order, PendingOrder, StatusOrder } from '../models/order';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.baseUrl;
  id: any;
  constructor( private http: HttpClient,private storageService: StorageService) { }

  makeOrder(order: Order[],comment: string){
    this.id = localStorage.getItem('id');

    return this.http.post(this.baseUrl + '/order/make/' + this.id + '/' + comment,order,this.storageService.getHttpHeader() );
  }

  getPendingOrder(): Observable<PendingOrder[]>{

    return this.http.get<PendingOrder[]>(this.baseUrl + '/order/pending',this.storageService.getHttpHeader());
  }

  getAllOrders(): Observable<StatusOrder[]>{

    return this.http.get<StatusOrder[]>(this.baseUrl + '/order/all',this.storageService.getHttpHeader());
  }

  getOrderHistory(id: number): Observable<StatusOrder[]>{

    return this.http.get<StatusOrder[]>(this.baseUrl + '/order/executed/' + id.toString(),this.storageService.getHttpHeader());
  }

  acceptOrder(orderId: number,deliverer: AcceptOrder) {

    return this.http.post(this.baseUrl + '/order/accept/' + orderId.toString(),deliverer,this.storageService.getHttpHeader());
  }

  getCurrentOrders(): Observable<CurrentOrder[]> {

    this.id = localStorage.getItem('id');

    return this.http.get<CurrentOrder[]>(this.baseUrl + '/order/current/' + this.id.toString(),this.storageService.getHttpHeader());
  }

}
