import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { UserDetails } from 'src/app/shared/models/user';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-show-user-profile',
  templateUrl: './show-user-profile.component.html',
  styleUrls: ['./show-user-profile.component.css']
})
export class ShowUserProfileComponent implements OnInit {
  user: UserDetails;
  id: any;

  constructor(private route: Router, private profileService: UserService) { }

  ngOnInit() {
    this.getUserDetails();
  }

  getUserDetails(){
    this.id = localStorage.getItem('id');
    console.log('wow');
    console.log(this.id)
    this.profileService.getUserDetails(this.id).subscribe(
      data=>{
        this.user = data;
        console.log(data);
      }, error =>{
        console.log('USER DETAILS')
      }

    );
  }

}
