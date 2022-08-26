import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscriber } from 'rxjs';
import { AccountService } from '../account/account.service';
import { IAddress } from '../shared/models/address';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.sass'],
  encapsulation: ViewEncapsulation.None
})
export class CheckoutComponent implements OnInit {

  checkoutForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressValues();
  }

  createCheckoutForm(){
    this.checkoutForm = this.formBuilder.group({
      addressForm: this.formBuilder.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        city: [null, Validators.required],
        postCode: [null, Validators.required],
        street: [null, Validators.required],
      }),
      deliveryForm: this.formBuilder.group({
        deliveryMethod: [null, Validators.required]
      }),
      paymentForm: this.formBuilder.group({
        nameOnCard: [null, Validators.required]
      }),
    });
  }

  getAddressValues(){
    this.accountService.getUserAddress().subscribe({
      next: (address: IAddress)=> {
        this.checkoutForm.get('addressForm').patchValue(address);
      },
      error: (e) => console.error(e)
    });
  }
}
