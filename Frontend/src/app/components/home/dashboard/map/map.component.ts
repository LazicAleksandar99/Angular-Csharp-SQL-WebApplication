import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as L from 'leaflet';
import { ToastrService } from 'ngx-toastr';
import { PendingOrder } from 'src/app/shared/models/order';
import { LocationService } from 'src/app/shared/services/location.service';
import { OrderService } from 'src/app/shared/services/order.service';
import { StorageService } from 'src/app/shared/services/storage.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  private map: L.Map;
  private centroid: L.LatLngExpression = [45.267136, 19.833549];
  orders: PendingOrder[];
  allPendingOrdersOnMap: string[] = [];

  constructor(private locationService: LocationService,
              private toastr: ToastrService,
              private orderService: OrderService,
              private router: Router,
              private storageService: StorageService) {


  }

  ngOnInit() {
    this.initMap();
  }

  private initMap(): void {

    const self = this;
    this.map = L.map('map', {
      center: this.centroid,
      zoom: 12,
      preferCanvas: true,
    });

    L.tileLayer('https://api.maptiler.com/maps/streets/{z}/{x}/{y}.png?key=8xYhULIwwlz6Tct5ewUU',{
      attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">&copy; MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">&copy; OpenStreetMap contributors</a>',
      maxZoom: 18,
      minZoom: 10,
    }).addTo(this.map);

    this.orderService.getPendingOrder().subscribe(
      data=>{
        this.orders = data;
        this.orders.forEach(order => {
          this.locationService.getLocation(order.address).subscribe(
            data=>{
               console.log(data);
               let cordinates: string = data.results[0].geometry.location.lat + "-" + data.results[0].geometry.location.lng
               if(this.allPendingOrdersOnMap.includes(cordinates)){
                let newLat: number = data.results[0].geometry.location.lat + 0.00003;
                let newLng: number = data.results[0].geometry.location.lng + 0.00003;
                L.marker([newLat, newLng]).on('click', function(e){self.markerOnClick(order.id)}).addTo(this.map)
                let newcCordinates: string = data.results[0].geometry.location.lat + "-" + data.results[0].geometry.location.lng
                this.allPendingOrdersOnMap.push(newcCordinates);
               }
               else{
                L.marker([data.results[0].geometry.location.lat, data.results[0].geometry.location.lng]).on('click',function(e){self.markerOnClick(order.id)}).addTo(this.map)
                this.allPendingOrdersOnMap.push(cordinates);
               }

            }, error =>{
              this.toastr.error(error.error.errorMessage, 'Error!', {
                timeOut: 3000,
                closeButton: true,
              });
            }
          );
        });
     }, error =>{
       this.toastr.error(error.error.errorMessage, 'Error!', {
         timeOut: 3000,
         closeButton: true,
       });
     }
   );
  }

  markerOnClick(orderID: number)
  {
    this.storageService.setSelectedPendingOrder(orderID);
    this.router.navigateByUrl('/home/selected-order');
  }

}
