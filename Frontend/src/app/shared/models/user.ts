import { Role } from "../enums/the-role";

export interface UserForRegister {
    username: string;
    email?: string;
    firstname: string;
    lastname: string;
    birthday: Date;
    address: string;
    password: string;
    role: Role;
    picture: string;
}

export interface UserForLogin {
    email: string;
    password: string;
    token: string;
}

export interface UserDetails{
  username: string;
  email: string;
  firstname: string;
  lastname: string;
  birthday: Date;
  address: string;
  picture: string;
  verification: string;
}

export interface UserUpdate{
  username: string;
  email: string;
  firstname: string;
  lastname: string;
  birthday: Date;
  address: string;
  oldpassword: string;
  newpassword: string;
}

export interface UserTokenModel{
  unique_name: string,
  nameid: string,
  role: string,
  authentication: string,
}

export interface DelivererDetails{
  username: string;
  email: string;
  firstname: string;
  lastname: string;
  birthday: Date;
  address: string;
  verification: string;
}

export interface VerifyUser{
  username:string;
}


