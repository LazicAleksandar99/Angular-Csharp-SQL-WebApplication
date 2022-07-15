import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddProduct, Product } from '../models/product';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient,private storageService: StorageService) { }

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl + '/product/list', this.storageService.getHttpHeader());
  }

  addProduct(product: AddProduct){
    return this.http.post(this.baseUrl + '/product/add',product,this.storageService.getHttpHeader());
  }
}
