
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardStats } from '../models//Staticstics.model';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private apiUrl = 'http://localhost:5088/api/Statistics';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getDashboardStats(role: string): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/${role}`, {
      headers: this.getAuthHeaders()
    });
  }
}
