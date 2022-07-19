import { Component, OnInit } from '@angular/core';
import { AcceptOrder, PendingOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';
import { ToastrService } from 'ngx-toastr';

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

  constructor(private orderService: OrderService,
              private toastr: ToastrService) { }

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
          this.toastr.success('Your order request has been successfully accepted, you can now go to work', 'Succes!', {
            timeOut: 3000,
            closeButton: true,
          });
        }, error =>{
          this.toastr.error(error.error.errorMessage, 'Error!', {
            timeOut: 3000,
            closeButton: true,
          });
        }

      );
    }
    else{
      this.toastr.error('You are not verified', 'Error!', {
        timeOut: 3000,
        closeButton: true,
      });
    }
  }
}
