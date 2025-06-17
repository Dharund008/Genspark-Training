import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/UserService';
import { UserLoginModel } from '../models/UserLoginModel';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [BrowserModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  user: UserLoginModel = { username: '', password: '' };

  constructor(private userService: UserService, private router: Router) {}

  login() {
    this.userService.validateUserLogin(this.user);
    
    if (this.userService.isAuthenticated()) {
      this.router.navigate(['/home']); // Redirect to home after login
    } else {
      alert('Login failed! Check your credentials.');
    }
  }
}
