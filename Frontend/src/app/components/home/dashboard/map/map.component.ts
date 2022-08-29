import { Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { ToastrService } from 'ngx-toastr';
import { PendingOrder } from 'src/app/shared/models/order';
import { LocationService } from 'src/app/shared/services/location.service';
import { OrderService } from 'src/app/shared/services/order.service';

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
              private orderService: OrderService,) {


  }

  ngOnInit() {
    this.initMap();
  }

  private initMap(): void {
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
               //ovdje treba jos while neki da non stop provjerava i malo bolje i ovo 0.00003 mozda jos provjeriti
               if(this.allPendingOrdersOnMap.includes(cordinates)){
                let newLat: number = data.results[0].geometry.location.lat + 0.00003;
                let newLng: number = data.results[0].geometry.location.lng + 0.00003;
                L.marker([newLat, newLng]).addTo(this.map)
                let newcCordinates: string = data.results[0].geometry.location.lat + "-" + data.results[0].geometry.location.lng
                this.allPendingOrdersOnMap.push(newcCordinates);
               }
               else{
                L.marker([data.results[0].geometry.location.lat, data.results[0].geometry.location.lng]).addTo(this.map)
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

/*    this.locationService.getLocation("Stražilovska 31,Novi Sad,Serbia").subscribe(
      data=>{
         console.log(data);
        // console.log(data.results[0].geometry.location.lat);
         L.marker([data.results[0].geometry.location.lat, data.results[0].geometry.location.lng]).addTo(this.map)
      }, error =>{
        this.toastr.error(error.error.errorMessage, 'Error!', {
          timeOut: 3000,
          closeButton: true,
        });
      }
    );

 */
   // L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
   // attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'

  //}).addTo(this.map);

   // L.marker([51.5, -0.09]).addTo(this.map)
   // .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
   // .openPopup();

   // const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
   //   maxZoom: 14,
   //   minZoom: 1,
   //   attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
   // });

    // create 5 random jitteries and add them to map
  //  const jittery = Array(5).fill(this.centroid).map(
  //      x => [x[0] + (Math.random() - .5)/10, x[1] + (Math.random() - .5)/10 ]
    //  ).map(
   //     x => L.marker(x as L.LatLngExpression)
    //  ).forEach(
   //     x => x.addTo(this.map)
   //   );

  // tiles.addTo(this.map);
  }

}
