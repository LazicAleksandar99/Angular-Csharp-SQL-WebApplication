import { Component, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/shared/models/product';
import { CartService } from 'src/app/shared/services/cart.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent {
  @Input() product: Product;
  quantityForm: FormGroup;
  quantity: any = 1;

  constructor(private cartService : CartService,private fb: FormBuilder) {
    this.quantityForm = this.fb.group({
      quantityField: [1,Validators.required]
    })
  }

  AddToCart(item: any) : void {
     this.quantity = this.quantityForm.get('quantityField')?.value;

    var product: any = {
      id: item.id,
      quantity:this.quantity,
      ingredients: item.ingredients,
      name: item.name,
      price: item.price,
      total: item.price*this.quantity
    }
    this.cartService.addtoCart(product);
  }

}
