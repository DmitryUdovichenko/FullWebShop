import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import {MatBadgeModule} from '@angular/material/badge';


@NgModule({
  declarations: [
    NavBarComponent
  ],
  imports: [
    CommonModule,
    MatBadgeModule
  ],
  exports: [
    NavBarComponent
  ]
})
export class CoreModule { }
