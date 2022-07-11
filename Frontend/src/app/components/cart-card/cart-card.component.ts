import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-card',
  templateUrl: './cart-card.component.html',
  styleUrls: ['./cart-card.component.css']
})
export class CartCardComponent implements OnInit {

  constructor(private route: Router) { }

  ngOnInit() {
  }

  CheckCart() : void{
    this.route.navigateByUrl('/home/cart')
  }
}
