import { Component, HostListener, OnInit } from '@angular/core';
import { debounce, debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { ProductService } from '../product-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductModel } from '../Models/product';
import { Product } from '../product/product';

@Component({
  selector: 'app-home',
  imports: [FormsModule, CommonModule, Product],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home implements OnInit {
  products:ProductModel[] = [];
  searchString: string = '';
  searchSubject = new Subject<string>();
  loading: boolean = false;
  limit = 10;
  skip = 0;
  total = 0;

  constructor(private productService: ProductService) {

  }

  ngOnInit(): void {
      this.searchSubject.pipe(
        debounceTime(400),  //Debounce search input
        distinctUntilChanged(),
        tap(() => this.loading = true),
        switchMap(query=>this.productService.getProductSearchResult(query,this.limit,this.skip)), // calling api: here we are calling the api to get the search result
       tap(()=>this.loading=false))
       .subscribe({ // now loading is set to false(hides it)
        next:(data:any)=>{
          this.products = data.products as ProductModel[];
          this.total = data.total;
          console.log(this.total);
        },
        error: (error) => {
          console.error('Error fetching search results:', error);
          this.loading = false;
        }
      });

    this.loadProducts();
  }

  loadProducts(): void {
      if (this.skip >= this.total) {
      console.log("No more products to load!");
      return;
    }
      this.loading = true;
      console.log(this.skip);
      this.skip += this.limit; //increases skip to move next page of results
      console.log(this.skip);
      this.productService.getProductSearchResult(this.searchString, this.limit, this.skip)
     .subscribe({
          next:(data:any)=>{
            console.log("Fetched products:", data.products);
            this.products = [...this.products,...data.products]; //appending more new products..
            this.loading = false;
          },
          error: (error) => {
            console.error('Error loading more products:', error);
            this.loading = false;
          }
        });
  }

  handleSearch(): void {
    this.skip = 0; 
    this.searchSubject.next(this.searchString);
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - 100;
    if (scrollPosition >= threshold && this.products.length < this.total) {
      this.loadProducts();
    }
  }

}
