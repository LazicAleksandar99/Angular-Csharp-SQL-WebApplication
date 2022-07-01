import { Component, OnInit, Inject } from '@angular/core';
import { NgForm, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    username : new FormControl("", Validators.required),
    password : new FormControl("", Validators.required),
  });
  constructor(private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/home');
  }

  onSubmit() {

  }
}
