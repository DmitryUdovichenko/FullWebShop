import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdersItemComponent } from './orders-item/orders-item.component';
import { OrdersComponent } from './orders.component';

const routes: Routes = [
  {path: '', component: OrdersComponent},
  {path: ':id', component: OrdersItemComponent, data: {breadcrumb: {alias: 'orderDetails'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
