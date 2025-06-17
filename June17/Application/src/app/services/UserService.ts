import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserLoginModel } from '../models/UserLoginModel';
import { inject } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private usernameSubject = new BehaviorSubject<string | null>(null);
  username$: Observable<string | null> = this.usernameSubject.asObservable();

  validateUserLogin(user: UserLoginModel) {
    if (user.username.length < 3) {
      this.usernameSubject.next(null);
      this.usernameSubject.error('Username too short');
    } else {
      this.callLoginAPI(user).subscribe({
        next: (data: any) => {
          this.usernameSubject.next(user.username);
          localStorage.setItem('token', data.token); // Store API token
        },
        error: () => alert('Invalid credentials'),
      });
    }
  }

  callGetProfile() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
    return this.http.get('https://dummyjson.com/auth/me', { headers });
  }

  callLoginAPI(user: UserLoginModel) {
    return this.http.post('https://dummyjson.com/auth/login', user);
  }

  logout() {
    localStorage.removeItem('token');
    this.usernameSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token'); // Returns true if token exists
  }
}
