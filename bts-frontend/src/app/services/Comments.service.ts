
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comment, CommentRequestDTO } from '../models/Comments.model';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = 'http://localhost:5088/api/Comment';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  addComment(comment: CommentRequestDTO): Observable<Comment> {
    return this.http.post<Comment>(`${this.apiUrl}/add-comment`, comment, {
      headers: this.getAuthHeaders()
    });
  }

  getBugComments(bugId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/bug/${bugId}`, {
      headers: this.getAuthHeaders()
    });
  }

  getAllComments(): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/comments-all`, {
      headers: this.getAuthHeaders()
    });
  }

  getCommentsByRole(userRole: string): Observable<Comment[]> {
    const params = new HttpParams().set('userRole', userRole);
    return this.http.get<Comment[]>(`${this.apiUrl}/user-role`, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }
}
