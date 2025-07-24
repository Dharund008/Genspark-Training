import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../../models/user.model';
import { ApiResponse } from '../../../models/api-response.model';
import { UserService } from '../../../services/User/user-service';
import { Router, RouterLink } from '@angular/router';
import { NotificationService } from '../../../services/Notification/notification-service';
import { UserDetails } from "../../user-details/user-details";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, UserDetails],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class Profile implements OnInit {
  user!: User;
  isEditingUsername = false;
  isChangingPassword = false;
  usernameForm!: FormGroup;
  passwordForm!: FormGroup;
  originalName:string = '';
  constructor(
    private userService: UserService, 
    private fb: FormBuilder,public router : Router,
    private notificationService : NotificationService) { }


  ngOnInit(): void {
    this.loadUserDetails();
    this.usernameForm = this.fb.group({
      username: ['', Validators.required]
    });

    this.passwordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }
  CancelEdit(){
    this.usernameForm.patchValue({ username: this.originalName });
    this.isEditingUsername = false;
  }
  loadUserDetails() {
    this.userService.getUserDetails().subscribe((res: ApiResponse) => {
      this.user = res.data;
      this.usernameForm.get('username')?.setValue(this.user.username);
      this.originalName = this.user.username;
    });
  }

  saveUsername() {
    if (this.usernameForm.invalid) return;

    const payload = { username: this.usernameForm.value.username };

    this.userService.updateUsername(payload).subscribe({
      next: (res: ApiResponse) => {
        this.user = res.data;
        this.isEditingUsername = false;
        this.notificationService.success("Username Changed");
        this.loadUserDetails();
      },
      error: () => alert('Failed to update username.')
    });
  }

  changePassword() {
    if (this.passwordForm.invalid) return;

    this.userService.changePassword(this.passwordForm.value).subscribe({
      next: (res: ApiResponse) => {
        alert('Password changed successfully!');
        this.isChangingPassword = false;
        this.passwordForm.reset();
      },
      error: () => alert('Failed to change password.')
    });
  }
  
    cancelPasswordChange() {
      this.isChangingPassword = false;
      this.passwordForm.reset();
    }
}