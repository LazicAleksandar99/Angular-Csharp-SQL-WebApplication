import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Order, OrderProducts } from 'src/app/shared/models/order';
import { CartProduct } from 'src/app/shared/models/product';
import { CartService } from 'src/app/shared/services/cart.service';
import { OrderService } from 'src/app/shared/services/order.service';

//window.paypal
declare var paypal: any;

@Component({
  selector: 'app-paypal-button',
  templateUrl: './paypal-button.component.html',
  styleUrls: ['./paypal-button.component.css']
})

export class PaypalButtonComponent implements OnInit {
  @Input() order: OrderProducts[];
  @Input() comment: string;

  @ViewChild('paypal', {static: true})
  paypalElement!:ElementRef

  constructor(private cartService: CartService,
              private toastr: ToastrService,
              private orderService: OrderService,) { }

  //nema on click na paypal kad pritisne a nemam comment
  ngOnInit() {
    const self = this;
      paypal
        .Buttons({
          style: {
            layout: 'horizontal'
          },

          createOrder: (data: any, actions: any) => {
            return actions.order.create({
              purchase_units: [
                {
                  amount: {
                    currency_code: 'USD',
                    value: self.cartService.getTotalPrice(),
                  },
                },
              ],
            });
          },

          onApprove: async (data: any, actions: any) => {
            const payment = await actions.order.capture();
            this.orderService.makeOrder(this.order,this.comment,"PayPalPaid").subscribe(
              data=>{
                this.toastr.success('You paid succesfuly', 'Succes!', {
                  timeOut: 3000,
                  closeButton: true,
                });
                this.cartService.removeAllCart();
            }, error =>{
              console.log('Error occurred at paypal-button.component.ts');
              this.toastr.error(error.error.errorMessage, 'Error');
            }
            );
          },

          onError: (err: any) => {
            this.toastr.error('Payment Failed', 'Error');
            console.log(err);
          },
        })
        .render(this.paypalElement.nativeElement);
  }
}
