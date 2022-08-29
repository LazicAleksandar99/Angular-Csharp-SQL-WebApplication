import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AcceptOrder, PendingOrder } from 'src/app/shared/models/order';
import { AuthService } from 'src/app/shared/services/auth.service';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-selected-order',
  templateUrl: './selected-order.component.html',
  styleUrls: ['./selected-order.component.css']
})
export class SelectedOrderComponent implements OnInit {
  order: any;
  orderId : any;
  token : any;
  userId : any ;
  deliverer: AcceptOrder = {
    id: -1
  };

  constructor(private orderService: OrderService,
              private toastr: ToastrService,
              private authService: AuthService,
              private route: Router,) { }

  ngOnInit() {
    this.orderId = localStorage.getItem('selectedOrder')
    this.orderService.getSelectedOrder(this.orderId).subscribe(
      data => {
        this.order = data;
      },
      error => {
        this.toastr.error(error.error.errorMessage, 'Error!', {
          timeOut: 3000,
          closeButton: true,
        });
      }
    );
  }

  Accept(id: number): void{
    this.token = localStorage.getItem('token');
    if(this.authService.getUserVerificationStatus(this.token) == 'Verified'){
      this.userId = this.authService.getUserId(this.token);
      this.deliverer.id = this.userId;
      this.orderService.acceptOrder(id,this.deliverer).subscribe(
        data=>{
          this.toastr.success('Your order request has been successfully accepted, you can now go to work', 'Succes!', {
            timeOut: 3000,
            closeButton: true,
          });
          this.route.navigateByUrl('/home/dashboard')
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
