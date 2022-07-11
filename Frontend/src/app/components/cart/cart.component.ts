import { Component, OnInit } from '@angular/core';
import { CartProduct, Product } from 'src/app/shared/models/product';
import { CartService } from 'src/app/shared/services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: CartProduct[];
  public products : any = [];
  public grandTotal !: number


  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.getProdcuts();
  }

  getProdcuts(): void{
    this.cartService.getProducts()
    .subscribe(res=>{
      this.products = res;
      this.grandTotal = this.cartService.getTotalPrice();
    })
  }

  RemoveItem(item: any): void{
    this.cartService.removeCartItem(item);
  }

  EmptyCart(): void {
    this.cartService.removeAllCart();
  }

}
