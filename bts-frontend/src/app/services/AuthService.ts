
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from "jwt-decode";
import { Router } from '@angular/router';
import { LoginRequest, RegisterRequest, AuthResponse, ForgotPasswordDTO, ResetPasswordDTO } from '../models/AuthModel';
import { User } from '../models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5088/api/Authentication';
  private userUrl = 'http://localhost:5088/api/User';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private isLoggedInSubject = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  role$ = new BehaviorSubject<string>('');
  token:string|null="";
  headers:any;

  constructor(private http: HttpClient) {
    this.loadCurrentUser();
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
  authorization()
    {
        this.token = localStorage.getItem('token');

        this.headers = new HttpHeaders({
        'Authorization': `Bearer ${this.token}`
        });
    }
// login(credentials: LoginRequest): Observable<AuthResponse> {
  login(user : LoginRequest){
    return this.http.post<AuthResponse>('http://localhost:5088/api/Authentication/login',user)
    .pipe(
      tap(response => {
        console.log('Login response:', response);
          if (response.token) {
          this.setToken(response.token);
          const user = {
            id: '', // No id in response, so empty or parse if available
            username: response.username,
            password: '', // Not returned in response
            role: response.role || ''
          };
          this.currentUserSubject.next(user);

          this.isLoggedInSubject.next(true); 

          this.loadCurrentUser();
        }
      })
    );
  }
 

  register(userData: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData, {
      headers: this.getHeaders()
    });
  }

  forgotPassword(data: ForgotPasswordDTO): Observable<any> {
    return this.http.post(`${this.userUrl}/forgot-password`, data, {
      headers: this.getHeaders()
    });
  }

  resetPassword(data: ResetPasswordDTO): Observable<any> {
    return this.http.post(`${this.userUrl}/reset-password`, data, {
      headers: this.getHeaders(),
      responseType: 'text' as 'json'
    });
  }
  logout(): void {
    localStorage.removeItem('authToken');
    this.clearToken();
    this.currentUserSubject.next(null);
    this.isLoggedInSubject.next(false); 
  }

  getMyDetails(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/my-details`, {
      headers: this.getAuthHeaders()
    });
  }

  private loadCurrentUser(): void {
    const token = this.getToken();
    if (token) {
      this.getMyDetails().subscribe({
        next: (user) => this.currentUserSubject.next(user),
        error: () => this.clearToken()
      });
    }
  }

  setToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  clearToken(): void {
    localStorage.removeItem('authToken');
    this.isLoggedInSubject.next(false); 
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.role === role;
  }

  getrolefromtoken(token:string|any)
    {
        // console.log(token);
        const decoded: any = jwtDecode(token);
        return decoded.role;
    }
  setRole(token:string|null)
    {
        const role = this.getrolefromtoken(token);
        this.role$.next(role);
        return role;
    }
}
