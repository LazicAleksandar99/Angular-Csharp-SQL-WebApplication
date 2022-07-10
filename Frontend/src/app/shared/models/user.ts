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
  brithday: Date;
  address: string;
}
