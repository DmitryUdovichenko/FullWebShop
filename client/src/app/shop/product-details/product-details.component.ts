import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.sass']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  quantity = 1;
  constructor(private shopService: ShopService, 
    private activateRoute: ActivatedRoute, 
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe({
      next: (p)=> {
        this.product = p;
        this.breadcrumbService.set('@productDetails', this.product.name);
      },
      error: (e) => console.error(e)
    });    
  }
  
  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
    if(this.quantity > 1){
      this.quantity--
    }    
  }
  
  addToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity);
  }
}
