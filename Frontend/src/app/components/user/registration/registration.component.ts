import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Role } from 'src/app/shared/enums/the-role';
import { UserForRegister } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/services/user.service';
import { Options } from 'ngx-google-places-autocomplete/objects/options/options';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {
  alertify: any;
  title = 'google-places-autocomplete';
  registerationForm: FormGroup ;
  selectedFile: File;
  user: UserForRegister;
  userRole: Role;
  id: any;

  options = {
  types: ['address'],
  componentRestrictions: { country: ['rs'] }
  } as unknown as Options;

  handleAddressChange(address: any) {
  }

  constructor(private router: Router,
              private fb: FormBuilder,
              private authService: AuthService,
              private toastr: ToastrService,
              private profileService: UserService) {

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
      birthday: ["2022-05-05",[Validators.required]],
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
         this.authService.registerUser(this.userData()).subscribe(
          data=>{
            this.toastr.success('You have registered correctly, try loging in now', 'Succes!', {
              timeOut: 3000,
              closeButton: true,
            });
            this.id = data;
            if(this.selectedFile){
              let formData = new FormData();
              formData.append("myfile",this.selectedFile);
              this.profileService.updateUserPhoto(this.id,formData).subscribe(
                data=>{
                  this.toastr.success('Your profile has been successfully updated', 'Succes!', {
                    timeOut: 3000,
                    closeButton: true,
                  });
                }, error => {
                  this.toastr.error(error.error.errorMessage, 'Error!', {
                    timeOut: 3000,
                    closeButton: true,
                  });
                }


              );
            }
            this.router.navigate(['/user/login']);
          }, error =>{
            this.toastr.error(error.error.errorMessage, 'Error!' , {
              timeOut: 3000,
              closeButton: true,
            });
          }

        );
    }
    else{
      this.toastr.error("You have to input every field valid", 'Error!' , {
        timeOut: 3000,
        closeButton: true,
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

  onFileChanged(imageInput: any){
    this.selectedFile = imageInput.files[0];
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
