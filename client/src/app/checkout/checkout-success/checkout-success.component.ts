import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IOrderCreate } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.sass']
})
export class CheckoutSuccessComponent implements OnInit {
  order: IOrderCreate;
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state;
    if(state){
      this.order = state as IOrderCreate;
    }else{
      
    }
   }

  ngOnInit(): void {
  }

}
