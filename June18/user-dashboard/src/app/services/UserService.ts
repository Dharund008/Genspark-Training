import { BehaviorSubject, Observable } from "rxjs";
import { User } from "../models/UserModel";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class UserService
{
    private http = inject(HttpClient);

    private apiUrl = 'https://dummyjson.com/users';

    getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
    }

    // Add a new user
    // addUser(user: User): Observable<User> {
    //     return this.http.post<User>(`${this.apiUrl}/add`, user, {
    //     headers: { 'Content-Type': 'application/json' }
    //     });
    // }
}