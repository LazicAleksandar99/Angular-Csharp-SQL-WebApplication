import { Component, OnInit } from '@angular/core';
import { StatusOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-all-orders',
  templateUrl: './all-orders.component.html',
  styleUrls: ['./all-orders.component.css']
})
export class AllOrdersComponent implements OnInit {
  orders: StatusOrder[];

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.getAllOrders();
  }

  getAllOrders(): void{
    this.orderService.getAllOrders().subscribe(
      data=>{
        this.orders = data;
      }, error =>{
        console.log('Error occurred at all-orders.component.ts')
      }

    );
  }

}
