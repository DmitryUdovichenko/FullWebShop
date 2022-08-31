import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { AdminService } from './admin.service';
import {ShopService} from '../shop/shop.service';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit {
  adminProducts: IProduct[];
  shopParams: ShopParams;
  totalCount: number;
  constructor(private shopService: ShopService, private adminService: AdminService) {
    this.shopParams = this.shopService.getShopParams();
    this.shopParams.isAdmin = true;
  }

  ngOnInit(): void {
    this.getAdminProducts();
  }

  getAdminProducts(useCache = false){
    this.shopService.getProducts(useCache, "product/admin-list").subscribe({
        next: (response)=> {
          this.adminProducts = response.data;
          this.totalCount = response.count;
        },
        error: (e) => console.error(e)
      });
  }

  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getAdminProducts(true);
    }
  }

  deleteProduct(id: number) {
    this.adminService.deleteProduct(id).subscribe((response: any) => {
      this.adminProducts.splice(this.adminProducts.findIndex(p => p.id === id), 1);
      this.totalCount--;
    });
  }

}
