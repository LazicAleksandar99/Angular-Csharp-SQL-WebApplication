import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CartProduct, Product } from 'src/app/shared/models/product';
import { CartService } from 'src/app/shared/services/cart.service';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: CartProduct[];
  public products : any = [];
  public grandTotal !: number;

  constructor(private cartService: CartService,
              private orderService: OrderService) { }

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

  MakeOrder(commentForm: NgForm): void{

    console.log(commentForm.value.comment);
    if(commentForm.valid){
      this.orderService.makeOrder(this.products,commentForm.value.comment).subscribe(
        data=>{
        console.log(data);

      }, error =>{
        console.log('Error while making order');
      }
      )
    }

  }

}
