import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AddProduct } from 'src/app/shared/models/product';
import { ProductService } from 'src/app/shared/services/product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  addProductForm: FormGroup;

  constructor(private fb: FormBuilder,
              private productService: ProductService) {
    this.createLoginForm();}

  ngOnInit() {
  }

  createLoginForm(){
    this.addProductForm = this.fb.group({
      name: [null,[Validators.required,Validators.minLength(2)]],
      price: [1,[Validators.required]],
      ingredients:[null,[Validators.required,Validators.minLength(2)]]
    })
  }

  addProduct(): void {
    if (this.addProductForm.valid){
      this.productService.addProduct(this.addProductForm.value).subscribe(
        data=>{

      }, error =>{
        console.log('Error while adding product');
      }
      )
    }
  }

  get name() {
    return this.addProductForm.get('name') as FormControl;
  }
  get price() {
    return this.addProductForm.get('price') as FormControl;
  }
  get ingredients() {
    return this.addProductForm.get('ingredients') as FormControl;
  }

}
