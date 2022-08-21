import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';
import { StatusOrder } from '../../../shared/models/order';
import { OrderService } from '../../../shared/services/order.service';

@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.css']
})
export class OrderHistoryComponent implements OnInit {
  orders: StatusOrder[];
  id: any;
  token: any;
  constructor(private orderService: OrderService,
              private router: Router,
              private authService: AuthService) { }

  ngOnInit() {
    this.getOrderHistory();
  }

  getOrderHistory(): void{
    this.token = localStorage.getItem('token');
    this.id = this.authService.getUserId(this.token);
    this.orderService.getOrderHistory(this.id).subscribe(
      data => {
         this.orders = data;
      }, error =>{
        console.log('Error occurred at order-history.component.ts')
      }
    );
  }

  TakeOrder(): void{
    this.router.navigateByUrl('/home/dashboard')
  }

}
