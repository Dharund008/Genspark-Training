
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminRequestDTO, DeveloperRequestDTO, TesterRequestDTO, Admin, Developer, Tester, User } from '../models/UserModel';
import { Bug } from '../models/bug.model';
import { map } from 'rxjs/operators';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5088/api';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      // 'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }
  // User Management
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Admin/list-users`, {
      headers: this.getHeaders()
    });
  }

  createDeveloper(developer: DeveloperRequestDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/Admin/add-developer`, developer, {
      headers: this.getHeaders()
    });
  }

  createTester(tester: TesterRequestDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/Admin/add-tester`, tester, {
      headers: this.getHeaders()
    });
  }

  deleteDeveloper(developerId: string): Observable<any> {
    const params = new HttpParams().set('developerId', developerId);
    return this.http.delete(`${this.apiUrl}/Admin/delete-developer/`, {
      headers: this.getHeaders(),
      params
    });
  }

  deleteTester(testerId: string): Observable<any> {
    const params = new HttpParams().set('testerId', testerId);
    return this.http.delete(`${this.apiUrl}/Admin/delete-tester/`, {
      headers: this.getHeaders(),
      params
    });
  }

  getAllBugs(page: number = 1, pageSize: number = 4): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get(
      `${this.apiUrl}/Bug/paginated-bugsall`,
      {
        headers: this.getHeaders(),
        params
      }
    );
  }


  assignBug(bugId: number, developerId: string): Observable<any> {
    const params = new HttpParams()
      .set('bugId', bugId.toString())
      .set('developerId', developerId);
    
    return this.http.put(`${this.apiUrl}/Admin/assign-bug`, {}, {
      headers: this.getHeaders(),
      params
    });
  }

  closeBug(bugId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/Admin/close-bug/${bugId}`, {}, {
      headers: this.getHeaders()
    });
  }

  deleteBug(bugId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Admin/delete-bug/${bugId}`, {
      headers: this.getHeaders()
    });
  }

  // Developer Management
  getAllDevelopers(): Observable<Developer[]> {
    return this.http.get<Developer[]>(`${this.apiUrl}/Admin/Every-developers`, {
      headers: this.getHeaders()
    });
  }

  getAvailableDevelopers(): Observable<Developer[]> {
    return this.http.get<Developer[]>(`${this.apiUrl}/Admin/available-developers`, {
      headers: this.getHeaders()
    });
  }

  // Tester Management
  getAllTesters(): Observable<Tester[]> {
    return this.http.get<Tester[]>(`${this.apiUrl}/Admin/Every-testers`, {
      headers: this.getHeaders()
    });
  }

  // Bug Reports by User
  getDeveloperBugs(developerId: string): Observable<Bug[]> {
    const params = new HttpParams().set('developerId', developerId);
    return this.http.get<Bug[]>(`${this.apiUrl}/Admin/all-developer-bugs`, {
      headers: this.getHeaders(),
      params
    });
  }

  getTesterBugs(testerId: string): Observable<Bug[]> {
    const params = new HttpParams().set('testerId', testerId);
    return this.http.get<Bug[]>(`${this.apiUrl}/Admin/all-tester-bugs`, {
      headers: this.getHeaders(),
      params
    });
  }

  checkUserExists(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/Admin/check-user/${email}`, {
      headers: this.getHeaders()
    });
  }
}
