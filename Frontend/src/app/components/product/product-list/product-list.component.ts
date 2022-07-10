import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ProductService } from 'src/app/shared/services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[];
  page: number = 1;
  token: any;
  constructor(private productService: ProductService) { }

  ngOnInit() {
    this.token = localStorage.getItem('token');
    this.productService.getAllProducts().subscribe(
      data=>{
        this.products = data;
       
        console.log(data);
      }, error =>{
        console.log('ERROR WITH PRODUCTS')
      }

    );

  }

}
