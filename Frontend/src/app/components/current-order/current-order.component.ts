import { Component, OnInit } from '@angular/core';
import { CurrentOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-current-order',
  templateUrl: './current-order.component.html',
  styleUrls: ['./current-order.component.css']
})
export class CurrentOrderComponent implements OnInit {
  orders: CurrentOrder[];
  endTime: Date;
  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.getCurrentOrder();
  }

  getCurrentOrder(): void{
    this.orderService.getCurrentOrders().subscribe(
      data=>{
        this.orders = data;

        for(let key in this.orders) {
          let child = this.orders[key];
          var time = new Date();
          this.endTime = new Date(child.deliveryTime);
          child.stopwatch = Math.ceil(Math.abs(time.getTime()- this.endTime.getTime()) / 36e5*60);
          }
      }, error =>{
        console.log('Error with orders')
      }
    );
  }
}
