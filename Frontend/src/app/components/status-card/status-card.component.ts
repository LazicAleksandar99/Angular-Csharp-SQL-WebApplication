import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-status-card',
  templateUrl: './status-card.component.html',
  styleUrls: ['./status-card.component.css']
})
export class StatusCardComponent implements OnInit {
  verifyStatus: any;

  constructor() { }

  ngOnInit() {
    this.verifyStatus = localStorage.getItem('verification');
  }

}
