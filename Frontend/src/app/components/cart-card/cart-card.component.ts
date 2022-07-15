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
  }

  CheckCart() : void{
    this.router.navigateByUrl('/home/cart')
  }
}
