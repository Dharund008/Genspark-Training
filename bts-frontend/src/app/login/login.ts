import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../services/AuthService';
import { UserRole } from '../models/UserModel';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading = false;
  loginError = '';
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
  
    if (this.authService.isAuthenticated()) {
      this.redirectToDashboard();
    }
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.loginError = '';

      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.redirectToDashboard();
        },
        error: (error) => {
          this.isLoading = false;
          this.loginError = error.error?.message || 'Login failed. Please try again.';
        }
      });
    }
  }

  private redirectToDashboard(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      switch (user.role) {
        case UserRole.Admin:
          this.router.navigate(['/admin-dashboard']);
          break;
        case UserRole.Developer:
          this.router.navigate(['/developer-dashboard']);
          break;
        case UserRole.Tester:
          this.router.navigate(['/tester-dashboard']);
          break;
        default:
          this.router.navigate(['/home']);
      }
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  // Demo login methods for testing
  loginAsAdmin(): void {
    this.loginForm.patchValue({
      username: 'admin',
      password: 'admin123'
    });
    this.onLogin();
  }

  loginAsDeveloper(): void {
    this.loginForm.patchValue({
      username: 'developer',
      password: 'dev123'
    });
    this.onLogin();
  }

  loginAsTester(): void {
    this.loginForm.patchValue({
      username: 'tester',
      password: 'test123'
    });
    this.onLogin();
  }
}
