// import { Component, OnInit } from '@angular/core';
// import { ProductService } from '../services/product.service';
// import { ProductModel } from '../models/product';
// import { Product } from "../product/product";
// import { CartItem } from '../models/cartItem';



// @Component({
//   selector: 'app-products',
//   imports: [Product],
//   templateUrl: './products.html',
//   styleUrl: './products.css'
// })
// export class Products implements OnInit {
//   products:ProductModel[]|undefined=undefined;
//   cartItems:CartItem[] =[];
//   cartCount:number =0;
//   constructor(private productService:ProductService){

//   }
//   handleAddToCart(event:Number)
//   {
//     console.log("Handling add to cart - "+event)
//     let flag = false;
//     for(let i=0;i<this.cartItems.length;i++)
//     {
//       if(this.cartItems[i].Id==event)
//       {
//          this.cartItems[i].Count++;
//          flag=true;
//       }
//     }
//     if(!flag)
//       this.cartItems.push(new CartItem(event,1));
//     this.cartCount++;
//   }
//   ngOnInit(): void {
//     this.productService.getAllProducts().subscribe(
//       {
//         next:(data:any)=>{
//          this.products = data.products as ProductModel[];
//         },
//         error:(err)=>{},
//         complete:()=>{}
//       }
//     )
//   }

// }

import { Component, HostListener, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { Product } from "../product/product";
import { CartItem } from '../models/cartItem';
import { FormsModule } from '@angular/forms';
import { debounce, debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { CommonModule } from '@angular/common';



@Component({
  selector: 'app-products',
  imports: [Product,FormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {
  products:ProductModel[] = [];
  cartItems:CartItem[] =[];
  cartCount:number =0;
  searchString:string=""; 
  searchSubject = new Subject<string>(); //debouncing 
  loading:boolean = false;
  limit = 10;  // Controls how many items are fetched
  skip = 0;    // Used for pagination
  total = 0;   // Stores the total number of available items
  constructor(private productService:ProductService){

  }
  handleSearchProducts(){
    // console.log(this.searchString)
    // if (this.searchString) {
    //   this.skip = 0;  // 🚀 Reset pagination when searching
    // }
    this.skip = 0;
    //because, when skip gets larger, it gets skip all items .. so there will no items when searching results..
    this.searchSubject.next(this.searchString);
    //searchSubject.next used to emit the value to the observable (which is broadcasts to all the subscribers)
  }


  handleAddToCart(event:Number)
  {
    console.log("Handling add to cart - "+event)
    let flag = false;
    for(let i=0;i<this.cartItems.length;i++)
    {
      if(this.cartItems[i].Id==event)
      {
         this.cartItems[i].Count++;
         flag=true;
      }
    }
    if(!flag)
      this.cartItems.push(new CartItem(event,1));
    this.cartCount++;
  }
  ngOnInit(): void {
    // this.productService.getAllProducts().subscribe(
    //   {
    //     next:(data:any)=>{
    //      this.products = data.products as ProductModel[];
    //     },
    //     error:(err)=>{},
    //     complete:()=>{}
    //   }
    // )
    this.searchSubject.pipe(
      debounceTime(500), //wait for 5 seconds before emitting( emitting  : sending the data to the next subscriber)
      distinctUntilChanged(), //used to ignore the duplicate values
      tap(()=>this.loading=true), // tap : used to perform some action before/after emitting the 
      // data which is coming from the previous observable( here we are setting the loading to true , 
      // so that the user can see the loading animation)
      switchMap(query=>this.productService.getProductSearchResult(query,this.limit,this.skip)), // calling api: here we are calling the api to get the search result
       tap(()=>this.loading=false))
       .subscribe({ // now loading is set to false(hides it)
        next:(data:any)=>{
          this.products = data.products as ProductModel[];
          this.total = data.total;
          console.log(this.total);
        }
      });

  }

  //Infinite Scrolling → Instead of showing all products at once, new items load dynamically as the user scrolls.
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
  loadMore(){
      if (this.skip >= this.total) {
      console.log("No more products to load!");
      return;
    }

    this.loading = true;
    console.log(this.skip);
    this.skip += this.limit; //increases skip to move next page of results
    console.log(this.skip);
    this.productService.getProductSearchResult(this.searchString,this.limit,this.skip)
        .subscribe({
          next:(data:any)=>{
            console.log("Fetched products:", data.products);
            this.products = [...this.products,...data.products]; //appending more new products..
            this.loading = false;
          }
        })

      }

}


