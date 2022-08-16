import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.sass']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', {static: false}) search: ElementRef;
  products: IProduct[];
  types: IType[];
  brands: IBrand[];
  shopParams = new ShopParams();
  totalCount: number;
  sortOptions = [
    {name: "name", value: "name"},
    {name: "Price lth", value: "priceAsc"},
    {name: "Price htl", value: "priceDesc"}
  ];


  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
        next: (response)=> {
          this.products = response.data;
          this.shopParams.pageNumber = response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        },
        error: (e) => console.error(e)
      });
  }

  getBrands(){
    this.shopService.getBrands().subscribe({
        next: (response)=> {this.brands = [{id: 0, name: "None"},...response]},
        error: (e) => console.error(e)
      });
  }

  getTypes(){
    this.shopService.getTypes().subscribe({
        next: (response)=> {this.types = [{id: 0, name: "None"},...response]},
        error: (e) => console.error(e)
      });
  }

  onBrandSelected(brandId: number){    
    this.shopParams.brandId = brandId ? brandId : null;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    this.shopParams.typeId = typeId ? typeId : null;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sortValue: string){
    this.shopParams.sortType = sortValue ? sortValue : null;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onChangePage(pe:PageEvent){
    this.shopParams.pageNumber = pe.pageIndex+1;
    this.shopParams.pageSize = pe.pageSize;
    this.getProducts();    
  }

  onSearch(){
    this.shopParams.search = this.search.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts(); 
  }
}
