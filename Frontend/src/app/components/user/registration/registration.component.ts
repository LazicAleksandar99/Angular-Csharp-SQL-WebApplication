import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  title = 'google-places-autocomplete';
  //userAddress: string = ''
  //userLatitude: string = ''
  //userLongitude: string = ''

  options={
    componentRestrictions:{
    country:["AU"]
    }
}

  handleAddressChange(address: any) {
    console.log(address);
   // this.userAddress = address.formatted_address
   // this.userLatitude = address.geometry.location.lat()
   // this.userLongitude = address.geometry.location.lng()
  }

  registrationForm = new FormGroup({
    UserName: new FormControl('', Validators.required),
    Name: new FormControl('', Validators.required),
    LastName: new FormControl('', Validators.email),
    Password: new FormControl('', [Validators.required, Validators.minLength(4)]),
  });



  constructor(private router: Router) { }

  ngOnInit() {
  }

  onSubmit() {


  }

}
