import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import {MatBadgeModule} from '@angular/material/badge';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeadeComponent } from './section-heade/section-heade.component';
import {BreadcrumbModule} from 'xng-breadcrumb';

@NgModule({
  declarations: [
    NavBarComponent,
    SectionHeadeComponent
  ],
  imports: [
    CommonModule,
    MatBadgeModule,
    RouterModule,
    BreadcrumbModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    })
  ],
  exports: [
    NavBarComponent,
    SectionHeadeComponent
  ]
})
export class CoreModule { }
