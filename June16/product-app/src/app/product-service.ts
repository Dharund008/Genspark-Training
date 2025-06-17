import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);
  private apiUrl = 'https://dummyjson.com/products/search';

  getProductSearchResult(searchData:string,limit:number=10,skip:number=0)
    {
        return this.http.get(`${this.apiUrl}?q=${searchData}&limit=${limit}&skip=${skip}`)
          .pipe(
            catchError((error) => {
              console.error('Error in ProductService:', error);
              return throwError(() => error);
            })
          );
    }
}
