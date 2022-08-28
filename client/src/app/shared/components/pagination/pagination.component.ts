import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.sass']
})
export class PaginationComponent implements OnInit {
@Input() totalCount: number;
@Input() pageSize: number;
@Input() pageNumber: number;
@Output() pageChanged = new EventEmitter<PageEvent>();

  constructor() { }

  ngOnInit(): void {
  }

  onPageChanged(event:PageEvent){
    this.pageChanged.emit(event);
  }
}
