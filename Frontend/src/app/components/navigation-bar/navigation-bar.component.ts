import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  SignOut(): void{
    localStorage.clear();
    this.router.navigateByUrl('/user/login');
  }
  CheckDashboard(): void{
    this.router.navigateByUrl('/home/dashboard')
  }


  CheckProfile(): void{
    this.router.navigateByUrl('/home/profile')
  }

  CheckCart(): void{
    this.router.navigateByUrl('/home/cart')
  }

  CheckHistory(): void{
    this.router.navigateByUrl('/home/history')
  }

}
