import { Component, OnInit } from '@angular/core';
import { DelivererDetails, VerifyUser } from 'src/app/shared/models/user';
import { UserService } from 'src/app/shared/services/user.service';
import { ToastrService } from 'ngx-toastr';

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
  constructor(private userService: UserService,
              private toastr: ToastrService) { }

  ngOnInit() {
    this.pending = "Pending";
    this.getAllDeliverers()
  }
  getAllDeliverers(): void{
    this.userService.getAllDeliverers().subscribe(
      data=>{
        this.deliverers = data;
      }, error =>{
        console.log('Error occurred at deliverers-list.component.ts')
      }
    );
  }

  Accept(username: string): void{
    this.verifyOrDeny.username = username;
    this.userService.verifyDeliverer(this.verifyOrDeny).subscribe(
      data=>{
        this.toastr.success('You hired another worker!', 'Succes!', {
          timeOut: 3000,
          closeButton: true,
        });
      }, error =>{
        this.toastr.error(error.error.errorMessage, 'Error!', {
          timeOut: 3000,
          closeButton: true,
        });
      }

    );
  }

  Deny(username: string): void{
    this.verifyOrDeny.username = username;
    this.userService.denyDeliverer(this.verifyOrDeny).subscribe(
      data=>{
        this.toastr.success('You have declined a job application!', 'Succes!', {
        timeOut: 3000,
        closeButton: true,
      });
      }, error =>{
        this.toastr.error(error.error.errorMessage, 'Succes!', {
          timeOut: 3000,
          closeButton: true,
        });
      }

    );
  }
}
