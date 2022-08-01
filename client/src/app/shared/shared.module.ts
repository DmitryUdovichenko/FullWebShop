import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card'
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonToggleModule} from '@angular/material/button-toggle'; 
import {MatPaginatorModule} from '@angular/material/paginator';
import { PaginationComponent } from './components/pagination/pagination.component';
import { FilterSelectComponent } from './components/filter-select/filter-select.component';


@NgModule({
  declarations: [
    PaginationComponent,
    FilterSelectComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatButtonToggleModule,
    MatPaginatorModule
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
    FilterSelectComponent
  ]
})
export class SharedModule { }
