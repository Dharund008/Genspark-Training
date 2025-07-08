
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router , RouterLink } from '@angular/router';
import { AuthService } from '../services/AuthService';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.css']
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  isSubmitted = false;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      
      const email = this.forgotPasswordForm.value.email;
      
      this.authService.forgotPassword({email}).subscribe({
        next: (data : any ) => {
          console.log(data);
          this.isLoading = false;
          this.isSubmitted = true;
        },
        error: (error) => {
          console.log(email);
          this.isLoading = false;
          this.errorMessage = 'Error sending reset email. Please try again.';
          console.error('Forgot password error:', error);
        }
      });
    }
  }
}
