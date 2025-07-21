import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Video } from '../Model/TrainingVideo';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class VideoService {
  private apiUrl = 'http://localhost:5010/api/videos';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Video[]> {
    return this.http.get<Video[]>(`${this.apiUrl}/getallvideos`);
  }

  upload(formData: FormData): Observable<Video> {
    return this.http.post<Video>(`${this.apiUrl}/upload`, formData);
  }
}
