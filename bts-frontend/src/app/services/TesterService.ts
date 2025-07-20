
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bug, BugSubmissionDTO, UpdateBugPatchDTO } from '../models/bug.model';
import { Tester } from '../models/UserModel';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})
export class TesterService {
  private apiUrl = 'http://localhost:5088/api/Tester';

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

  createBug(bug: BugSubmissionDTO): Observable<Bug> {
    return this.http.post<Bug>(`${this.apiUrl}/create-bug`, bug, {
      headers: this.getAuthHeaders()
    });
  }

  uploadScreenshot(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${this.apiUrl}/upload-screenshot`, formData, {
      headers: this.getMultipartHeaders()
    });
  }

  updateBugDetails(bugId: number, updates: UpdateBugPatchDTO): Observable<Bug> {
    const params = new HttpParams().set('bugId', bugId.toString());
    
    return this.http.patch<Bug>(`${this.apiUrl}/update-bug-details`, updates, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }

  updateBugStatus(bugId: number, newStatus: number): Observable<{ message: string }> {
    const params = new HttpParams().set('newStatus', newStatus.toString());
    
    return this.http.put<{ message: string }>(`${this.apiUrl}/update-bug-status/${bugId}`, null, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }
  // updateBugStatus(bugId: number, newStatus: number): Observable<any> {
  //   const params = new HttpParams()
  //     .set('bugId', bugId.toString())
  //     .set('newStatus', newStatus.toString());

  //   return this.http.put(`${this.apiUrl}/status`, null, {
  //     headers: this.getAuthHeaders(),
  //     params: params
  //   });
  // }


  getMyBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}/my-bugs`, {
      headers: this.getAuthHeaders()
    });
  }

  getTesterByEmail(email: string): Observable<Tester> {
    const params = new HttpParams().set('email', email);
    return this.http.get<Tester>(`${this.apiUrl}/tester-by-email`, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }

  getAllTesters(): Observable<Tester[]> {
    return this.http.get<Tester[]>(`${this.apiUrl}/all-testers`, {
      headers: this.getAuthHeaders()
    });
  }

  getTestersWithBugs(): Observable<Tester[]> {
    return this.http.get<Tester[]>(`${this.apiUrl}/testers-associated-with-bugs`, {
      headers: this.getAuthHeaders()
    });
  }
}
