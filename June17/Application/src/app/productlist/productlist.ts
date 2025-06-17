// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-productlist',
//   imports: [],
//   templateUrl: './productlist.html',
//   styleUrl: './productlist.css'
// })
// export class Productlist {

// }


import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/ProductService';
import { ProductModel } from '../models/ProductModel';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports : [ FormsModule, CommonModule, RouterModule],
  templateUrl: './productlist.html',
  styleUrls: ['./productlist.css']
})
export class ProductDetailComponent implements OnInit {
  product!: ProductModel;
  loading = true;
  errorMessage = '';

  constructor(private route: ActivatedRoute, private productService: ProductService, private router: Router) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id'); // Extract ID from URL

    if (id) {
      this.productService.getProductById(id).subscribe({
        next: (data) => {
          this.product = data;
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Product not found!';
          this.loading = false;
        }
      });
    } else {
      this.router.navigate(['/home']); // Redirect if ID is missing
    }
  }
}
