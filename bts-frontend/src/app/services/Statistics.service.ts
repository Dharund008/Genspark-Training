
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminStatistics, DeveloperStatistics, TesterStatistics } from '../models/Staticstics.model';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private readonly API_URL = 'http://localhost:5088/api/Statistics';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAdminStatistics(): Observable<AdminStatistics> {
    return this.http.get<AdminStatistics>(`${this.API_URL}/ADMIN`, {
      headers: this.getAuthHeaders()
    });
  }

  getDeveloperStatistics(): Observable<DeveloperStatistics> {
    return this.http.get<DeveloperStatistics>(`${this.API_URL}/DEVELOPER`, {
      headers: this.getAuthHeaders()
    });
  }

  getTesterStatistics(): Observable<TesterStatistics> {
    return this.http.get<TesterStatistics>(`${this.API_URL}/TESTER`, {
      headers: this.getAuthHeaders()
    });
  }
}
