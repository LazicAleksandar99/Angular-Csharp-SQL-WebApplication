import { Component, OnInit } from '@angular/core';
import { AcceptOrder, PendingOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: PendingOrder[];
  deliverer: AcceptOrder = {
    id: -1
  };
  userId: any;

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.getPendingOrders();
  }

  getPendingOrders(): void{
    this.orderService.getPendingOrder().subscribe(
      data=>{
        this.orders = data;

      }, error =>{
        console.log('Error with orders')
      }

    );
  }

  Accept(id: number): void{
    if(localStorage.getItem('verification') == 'Verified'){
      this.userId = localStorage.getItem('id');
      this.deliverer.id = this.userId;
      this.orderService.acceptOrder(id,this.deliverer).subscribe(
        data=>{
        }, error =>{
          console.log('Error while accepting order')
        }

      );
    }
    else{
      alert('You are not verified you cant accept order')
    }
  }
}
