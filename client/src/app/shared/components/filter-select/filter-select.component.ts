import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IFilter } from '../../models/filter';

@Component({
  selector: 'app-filter-select',
  templateUrl: './filter-select.component.html',
  styleUrls: ['./filter-select.component.sass']
})
export class FilterSelectComponent implements OnInit {
@Input() label: string;
@Input() selectedId: number;
@Input() items: IFilter[];
@Output() filterChanged = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

  onFilterChanged(filterId: number){
    this.filterChanged.emit(filterId);
  }
}
