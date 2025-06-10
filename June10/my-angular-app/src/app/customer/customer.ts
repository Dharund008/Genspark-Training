import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer',
  imports: [FormsModule, CommonModule],
  templateUrl: './customer.html',
  styleUrl: './customer.css'
})
export class Customer {
  name!: string;
  age?: number;
  email?: string;
  isSubmitted:boolean = false;
  likeCount: number = 0;
  dislikeCount: number = 0;

  SubmitCustomer()
  {
    this.isSubmitted = true;
  }

  like() {
    this.likeCount++;
  }

  dislike() {
    this.dislikeCount++;
  }

}
