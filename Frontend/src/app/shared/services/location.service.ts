import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

constructor(private http: HttpClient) { }

  getLocation(streetAddress: string): Observable<any>{  //" +" Strazilovska+31,Novi+Sad" + "
    return this.http.get<any>("https://maps.googleapis.com/maps/api/geocode/json?address=Strazilovska+31,Novi+Sad,Serbia&key=AIzaSyCFDLN87ufEb4O8fDBm4JuygjVFk6pDJCk");
  }
  //Stražilovska 31, Novi Sad, Serbia
  convertStreetToValidGoogleFormat(streetAddress: string): string{
    let streetComponents: string[] =  streetAddress.split(','); //data[0]="Stražilovska 31" , data[1]= "Novi Sad", data[2] = "Serbia"

   // foreach()

    return "";
  }
}
