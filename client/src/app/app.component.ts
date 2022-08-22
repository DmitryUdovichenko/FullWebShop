import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit{

  title = 'WebShop';
  constructor(private basketService: BasketService, private accountService: AccountService){}

  ngOnInit(): void {
    this.loadUser();
    this.loadBasket();    
  }

  loadBasket(){
    const basketId = localStorage.getItem('basket_id');
    if(basketId){
      this.basketService.getBasket(basketId).subscribe({
        next: () => console.log('init basket'),
        error: error =>console.log(error)
      });
    }
  }

  loadUser(){
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe({
      next: () => console.log('init user'),
      error: error =>console.log(error)
    });
  }
}
