import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './AuthService';
import {
  Bug,
  CreateBugRequest,
  UpdateBugRequest,
  AssignBugRequest,
  UpdateBugStatusRequest
} from '../models/bug.model';

@Injectable({
  providedIn: 'root'
})
export class BugService {
  private baseUrl = 'http://localhost:5088/api/Bug';


constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAllBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.baseUrl}`, { headers: this.getAuthHeaders() });
  }

 getBugsByAssignedUser(userId: string): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.baseUrl}/api/bugs/assigned/${userId}`, {
      headers: this.getAuthHeaders()
    });
  }

  getBugsByCreatedUser(userId: string): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.baseUrl}/api/bugs/created/${userId}`, {
      headers: this.getAuthHeaders()
    });
  }

  createBug(bugData: CreateBugRequest): Observable<Bug> {
    return this.http.post<Bug>(`${this.baseUrl}/api/bugs`, bugData, {
      headers: this.getAuthHeaders()
    });
  }

  assignBug(assignData: AssignBugRequest): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/bugs/assign`, assignData, {
      headers: this.getAuthHeaders()
    });
  }

  updateBugStatus(statusData: UpdateBugStatusRequest): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/bugs/status`, statusData, {
      headers: this.getAuthHeaders()
    });
  }

  closeBug(bugId: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/bugs/${bugId}/close`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  softDeleteBug(bugId: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/bugs/${bugId}/soft-delete`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  uploadScreenshot(file: File): Observable<{filePath: string}> {
    const formData = new FormData();
    formData.append('screenshot', file);
    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<{filePath: string}>(`${this.baseUrl}/api/bugs/upload-screenshot`, formData, {
      headers: headers
    });
  }

  getBugStatusLogs(bugId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/api/bugs/${bugId}/status-logs`, {
      headers: this.getAuthHeaders()
    });
  }
}
