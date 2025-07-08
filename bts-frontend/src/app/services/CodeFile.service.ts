
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CodeFileLog } from '../models/Staticstics.model';

@Injectable({
  providedIn: 'root'
})
export class CodeFileService {
  private readonly API_URL = 'http://localhost:5088/api';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  uploadCodeFile(file: File, bugId?: number): Observable<CodeFileLog> {
    const formData = new FormData();
    formData.append('file', file);
    if (bugId) {
      formData.append('bugId', bugId.toString());
    }
    
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post<CodeFileLog>(`${this.API_URL}/api/code-files/upload`, formData, {
      headers: headers
    });
  }

  getCodeFilesByDeveloper(developerId: number): Observable<CodeFileLog[]> {
    return this.http.get<CodeFileLog[]>(`${this.API_URL}/api/code-files/developer/${developerId}`, {
      headers: this.getAuthHeaders()
    });
  }

  getAllCodeFiles(): Observable<CodeFileLog[]> {
    return this.http.get<CodeFileLog[]>(`${this.API_URL}/api/code-files`, {
      headers: this.getAuthHeaders()
    });
  }

  downloadCodeFile(filePath: string): Observable<Blob> {
    return this.http.get(`${this.API_URL}/api/code-files/download`, {
      params: { filePath },
      headers: this.getAuthHeaders(),
      responseType: 'blob'
    });
  }
}
