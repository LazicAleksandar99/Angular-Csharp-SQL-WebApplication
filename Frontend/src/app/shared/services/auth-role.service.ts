import { Injectable } from '@angular/core';
import { UserTokenModel } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthRoleService {

  constructor() { }

  private getUser(token: string): UserTokenModel{
    return JSON.parse(token.split('.')[1]) as UserTokenModel;;
  }
}
