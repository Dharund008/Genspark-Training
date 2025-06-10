import { Component, NgModule } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { First } from "./first/first";
import { Customer } from './customer/customer';
import { Products } from './products/products';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, Customer, Products]
})

// @NgModule({
//   imports: [FormsModule]
// })

export class App {
  protected title = 'my-angular-app';
}
