import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { AdminService } from '../../../services/AdminService';
import { Developer, Tester, DeveloperRequestDTO, TesterRequestDTO, User } from '../../../models/UserModel';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule, Navigation],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css'
})
export class UserManagement implements OnInit {
  users: User[] = [];
  activeTab = 'developers';
  developers: Developer[] = [];
  testers: Tester[] = [];
  userType = 'developer';
  newUser: DeveloperRequestDTO | TesterRequestDTO = {
    name: '',
    email: '',
    password: ''
  };
  confirmPassword: string = '';
  showCreateDeveloper = false;
  showCreateTester = false;
  isLoading = false;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(private adminService: AdminService) {}

  ngOnInit() {
    this.loadAllData();
  }

  loadAllData() {
    this.loadUsers();
    this.loadDevelopers();
    this.loadTesters();
  }

  loadUsers() {
    this.isLoading = true;
    this.adminService.getAllUsers().subscribe({
      next: (response: any) => {
      this.users = Array.isArray(response?.$values) ? response.$values : [];
      this.isLoading = false;
    },
      error: (error) => {
        console.error('Error loading users:', error);
        this.errorMessage = 'Failed to load users';
        this.isLoading = false;
        this.users = [];
      }
    });
  }

  loadDevelopers() {
    this.adminService.getAllDevelopers().subscribe({
      // next: (developers) => {
      //   this.developers = Array.isArray(developers) ? developers : [];
      // },
      next: (response: any) => {
      this.developers = Array.isArray(response?.$values) ? response.$values : [];
    },
      error: (error) => {
        console.error('Error loading developers:', error);
        this.developers = [];
      }
    });
  }

  loadTesters() {
    this.adminService.getAllTesters().subscribe({
      next: (response: any) => {
      this.testers = Array.isArray(response?.$values) ? response.$values : [];
      this.isLoading = false;
    },
      error: (error) => {
        console.error('Error loading testers:', error);
        this.testers = [];
      }
    });
  }

   deleteDeveloper(developerId: string) {
    if (confirm('Are you sure you want to delete this developer?')) {
      this.adminService.deleteDeveloper(developerId).subscribe({
        next: () => {
          this.successMessage = 'Developer deleted successfully';
          this.loadAllData();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          console.error('Error deleting developer:', error);
          this.errorMessage = 'Failed to delete developer';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

  deleteTester(testerId: string) {
    if (confirm('Are you sure you want to delete this tester?')) {
      this.adminService.deleteTester(testerId).subscribe({
        next: () => {
          this.successMessage = 'Tester deleted successfully';
          this.loadAllData();
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          console.error('Error deleting tester:', error);
          this.errorMessage = 'Failed to delete tester';
          setTimeout(() => this.errorMessage = '', 3000);
        }
      });
    }
  }

    createDeveloper() {
    if (!this.newUser.name || !this.newUser.email || !this.newUser.password) {
      this.errorMessage = 'Please fill in all fields';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }
    if (this.newUser.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }

    this.adminService.createDeveloper(this.newUser as DeveloperRequestDTO).subscribe({
      next: () => {
        this.successMessage = 'Developer created successfully';
        this.loadAllData();
        this.showCreateDeveloper = false;
        this.newUser = { name: '', email: '', password: '' };
        this.confirmPassword = '';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error creating developer:', error);
        this.errorMessage = error.error?.error || 'Failed to create developer';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

    createTester() {
    if (!this.newUser.name || !this.newUser.email || !this.newUser.password) {
      this.errorMessage = 'Please fill in all fields';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }
    if (this.newUser.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }

    this.adminService.createTester(this.newUser as TesterRequestDTO).subscribe({
      next: () => {
        this.successMessage = 'Tester created successfully';
        this.loadAllData();
        this.showCreateTester = false;
        this.newUser = { name: '', email: '', password: '' };
        this.confirmPassword = '';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error creating tester:', error);
        this.errorMessage = error.error?.error || 'Failed to create tester';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  cancelCreateDeveloper() {
    this.showCreateDeveloper = false;
    this.newUser = { name: '', email: '', password: '' };
    this.confirmPassword = '';
  }

  cancelCreateTester() {
    this.showCreateTester = false;
    this.newUser = { name: '', email: '', password: '' };
    this.confirmPassword = '';
  }

}

