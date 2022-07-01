import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})

export class UserService {

  constructor( private http: HttpClient) { }

  //login(login:Login) :Observable<Token> {
  //  return this.http.post<Token>(environment.serverURL + '/api/users/login', login);
  //}
  //var requestOptions = {
 //method: 'GET',
///};
  // var= fetch("https://api.geoapify.com/v1/geocode/autocomplete?text=Mosco&apiKey=4000191e0ce34c7eab077e13aded54e1", requestOptions)
  //  .then(response => response.json())
   // .then(result => console.log(result))
    //.catch(error => console.log('error', error));
}

