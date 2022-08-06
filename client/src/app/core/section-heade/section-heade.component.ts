import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-heade',
  templateUrl: './section-heade.component.html',
  styleUrls: ['./section-heade.component.sass']
})
export class SectionHeadeComponent implements OnInit {
  breadcrumb$: Observable<any[]>;

  constructor(private breadcrumbService: BreadcrumbService) { }

  ngOnInit(): void {
    this.breadcrumb$ = this.breadcrumbService.breadcrumbs$;
  }

}
