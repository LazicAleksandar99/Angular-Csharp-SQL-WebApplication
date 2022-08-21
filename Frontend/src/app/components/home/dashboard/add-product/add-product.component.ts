import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AddProduct } from 'src/app/shared/models/product';
import { ProductService } from 'src/app/shared/services/product.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  addProductForm: FormGroup;

  constructor(private fb: FormBuilder,
              private productService: ProductService,
              private toastr: ToastrService) {
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
          this.toastr.success('You successfully added new product!', 'Succes!', {
            timeOut: 3000,
            closeButton: true,
          });
        }, error =>{
          this.toastr.error(error.error.errorMessage, 'Error!', {
            timeOut: 3000,
            closeButton: true,
          });
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
