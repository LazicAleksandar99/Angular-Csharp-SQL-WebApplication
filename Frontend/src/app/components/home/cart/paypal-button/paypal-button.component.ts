import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Order } from 'src/app/shared/models/order';
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
  @Input() order: Order[];
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
            console.log('TOTALITI');
            console.log(this.cartService.getTotalPrice());
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
            this.orderService.payOrderByPayPal(this.order,this.comment).subscribe(
              data=>{
                this.toastr.success('You paid succesfuly', 'Succes!', {
                  timeOut: 3000,
                  closeButton: true,
                });
                this.cartService.removeAllCart();
            }, error =>{
              console.log('Error occurred at paypal-button.component.ts');
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
