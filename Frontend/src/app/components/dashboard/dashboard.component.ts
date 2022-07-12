import { Component, OnInit } from '@angular/core';
import { StorageService } from 'src/app/shared/services/storage.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(private storageService: StorageService) { }

  ngOnInit() {
  }

  hasRole(role: string): boolean {
    return this.storageService.user.role.includes(role) || false;
  }

}
