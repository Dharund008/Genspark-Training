
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bug, BugSubmissionDTO, UpdateBugPatchDTO, BugStats } from '../models/bug.model';
import { map } from 'rxjs/operators';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})
export class BugService {
  private apiUrl = 'http://localhost:5088/api/Bug';
  private apiUrl2 = 'http://localhost:5088/api/Tester';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  private getMultipartHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(response => Array.isArray(response) ? response : [])
    );
  }

 

  getBugById(id: number): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}/bug-id/${id}`, {
      headers: this.getAuthHeaders()
    });
  }
  updateBugStatus(bugId: number, newStatus: number): Observable<{ message: string }> {
    const params = new HttpParams().set('newStatus', newStatus.toString());
    
    return this.http.put<{ message: string }>(`${this.apiUrl}/update-bug-status/${bugId}`, null, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }

  getPaginatedBugs(page: number = 1, pageSize: number = 7): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}/paginated-bugsall?page=${page}&pageSize=${pageSize}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(response => Array.isArray(response) ? response : [])
    );
  }
  

 getAssignedBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}/assigned-bugs`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(response => Array.isArray(response) ? response : [])
    );
  }
}