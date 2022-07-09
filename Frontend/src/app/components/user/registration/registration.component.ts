import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Role } from 'src/app/shared/enums/the-role';
import { UserForRegister } from 'src/app/shared/models/user';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {
  alertify: any;
  title = 'google-places-autocomplete';
  registerationForm: FormGroup ;
  //currentdate = Date.now('yyyy-mm-dd');
  user: UserForRegister;
  userRole: Role;
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


  constructor(private router: Router,
              private fb: FormBuilder,
              private authService: AuthService,
              private alertifyService: AlertifyService) {

      this.createRegisterationForm();
  }

  ngOnInit() {


  }

  createRegisterationForm() {
    this.registerationForm = this.fb.group({
      username: [null,Validators.required],
      email: [null,[Validators.required,Validators.email]],
      firstname: [null,[Validators.required,Validators.minLength(3)]],
      lastname: [null,[Validators.required,Validators.minLength(3)]],
      birthday: [null,[Validators.required]],
      address: [null,[Validators.required]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      confirmPassword: [null, Validators.required],
    },{validators: this.passwordMatchingValidatior});

  }



  passwordMatchingValidatior(fg: FormGroup): Validators {
      return fg.get('password')?.value === fg.get('confirmPassword')?.value ? true : {notmatched: true};
  }


  onSubmitUser() {
    this.userRole = Role.normalUser
    this.OnSubmit();
  }

  onSubmitDeliverer() {
    this.userRole = Role.deliverer
    this.OnSubmit();
  }

  OnSubmit(){
    if (this.registerationForm.valid) {
      // this.user = Object.assign(this.user, this.registerationForm.value);
         this.authService.registerUser(this.userData()).subscribe(() =>
         {
         // this.alertify.success('Congrats, you are successfully registered');
          this.router.navigate(['/user/login']);
       });
    }
  }

  userData(): UserForRegister {
    return this.user = {
        username: this.username.value,
        email: this.email.value,
        firstname: this.firstname.value,
        lastname: this.lastname.value,
        birthday: this.registerationForm.value['birthday'],
        address: this.address.value,
        role: this.userRole,
        password: this.password.value,
        picture: ""
    };
  }

  get username() {
    return this.registerationForm.get('username') as FormControl;
  }
  get email() {
    return this.registerationForm.get('email') as FormControl;
  }
  get firstname() {
    return this.registerationForm.get('firstname') as FormControl;
  }
  get lastname() {
    return this.registerationForm.get('lastname') as FormControl;
  }
  get address() {
    return this.registerationForm.get('address') as FormControl;
  }
  get password() {
      return this.registerationForm.get('password') as FormControl;
  }
  get confirmPassword() {
      return this.registerationForm.get('confirmPassword') as FormControl;
  }

}
