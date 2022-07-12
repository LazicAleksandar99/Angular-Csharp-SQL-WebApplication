import { Component, OnInit } from '@angular/core';
import { PendingOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: PendingOrder[];

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.getPendingOrders();
  }

  getPendingOrders(): void{
    this.orderService.getPendingOrder().subscribe(
      data=>{
        this.orders = data;
        console.log(this.orders);

      }, error =>{
        console.log('ERROR WITH PRODUCTS')
      }

    );
  }
}
