export interface Order{
  id: number;
  name: string;
  price: number;
  quantity: number;
  total: number;
}

export interface PendingOrder{
  id: number;
  price: number;
  userId: number;
  address: string;
  email: string;
  comment: string;
}
