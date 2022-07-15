import { Component, OnInit } from '@angular/core';
import { DelivererDetails, VerifyUser } from 'src/app/shared/models/user';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-deliverers-list',
  templateUrl: './deliverers-list.component.html',
  styleUrls: ['./deliverers-list.component.css']
})
export class DeliverersListComponent implements OnInit {
  deliverers: DelivererDetails[];
  pending: string;
  verifyOrDeny: VerifyUser = {
    username: ""
  };
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.pending = "Pending";
    this.getAllDeliverers()
  }
  getAllDeliverers(): void{
    this.userService.getAllDeliverers().subscribe(
      data=>{
        this.deliverers = data;
      }, error =>{
        console.log('ERROR WITH DELIVERERS')
      }

    );
  }

  Accept(username: string): void{
    this.verifyOrDeny.username = username;
    this.userService.verifyDeliverer(this.verifyOrDeny).subscribe(
      data=>{
      }, error =>{
        console.log('Error ocured while verifing user');
        console.log(username);
      }

    );
  }

  Deny(username: string): void{
    this.verifyOrDeny.username = username;
    this.userService.denyDeliverer(this.verifyOrDeny).subscribe(
      data=>{
      }, error =>{
        console.log('Error ocured while denning user');
        console.log(username);
      }

    );
  }
}
