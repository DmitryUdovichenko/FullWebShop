import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProductForm } from '../shared/models/ProductForm';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiHost;
  
  constructor(private http: HttpClient) { }

  createProduct(product: ProductForm) {
    return this.http.post(this.baseUrl + 'product', product);
  }

  updateProduct(product: ProductForm, id: number) {
    return this.http.put(this.baseUrl + 'product/' + id, product);
  }

  deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + 'product/' + id);
  }

}
