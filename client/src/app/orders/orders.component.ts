import { Component, OnInit } from '@angular/core';
import { OrdersService } from './orders.service';
import { IOrderResponse } from '../shared/models/orderResponse';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.sass']
})
export class OrdersComponent implements OnInit {
  orders: IOrderResponse[];

  constructor(private orderService: OrdersService) { }

  ngOnInit(): void {
    this.getUserOrders()
  }

  getUserOrders(){
    this.orderService.getUserOrders().subscribe({
      next: (orders: IOrderResponse[]) => {
        this.orders = orders;
        console.log(this.orders);
      },
      error: e => console.log(e)
    });
  }
}
