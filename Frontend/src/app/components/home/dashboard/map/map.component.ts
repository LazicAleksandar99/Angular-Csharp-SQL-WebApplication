import { Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'src/app/shared/services/location.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  private map: L.Map;
  private centroid: L.LatLngExpression = [45.267136, 19.833549]; //Trebam naci srbiju

  constructor(private locationService: LocationService,
              private toastr: ToastrService,) {


  }

  ngOnInit() {
    this.initMap();
    //locationService.
    //console.log(locationService.)
    //https://maps.googleapis.com/maps/api/geocode/json?address=Strazilovska+31,Novi+Sad&key=AIzaSyCFDLN87ufEb4O8fDBm4JuygjVFk6pDJCk
    //if(navigator.geolocation){
     // navigator.geolocation.getCurrentPosition(function(position){
    //    console.log(position);
    //    .get("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + position.coords.latitude + ',' + position.coords.longitude + "&sensor=false", function(data){
    //      console.log(data);
    //    })
    //  });
   // }
    this.locationService.getLocation("sda").subscribe(
      data=>{
         console.log(data);
      }, error =>{
        this.toastr.error(error.error.errorMessage, 'Error!', {
          timeOut: 3000,
          closeButton: true,
        });
      }
    );
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
