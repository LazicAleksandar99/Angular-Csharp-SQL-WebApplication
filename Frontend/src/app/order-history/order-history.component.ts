import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StatusOrder } from '../shared/models/order';
import { OrderService } from '../shared/services/order.service';

@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.css']
})
export class OrderHistoryComponent implements OnInit {
  orders: StatusOrder[];
  id: any;
  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit() {
    this.getOrderHistory();
  }

  getOrderHistory(): void{
    this.id = localStorage.getItem("id");
    this.orderService.getOrderHistory(this.id).subscribe(
      data => {
         this.orders = data;
      }, error =>{
        console.log('Error with order history')
      }
    );
  }

  TakeOrder(): void{
    this.router.navigateByUrl('/home/dashboard')
  }

}
