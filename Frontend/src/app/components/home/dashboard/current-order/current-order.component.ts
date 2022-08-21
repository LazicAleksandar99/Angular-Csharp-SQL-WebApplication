import { Component, OnInit, ViewChild } from '@angular/core';
import { CurrentOrder } from 'src/app/shared/models/order';
import { OrderService } from 'src/app/shared/services/order.service';
import { CountdownComponent,CountdownEvent,CountdownConfig} from 'ngx-countdown';

const KEY = 'time';
const DEFAULT = 100;

@Component({
  selector: 'app-current-order',
  templateUrl: './current-order.component.html',
  styleUrls: ['./current-order.component.css']
})
export class CurrentOrderComponent implements OnInit {
  orders: CurrentOrder[];
  endTime: Date;
  minutes: number;
  seconds: number;
  config: CountdownConfig = { leftTime: DEFAULT, notify: 0, format: 'm:s' };

  @ViewChild('cd', { static: false }) private countdown: CountdownComponent;

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
          this.minutes = (Math.abs(this.endTime.getTime() - time.getTime()) / (1000 * 60) % 60);
          this.seconds = (Math.abs(this.endTime.getTime() - time.getTime()) / (1000) % 60);
          let allSeconds = (this.minutes * 60);
          this.config = { ...this.config, leftTime: Math.ceil(allSeconds) };
          }
      }, error =>{
        console.log('Error occurred at current-order.component.ts')
      }
    );
  }

  handleEvent(ev: CountdownEvent) {
    if (ev.action === 'notify') {
      // Save current value
      localStorage.setItem(KEY, `${ev.left / 1000}`);
    }
  }
}
