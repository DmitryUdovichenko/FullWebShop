import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.sass']
})
export class NavBarComponent implements OnInit {
  basket$: Observable<IBasket>;
  isHideBadge = true;
  basketCount = 0;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
      this.basket$.subscribe({
        next: basket => {
          if(basket){
            this.basketCount = basket.items.length;
            this.isHideBadge = false;
          }else{
            this.basketCount = 0;
            this.isHideBadge = true;
          }
          
        },
        error: error => console.log(error)
      });
    
  }

}
