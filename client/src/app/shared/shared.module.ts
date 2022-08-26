import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card'
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonToggleModule } from '@angular/material/button-toggle'; 
import { MatPaginatorModule } from '@angular/material/paginator';
import { PaginationComponent } from './components/pagination/pagination.component';
import { FilterSelectComponent } from './components/filter-select/filter-select.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { TextInputComponent } from './components/text-input/text-input.component';
import { MatRadioModule } from '@angular/material/radio';
import { BasketSummaryComponent } from './components/basket-summary/basket-summary.component'; 
import { CdkStepperModule } from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';

@NgModule({
  declarations: [
    PaginationComponent,
    FilterSelectComponent,
    OrderTotalsComponent,
    TextInputComponent,
    BasketSummaryComponent,
    StepperComponent,
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatRadioModule,
    CdkStepperModule
  ],
  exports: [
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    PaginationComponent,
    FilterSelectComponent,
    OrderTotalsComponent,
    ReactiveFormsModule,
    MatMenuModule,
    TextInputComponent,
    MatRadioModule,
    BasketSummaryComponent,
    CdkStepperModule,
    StepperComponent
  ]
})
export class SharedModule { }
