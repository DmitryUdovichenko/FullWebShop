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
  shopParams: ShopParams;
  totalCount: number;
  sortOptions = [
    {name: "name", value: "name"},
    {name: "Price lth", value: "priceAsc"},
    {name: "Price htl", value: "priceDesc"}
  ];


  constructor(private shopService: ShopService) { 
    this.shopParams = this.shopService.getShopParams();
    console.log("ShopParams", this.shopParams);
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  getProducts(useCache = false){
    this.shopService.getProducts(useCache).subscribe({
        next: (response)=> {
          this.products = response.data;
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
    const params = this.shopService.getShopParams();
    params.brandId = brandId ? brandId : null;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    const params = this.shopService.getShopParams();
    params.typeId = typeId ? typeId : null;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sortValue: string){
    const params = this.shopService.getShopParams();
    params.sortType = sortValue ? sortValue : null;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onChangePage(pe:PageEvent){
    const params = this.shopService.getShopParams();
    params.pageNumber = pe.pageIndex+1;
    params.pageSize = pe.pageSize;
    this.shopService.setShopParams(params);
    this.getProducts(true);    
  }

  onSearch(){
    const params = this.shopService.getShopParams();
    this.shopParams.search = this.search.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts(); 
  }
}
