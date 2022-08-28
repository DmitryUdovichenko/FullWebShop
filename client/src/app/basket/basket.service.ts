import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket } from '../shared/models/basket';
import { IBasketItem } from '../shared/models/basketItem';
import { IBasketTotals } from '../shared/models/basketTotals';
import { IDeliveryMethod } from '../shared/models/delivery';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiHost;
  private basketSrc = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSrc.asObservable();
  private basketTotalSrc = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSrc.asObservable();
  shipingPrice = 0;
  constructor(private http: HttpClient) { }

  getBasket(id: string){
    return this.http.get(this.baseUrl+'basket?basketId='+id).pipe(
      map((basket: IBasket) => {
        this.basketSrc.next(basket);
        this.shipingPrice = basket.shippingPrice;
        this.calculateTotals();
      })
    );
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl+"basket", basket).subscribe({
      next: (response: IBasket)=> {
        this.basketSrc.next(response); 
        this.calculateTotals();
      },
      error: (e) => console.error(e)
    })
  }

  getBasketValue(){
    return this.basketSrc.value;
  }

  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd: IBasketItem = this.mapProductToItem(item, quantity);
    const basket = this.getBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItems(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getBasketValue();
    const itemIndex = basket.items.findIndex(i => i.id == item.id);
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getBasketValue();
    const itemIndex = basket.items.findIndex(i => i.id == item.id);
    if(basket.items[itemIndex].quantity > 1){
      basket.items[itemIndex].quantity--;
      this.setBasket(basket);
    }else{
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getBasketValue();
    if(basket.items.some(i => i.id == item.id)){
      basket.items = basket.items.filter(i => i.id != item.id);
      if(basket.items.length > 0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket);
      }
    }
    
  }

  deleteLocalBasket(id: string){
    this.basketSrc.next(null);
    this.basketTotalSrc.next(null);
    localStorage.removeItem('basket_id');
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe({
      next: () =>{
        this.basketSrc.next(null);
        this.basketTotalSrc.next(null);
        localStorage.removeItem('basket_id');
      },
      error: error => console.log(error)
    });
  }
  
  addOrUpdateItems(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const itemIndex = items.findIndex(i => i.id == itemToAdd.id);
    if(itemIndex === -1){
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else{
      items[itemIndex].quantity += quantity;
    }
    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductToItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      imageUrl: item.imageUrl,
      quantity
    };
  }

  private calculateTotals(){
    const basket = this.getBasketValue();
    const shipping = this.shipingPrice;
    const subTotal = basket.items.reduce((index, item) => (item.price*item.quantity) + index, 0);
    const total = subTotal + shipping;
    this.basketTotalSrc.next({shipping, subTotal, total});
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod){
    this.shipingPrice = deliveryMethod.price;
    const basket = this.getBasketValue();
    basket.deliveryMethodId = deliveryMethod.id;
    basket.shippingPrice = deliveryMethod.price;
    this.calculateTotals();
    this.setBasket(basket);
  }

  createPaymentIntent(){
    return this.http.post(this.baseUrl+"payment/"+this.getBasketValue().id, []).pipe(
      map((basket: IBasket)=>{
        this.basketSrc.next(basket);
      })
    );
  }
}
