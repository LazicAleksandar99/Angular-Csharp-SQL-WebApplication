import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

constructor(private http: HttpClient) { }

  getLocation(streetAddress: string): Observable<any>{
    let convertedAddres: string = this.convertStreetToValidGoogleFormat(streetAddress);
    return this.http.get<any>("https://maps.googleapis.com/maps/api/geocode/json?address="+ convertedAddres + "&key=AIzaSyCFDLN87ufEb4O8fDBm4JuygjVFk6pDJCk");
  }

  convertStreetToValidGoogleFormat(streetAddress: string): string{
    let streetComponents: string[] =  streetAddress.split(',');
    let words: string[];
    let returnString = "";
    streetComponents.forEach( part => {
      words = part.split(' '),
      words.forEach( word =>{
        returnString += word;
        returnString += "+";
      })
      returnString = returnString.slice(0,-1);
      returnString += ",";

      })
      returnString = returnString.slice(0,-1);
    return returnString;
  }
}
