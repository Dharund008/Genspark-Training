import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../services/AuthService';
import { LoginRequest } from '../models/AuthModel';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading = false;
  loginError = '';
  showPassword = false;
  role : string = '';

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
          console.log('Login successful:', response);
          this.role = this.authService.setRole(response.token); // Set the role from the response token
          console.log('User role:', this.role);
          this.isLoading = false;
          this.redirectToDashboard();
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.isLoading = false;
          this.loginError = error.error?.message || 'Login failed. Please try again.';
        }
      });
    }
  }

  private redirectToDashboard(): void {
    // Wait for user state to be properly set
    setTimeout(() => {
      const user = this.authService.getCurrentUser();
      console.log('Redirecting user:', user);
      if (user) {
        switch (this.role) {
          case 'ADMIN':
            this.router.navigate(['/admin/dashboard']);
            break;
          case 'DEVELOPER':
            this.router.navigate(['/developer/dashboard']);
            break;
          case 'TESTER':
            this.router.navigate(['/tester/dashboard']);
            break;
          default:
            this.router.navigate(['/home']);
        }
      } else {
        // Fallback if user is still not available
        this.router.navigate(['/home']);
      }
    }, 100);
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
