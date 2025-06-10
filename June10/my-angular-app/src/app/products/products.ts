import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-products',
  imports: [FormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
  cartCount: number = 0;

  products = [
    { name: "Camera", price: 300, image: "assets/images/pexels-madebymath-90946.jpg" },
    { name: "Super-Watch", price: 800, image: "assets/images/photo-1523275335684-37898b6baf30.jpeg" },
    { name: "HeadPhones", price: 100, image: "assets/images/premium_photo-1679913792906-13ccc5c84d44.jpeg" }
  ];

  addToCart() {
    if (this.cartCount < 10) {
    this.cartCount++;
  }
  else
  {
    alert("You can't add more than 10 items to cart");
  }

  }
}
