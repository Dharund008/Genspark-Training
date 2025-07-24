import { Injectable } from '@angular/core';
import { ApiResponse } from '../../models/api-response.model';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../misc/constants';
import { User } from '../../models/user.model';
import { UserWallet } from '../../models/userwallet.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient) { }
  
  getUserDetails() {
    return this.http.get<ApiResponse>(`${API_BASE_URL}/users/me`);
  }
  
  updateUsername(payload: { username: string }) {
    return this.http.put<ApiResponse>(`${API_BASE_URL}/users/update`, payload);
  }
  
  changePassword(payload: { oldPassword: string; newPassword: string }) {
    return this.http.put<ApiResponse>(`${API_BASE_URL}/users/changepassword`, payload);
  }
  
  getAllUsers(){
    return this.http.get<ApiResponse>(`${API_BASE_URL}/users/all`);
  }
  deleteUser(id:string){
    return this.http.delete<ApiResponse>(`${API_BASE_URL}/users/delete/${id}`);
  }
//   getWallet() {
//   return this.http.get<ApiResponse>(`${API_BASE_URL}/wallet/User`);
// }

  getWallet(): Observable<UserWallet> {
      return this.http.get<UserWallet>(`${API_BASE_URL}/wallet`);
    }

}
