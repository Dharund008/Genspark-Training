// import { HttpClient } from "@angular/common/http"
// import { inject } from "@angular/core"
// import { UserLoginModel } from "../models/UserLoginModel";
// import { jwtDecode } from "jwt-decode";
// import { UserSignUpModel } from "../models/UserSignUp";

// export class AuthService
// {
//     private http = inject(HttpClient);
    
//     //login
//     login(user:UserLoginModel)
//     {
//         return this.http.post('http://localhost:5088/api/Authentication/login',user);
//     }
//     getrolefromtoken(token:string)
//     {
//         const decoded: any = jwtDecode(token);
//         return decoded.role;
//     }

//     //signup [only for admin]
//     singup(singupData:UserSignUpModel)
//     {
//         return this.http.post('http://localhost:5088/api/Admin/Add-Admin',singupData);
//     }

//     logout()
//     { 
//         localStorage.removeItem('token');
//         //this.router.navigate(['/login']);
//     }
// }

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { LoginRequest, LoginResponse, User, ForgotPasswordRequest, ResetPasswordRequest } from '../models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5088/api'; 
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loadUserFromStorage();
  }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

    login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Authentication/login`, credentials, {
      headers: this.getHeaders()
    }).pipe(
      tap(response => {
        if (response.token && response.user) {
          localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        }
      })
    );
  }
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/home']);
  }

  forgotPassword(request: ForgotPasswordRequest): Observable<any> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  return this.http.post(
    `${this.apiUrl}/User/forgot-password`,
    request,
    { headers }
  );
  }
  getMyDetails(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/api/auth/mydetails`, {
      headers: this.getAuthHeaders()
    });
  }


  resetPassword(request: ResetPasswordRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/User/forgot-password`, request);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

   isLoggedIn(): boolean {
    return !!this.getToken() && !!this.getCurrentUser();
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp > Date.now() / 1000;
    } catch {
      return false;
    }
  }

  private setCurrentUser(user: User, token: string): void {
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private loadUserFromStorage(): void {
    const token = this.getToken();
    const userStr = localStorage.getItem('user');
    if (token && userStr) {
      try {
        const user = JSON.parse(userStr);
        this.currentUserSubject.next(user);
      } catch (error) {
        console.error('Error parsing user from storage:', error);
        this.logout();
      }
    }
  }

}
