import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductModel } from '../models/ProductModel';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}

  searchProducts(query: string, limit: number=10, skip: number=10): Observable<any> {
    return this.http.get<any>(
      `https://dummyjson.com/products/search?q=${query}&limit=${limit}&skip=${skip}`
    );
  }

  getProductById(id: string) {
  return this.http.get<ProductModel>(`https://dummyjson.com/products/${id}`);
}

}
