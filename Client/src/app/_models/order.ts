import { User } from './user';

export class Order {
    foodId: number;
    locationId: number;
    menuId: number;
    timeId: number;
    orderId: number;
    sideDishes: number[];
    user: User;


  public constructor(init?: Partial<Order>) {
    Object.assign(this, init);
  }
}
