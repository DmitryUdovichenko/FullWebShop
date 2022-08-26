import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/delivery';
import { IOrderCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiHost;
  constructor(private http: HttpClient) { }

  getDeliveryMethods(){
    return this.http.get(this.baseUrl + 'order/deliverymethods').pipe(
      map((deliveryMethods: IDeliveryMethod[]) => {
        return deliveryMethods.sort((a,b) => b.price-a.price);
      })
    );
  }

  createOrder(order: IOrderCreate){
    return this.http.post(this.baseUrl+'order',order);
  }
}
