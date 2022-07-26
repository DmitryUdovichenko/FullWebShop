import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { lastValueFrom } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrderResponse } from 'src/app/shared/models/orderResponse';
import { CheckoutService } from '../checkout.service';
declare var Stripe;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.sass']
})
export class CheckoutPaymentComponent implements AfterViewInit, OnDestroy {
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', {static: true}) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', {static: true}) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', {static: true}) cardCvcElement: ElementRef;
  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);
  loading = false;
  cardNumberValid = false;
  cardExpiryValid = false;
  cardCvcValid = false;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private router: Router, private toastr: ToastrService) { }

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  ngAfterViewInit(): void {
    this.stripe = Stripe('pk_test_51LazCSLi3nOhrkcMsKzhfCo5QueDzejAI52f88XCsdFAtxnMkmnnkDD2fFNWOtrUKcuVTUjwE8YpcKin8H8YeyI600DonZwc0m');
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler)

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler)
  }

  async submitOrder()
  {
    this.loading = true;
    const basket = this.basketService.getBasketValue();

    try {
      const createdOrder = await this.getNewOrder(basket);
      const paymentResult = await this.confirmPayment(basket.clientSecret);
      if(paymentResult.paymentIntent){
        this.basketService.deleteBasket(basket);
        const navigationExtras: NavigationExtras = {state: createdOrder};
        this.router.navigate(['checkout/success'],navigationExtras);
      }else{
        this.toastr.error(paymentResult.error.message);
      }
      this.loading = false;
    } catch (error) {
      console.log(error);
      this.loading = false;
    }

  }

  private async confirmPayment(clientSecret) {
    return this.stripe.confirmCardPayment(clientSecret, {
      payment_method: {
        card: this.cardNumber,
        billing_details: {
          name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
        }
      }
    });
  }

  private async getNewOrder(basket: IBasket) {
    const createOrder = this.createOrder(basket);
    return lastValueFrom(this.checkoutService.createOrder(createOrder));
  }

  private createOrder(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      address: this.checkoutForm.get('addressForm').value
    }
  }

  onChange(event){
    if(event.error){
      this.cardErrors = event.error.message;
    }
    else{
      this.cardErrors = null;
    }

    this[event.elementType+'Valid'] = event.complete;
  }
}
