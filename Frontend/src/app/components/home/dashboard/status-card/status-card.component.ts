import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-status-card',
  templateUrl: './status-card.component.html',
  styleUrls: ['./status-card.component.css']
})
export class StatusCardComponent implements OnInit {
  verifyStatus: any;
  token: any;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.token = localStorage.getItem('token');
    this.verifyStatus = this.authService.getUserVerificationStatus(this.token);
  }

}
