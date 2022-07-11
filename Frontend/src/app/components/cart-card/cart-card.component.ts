import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-card',
  templateUrl: './cart-card.component.html',
  styleUrls: ['./cart-card.component.css']
})
export class CartCardComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  console.log('check-cart-load');
  console.log(sessionStorage.getItem("id"));

  }

  CheckCart() : void{
    console.log('check-cart');
    console.log(sessionStorage.getItem('id'));
    this.router.navigateByUrl('/home/cart')
  }
}
