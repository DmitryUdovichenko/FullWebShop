import { ICreateProduct } from "./createProduct";

export class ProductForm implements ICreateProduct {
    name = '';
    description = '';
    price = 0;
    imageUrl = "";
    productType: string;
    productBrand: string;
  
    constructor(init?: ProductForm) {
      Object.assign(this, init);
    }
    
}