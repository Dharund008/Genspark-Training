import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

declare var Razorpay: any;

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './payment.html',
  styleUrls: ['./payment.css']
})
export class Payment {
  
  paymentStatus = '';
  isLoading = false;
  
  form: any;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      amount: [null, [Validators.required, Validators.min(1)]],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    });
  }


  pay() {
    if (this.form.invalid) return;

    const { amount, name, email, contact } = this.form.value;

    this.isLoading = true;

    const options = {
      key: 'rzp_test_1DP5mmOlF5G5ag', 
      amount: Number(amount) * 100,
      currency: 'INR',
      name: name,
      description: 'Test UPI Payment',
      handler: (response: any) => {
        this.isLoading = false;
        this.paymentStatus = 'Success! Payment ID: ' + response.razorpay_payment_id;
      },
      modal: {
        ondismiss: () => {
          this.isLoading = false;
          this.paymentStatus = 'Payment Cancelled';
        }
      },
      prefill: {
        name,
        email,
        contact
      },
      method: {
        upi: true
      }
    };

    const rzp = new Razorpay(options);
    //rzp.open();
    setTimeout(() => rzp.open(), 300);
  }
}
