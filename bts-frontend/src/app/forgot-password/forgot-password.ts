import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/AuthService';
import { ForgotPasswordDTO, ResetPasswordDTO } from '../models/AuthModel';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.html',
  styleUrls: ['./forgot-password.css']
})
export class ForgotPasswordComponent implements OnInit {
  step = 1; // 1: Email input, 2: Token and new password
  forgotForm: FormGroup;
  resetForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  email = '';
  resetToken: string = ''; 
  showTokenAlert: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.resetForm = this.fb.group({
      token: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onForgotSubmit(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const forgotPasswordData: ForgotPasswordDTO = {
      email: this.forgotForm.get('email')?.value
    };

    this.authService.forgotPassword(forgotPasswordData).subscribe({
      next: (response) => {
       // this.isLoading = false;
        this.email = this.forgotForm.get('email')?.value;
        this.resetToken = response.token || response.Token || '';

         // token alert for 20 seconds
          this.showTokenAlert = true;
          setTimeout(() => {
            this.showTokenAlert = false;
          }, 20000);

        this.step = 2;
        this.successMessage = 'Reset token sent! Enter the token.';
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Failed to send reset email. Please try again.';
      }
    });
  }

  onResetSubmit(): void {
    if (this.passwordMismatch()) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const resetPasswordData: ResetPasswordDTO = {
      email: this.email,
      token: this.resetForm.get('token')?.value,
      newPassword: this.resetForm.get('newPassword')?.value
    };

    this.authService.resetPassword(resetPasswordData).subscribe({
  next: (response) => {

    console.log('Reset response:', response);

    this.isLoading = false;
    this.successMessage = typeof response === 'string'
      ? response
      : 'Password reset successfully!';

    setTimeout(() => {
      this.router.navigate(['/login']);
    }, 2000);
  },
  error: (error) => {
    this.isLoading = false;
    this.errorMessage = error.error?.message || 'Failed to reset password. Please try again.';
  }
});

  }

  passwordMismatch(): boolean {
    const newPassword = this.resetForm.get('newPassword')?.value;
    const confirmPassword = this.resetForm.get('confirmPassword')?.value;
    return newPassword !== confirmPassword && confirmPassword !== '';
  }

  goBackToEmailStep(): void {
    this.step = 1;
    this.errorMessage = '';
    this.successMessage = '';
  }

  goBackToEmail(): void {
    this.step = 1;
    this.errorMessage = '';
    this.successMessage = '';
    this.resetForm.reset();
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}