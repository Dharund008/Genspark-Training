import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductModel } from '../models/ProductModel';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css'
})
export class ProductCard {
  @Input() product!: ProductModel;
}
// product!: → The ! is called a "Definite Assignment Assertion"
// It tells TypeScript "Trust me, this will be assigned before being used."


//input statment is require here to get the product data from parent component(home)
//it does dynamically
//its like an delivery system helps to get the provided product data from home to reciever data side which is product-card.

