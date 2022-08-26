import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdersService } from '../orders.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { IOrderResponse } from 'src/app/shared/models/orderResponse';

@Component({
  selector: 'app-orders-item',
  templateUrl: './orders-item.component.html',
  styleUrls: ['./orders-item.component.sass']
})
export class OrdersItemComponent implements OnInit {
  order: IOrderResponse;
  constructor(private orderService: OrdersService,    private activateRoute: ActivatedRoute, 
    private breadcrumbService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder(){
    this.orderService.getUserOrder(+this.activateRoute.snapshot.paramMap.get('id')).subscribe({
      next: (o: IOrderResponse)=> {
        this.order = o;
        this.breadcrumbService.set('@orderDetails', "Order #"+this.order.id);
      },
      error: (e) => console.error(e)
    });    
  }

}
