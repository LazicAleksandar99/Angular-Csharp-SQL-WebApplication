import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { UserDetails } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-show-user-profile',
  templateUrl: './show-user-profile.component.html',
  styleUrls: ['./show-user-profile.component.css']
})
export class ShowUserProfileComponent implements OnInit {
  user: UserDetails;
  id: any;
  token: any;

  constructor(private route: Router,
              private profileService: UserService,
              private authService: AuthService) { }

  ngOnInit() {
    this.getUserDetails();
  }

  getUserDetails(){
    this.token = localStorage.getItem('token');
    this.id = this.authService.getUserId(this.token);
    this.profileService.getUserDetails(this.id).subscribe(
      data=>{
        this.user = data;
      }, error =>{
        console.log('Error occurred at show-user-profile.component.ts')
      }

    );
  }

  ChangeProfile(): void {
    this.route.navigateByUrl('/home/profile')
  }

}
