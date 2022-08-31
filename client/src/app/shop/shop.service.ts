import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IBrand } from '../shared/models/brand';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiHost;
  products: IProduct[] = [];
  brands: IBrand[];
  types: IType[];
  pagination = new Pagination();
  shopParams = new ShopParams();
  productCache = new Map();

  constructor(private http: HttpClient) { }

  getProducts(useCache: boolean, path = 'product')
  {
    if(useCache == false){
      this.productCache = new Map();
    }

    if(this.productCache.size > 0 && useCache == true){
      if(this.productCache.has(Object.values(this.shopParams).join('-'))){
          this.pagination.data = this.productCache.get(Object.values(this.shopParams).join('-'));
          return of(this.pagination);
      }
    }

    let params = new HttpParams();
    
    if(this.shopParams.brandId){
      params = params.append("brandId", this.shopParams.brandId.toString());
    }
    
    if(this.shopParams.typeId){
      params = params.append("typeId", this.shopParams.typeId.toString());
    }

    if(this.shopParams.sortType){
      params = params.append("sort", this.shopParams.sortType);
    }

    if(this.shopParams.pageNumber){
      params = params.append("pageIndex", this.shopParams.pageNumber);
    }

    if(this.shopParams.pageSize){
      params = params.append("pageSize", this.shopParams.pageSize);
    }
    
    if(this.shopParams.search){
      params = params.append("search", this.shopParams.search);
    }

    if(this.shopParams.isAdmin){
      params = params.append("isadmin", this.shopParams.isAdmin);
    }

    return this.http.get<IPagination>(this.baseUrl+path, {observe: 'response', params})
    .pipe(
      map(response =>{
        this.productCache.set(Object.values(this.shopParams).join('-'),response.body.data);
        this.pagination = response.body
        return this.pagination;
      })
    )
  }

  getBrands()
  {
    if(this.brands){
      console.log("Brands",this.brands);
      return of(this.brands);
    }
    return this.http.get<IBrand[]>(this.baseUrl+'product/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  getTypes()
  {
    if(this.types){
      console.log("Types",this.types);
      return of(this.types);
    }
    return this.http.get<IType[]>(this.baseUrl+'product/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );
  }

  getProduct(id: number){
    let product: IProduct;
    this.productCache.forEach((products:IProduct[])=>{
      product = products.find(x => x.id == id);
    })
    if(product)
      return of(product);

    return this.http.get<IProduct>(this.baseUrl+'product/'+id);
  }

  setShopParams(params: ShopParams){
    this.shopParams = params;
  }

  getShopParams(){
    return this.shopParams;
  }
}
