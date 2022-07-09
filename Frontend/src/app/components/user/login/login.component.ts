import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { NgForm, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  user: UserForLogin;
  auth2: any;
  @ViewChild('loginRef', {static: true }) loginElement!: ElementRef;

  constructor(private router: Router,
              private fb: FormBuilder,
              private authService: AuthService,) {

      this.createLoginForm();
  }

  ngOnInit() {
    this.googleAuthSDK();
    console.log(this.auth2);
  }

  createLoginForm(){
    this.loginForm = this.fb.group({
      email: [null,[Validators.required,Validators.email]],
      password: [null,[Validators.required,Validators.minLength(8)]]
    })
  }

  onLogin() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value)
      this.authService.authUser(this.loginForm.value).subscribe(
        (res: any) => {
          localStorage.setItem('token', res.token);
          this.router.navigateByUrl('/home');
        },
        err => {
          if (err.status == 400)
           // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
           alert('sd');
          else
            console.log(err);
        }
    );
    }
  }

  userData(): UserForLogin {
    return this.user = {
        email: this.email.value,
        password: this.password.value,
        token: ""
    };
  }

  callLoginButton() {

    this.auth2.attachClickHandler(this.loginElement.nativeElement, {},

      (googleAuthUser:any) => {

        let profile = googleAuthUser.getBasicProfile();
        console.log('Token || ' + googleAuthUser.getAuthResponse().id_token);
        console.log('ID: ' + profile.getId());
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail());

       /* Write Your Code Here */
       this.router.navigateByUrl('/home');
      }, (error:any) => {
        alert(JSON.stringify(error, undefined, 2));
      });

  }

  /**
   * Write code on Method
   *
   * @return response()
   */
  googleAuthSDK() {
    (<any>window)['googleSDKLoaded'] = () => {
      (<any>window)['gapi'].load('auth2', () => {
        this.auth2 = (<any>window)['gapi'].auth2.init({
          client_id: '918432122938-cd6oba2qtpsle40ehi05pde64rf8p1ka.apps.googleusercontent.com',
          cookiepolicy: 'single_host_origin',
          plugin_name:'login',
          scope: 'profile email'
        });
        this.callLoginButton();
      });
    }
    (function(d, s, id){
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) {return;}
      js = d.createElement('script');
      js.id = id;
      js.src = "https://apis.google.com/js/platform.js?onload=googleSDKLoaded";
      fjs?.parentNode?.insertBefore(js, fjs);
    }(document, 'script', 'google-jssdk'));
  }

  get email() {
    return this.loginForm.get('email') as FormControl;
  }
  get password() {
    return this.loginForm.get('password') as FormControl;
  }

}
