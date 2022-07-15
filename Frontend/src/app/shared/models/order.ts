import { Time } from "@angular/common";

export interface Order{
  id: number;
  name: string;
  price: number;
  quantity: number;
  total: number;
}

export interface StatusOrder{
  id: number;
  price: number;
  userId: number;
  address: string;
  email: string;
  comment: string;
  orderStatus: string;
  content: string
}

export interface PendingOrder{
  id: number;
  price: number;
  userId: number;
  address: string;
  email: string;
  comment: string;
  content: string
}

export interface AcceptOrder{
  id:number;
}

export interface CurrentOrder{
  id: number;
  price: number;
  userId: number;
  address: string;
  email: string;
  comment: string;
  orderStatus: string;
  content: string;
  deliveryTime: Date;
  stopwatch: number;
}
