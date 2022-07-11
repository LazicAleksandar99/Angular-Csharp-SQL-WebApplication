import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  public cartItemList : any =[]
  public productList = new BehaviorSubject<any>([]);
  public added = false;
  constructor() { }

  getProducts(){
    return this.productList.asObservable();
  }

  setProduct(product : any){
    this.cartItemList.push(...product);
    this.productList.next(product);
  }
  addtoCart(product : any){
    if(this.cartItemList.length != 0){
      this.cartItemList.map((a:any)=>{
        if(product.id === a.id){

          a.quantity += product.quantity;
          a.total += product.total;
          this.added = true;
        }
      })
      if(!this.added){
        this.cartItemList.push(product);
        this.productList.next(this.cartItemList);
        this.getTotalPrice();
      }
      this.added = false;
      console.log(this.cartItemList)
    }
    else{
      this.cartItemList.push(product);
      this.productList.next(this.cartItemList);
      this.getTotalPrice();
      console.log(this.cartItemList)
    }
  }
  getTotalPrice() : number{
    let grandTotal = 0;
    this.cartItemList.map((a:any)=>{
      grandTotal += a.total;
    })
    return grandTotal;
  }
  removeCartItem(product: any){
    this.cartItemList.map((a:any, index:any)=>{
      if(product.id=== a.id){
        this.cartItemList.splice(index,1);
      }
    })
    this.productList.next(this.cartItemList);
  }
  removeAllCart(){
    this.cartItemList = []
    this.productList.next(this.cartItemList);
  }


}
