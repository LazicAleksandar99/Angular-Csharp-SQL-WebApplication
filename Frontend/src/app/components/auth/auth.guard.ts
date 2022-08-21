import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { StorageService } from 'src/app/shared/services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  token: any;
  id: any;

  constructor(private router: Router,
              private cookieService:CookieService,
              private storageService: StorageService) {
  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    var jwt = this.cookieService.get('jwt')
    if (localStorage.getItem('token') != null){
      this.token = localStorage.getItem('token');
      this.id = localStorage.getItem('id');
      this.storageService.setStorage(this.token,this.id);//

      if(this.storageService.user.role.includes(next.data['role1']) ||
         this.storageService.user.role.includes(next.data['role2']) ||
         this.storageService.user.role.includes(next.data['role3'])){
          const isAuthorized = true;
          return isAuthorized;
      }
      else{
        this.router.navigate(['/user/login']);
      }
      return false;
    }
    else {
      this.router.navigate(['/user/login']);
      return false;
    }

  }
}
