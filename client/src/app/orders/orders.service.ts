import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IOrderResponse } from '../shared/models/orderResponse';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiHost;
  constructor(private http: HttpClient) { }

  getUserOrders(){
    return this.http.get(this.baseUrl+'order').pipe(
      map((orders:IOrderResponse[])=>{
        return orders.sort((a,b) => <any>new Date(b.createdDate)-<any>new Date(a.createdDate));
      })
    );
  }

  getUserOrder(id: number){
    return this.http.get(this.baseUrl + 'order/' + id);
  }
}
