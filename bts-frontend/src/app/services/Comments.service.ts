
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comment, CreateCommentRequest } from '../models/Comments.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly API_URL = 'http://localhost:5088/api';
  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getCommentsByBug(bugId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.API_URL}/comments/bug/${bugId}`, {
      headers: this.getAuthHeaders()
    });
  }

  createComment(commentData: CreateCommentRequest): Observable<Comment> {
    return this.http.post<Comment>(`${this.API_URL}/comments`, commentData, {
      headers: this.getAuthHeaders()
    });
  }

  getCommentsByUser(userId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.API_URL}/comments/user/${userId}`, {
      headers: this.getAuthHeaders()
    });
  }
}
