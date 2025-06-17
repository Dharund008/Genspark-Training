// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-home',
//   imports: [],
//   templateUrl: './home.html',
//   styleUrl: './home.css'
// })
// export class Home {

// }
import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductCard } from '../product-card/product-card';
import { ProductService } from '../services/ProductService';
import { ProductModel } from '../models/ProductModel';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, ProductCard, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  //Onint uses to initialize the component only once, that too for the first time when the component is loaded.
  //prescribely used for initializing the main component in application that is where all the functions take place.
  //used to render componenets : fetches data before component loading
  //variables dynamically , observables

  products: ProductModel[] = [];  // array of products
  loading = false;
  searchTerm = '';
  limit = 10;
  skip = 0;
  total = 0;
  private searchSubject = new Subject<string>(); // observable : string

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.searchSubject.pipe( //pipe :Allows multiple operations to be applied sequentially.
      debounceTime(500),  // Waits 500ms before making API request
      distinctUntilChanged(), //duplicate values don’t trigger unnecessary API requests.
      tap(() => {  //tap : doesn’t modify the stream—just lets you debug, log, or run extra logic.
        this.loading = true;
      }),
      switchMap(term =>
        this.productService.searchProducts(term, this.limit, this.skip)
      ),
      tap(() => this.loading = false)
    ).subscribe({ // now loading is set to false(hides it)
      next: (data: any) => {
        this.products = data.products as ProductModel[];
        this.total = data.total;
        console.log(this.total);
      }
    });

    // this.onSearch('');
  }

  onSearch() {
    this.skip = 0;
    this.searchSubject.next(this.searchTerm); //when user types something in search bar, it triggers the searchSubject.
  }
  //.next used to push the value into streams (observables).

  @HostListener('window:scroll',[]) //used for detecting scroll
  onScroll():void
  {

    const scrollPosition = window.innerHeight + window.scrollY; //innerheight : height of the window : visible area in browser(horizontal)
    //windows.scroll : current scroll position : vertical psoition : that how far user has scrolled
    //here finding out the actual position of bottom of users page
    const threshold = document.body.offsetHeight-100;
    //offsetheigt : total height of page including unseen portion...
    //set 100 to see the loading/trigger animation at the bottom of the page smoothly 

    //why herre 100means , we r saying that before 100pixels of the bottom , ... load the loadmore method...
    if(scrollPosition>=threshold && this.products?.length<this.total)
    {
      //if user scrolled more than the threshold....
      console.log(scrollPosition);
      console.log(threshold)
      
      this.loadMore();
    }
  }

  loadMore() {
    this.loading = true;
    this.skip += this.limit; //increases skip to move next page of results
    this.productService.searchProducts(this.searchTerm, this.limit, this.skip)
       .subscribe({
          next:(data:any)=>{
            console.log("Fetched products:", data.products);
            this.products = [...this.products,...data.products]; //appending more new products..
            this.loading = false;
          }
        })
  }
}
