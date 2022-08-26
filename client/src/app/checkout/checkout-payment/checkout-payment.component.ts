import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrderResponse } from 'src/app/shared/models/orderResponse';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.sass']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private router: Router) { }

  ngOnInit(): void {
  }

  submitOrder()
  {
    const basket = this.basketService.getBasketValue();
    const createOrder = this.createOrder(basket);
    this.checkoutService.createOrder(createOrder).subscribe({
      next: (order: IOrderResponse) => {
        this.basketService.deleteLocalBasket(basket.id);
        const navigationExtras: NavigationExtras = {state: order};
        this.router.navigate(['checkout/success'],navigationExtras);
      },
      error: e => console.log(e)
    });
  }

  private createOrder(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      address: this.checkoutForm.get('addressForm').value
    }
  }
}
