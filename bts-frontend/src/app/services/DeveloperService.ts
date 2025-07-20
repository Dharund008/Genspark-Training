
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bug, BugStatus } from '../models/bug.model';
import { Developer } from '../models/UserModel';
import { AuthService } from './AuthService';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DeveloperService {
  private apiUrl = 'http://localhost:5088/api/Developer';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAssignedBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.apiUrl}/assigned-bugs`, {
      headers: this.getAuthHeaders()
    });
  }

 updateBugStatus(bugId: number, newStatus: BugStatus): Observable<any> {
    const params = new HttpParams()
      .set('bugId', bugId.toString())
      .set('newStatus', newStatus.toString());

    return this.http.put(`${this.apiUrl}/update-bug-status`, {}, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }

  getDeveloperByEmail(email: string): Observable<any> {
    const params = new HttpParams().set('email', email);
    return this.http.get(`${this.apiUrl}/developer-by-email`, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }

   getAllDevelopers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all-developers`, {
      headers: this.getAuthHeaders()
    });
  }

  getTestersForMyBugs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/my-bugs-testers`, {
      headers: this.getAuthHeaders()
    });
  }

  getDevelopersWithBugs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/developer-with-bugs`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(response => Array.isArray(response) ? response : [])
    );
  }
}
