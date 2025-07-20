
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UploadResponse, UploadedFile } from '../models/FileModel';
import { AuthService } from './AuthService';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CodeFileService {
  private apiUrl = 'http://localhost:5088/api/CodeFile';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  uploadCodeFile(file: File): Observable<{message: string}> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<{message: string}>(`${this.apiUrl}/upload`, formData, {
      headers: this.getAuthHeaders(),
      responseType: 'text' as 'json'
    });
  }

  getAllCodeFiles(page: number = 1, pageSize: number = 5): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get(`${this.apiUrl}/get-all-code-files`, { params,  headers: this.getAuthHeaders() });
  }

  getFileLogsByDeveloper(developerId: string): Observable<UploadedFile[]> {
    return this.http.get<UploadedFile[]>(`${this.apiUrl}/filter-developers-filelogs?developerId=${developerId}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(response => Array.isArray(response) ? response : [])
    );
  }

  downloadFile(filename: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/downloadfile?filename=${filename}`, {
      headers: this.getAuthHeaders(),
      responseType: 'blob'
    });
  }
}

