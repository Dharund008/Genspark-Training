
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { Navigation } from '../../shared/navigation/navigation';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, Navigation],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      const role = user.role.toUpperCase();
      switch (role) {
        case 'ADMIN':
          this.router.navigate(['/admin/dashboard']);
          break;
        case 'DEVELOPER':
          this.router.navigate(['/developer/dashboard']);
          break;
        case 'TESTER':
          this.router.navigate(['/tester/dashboard']);
          break;
        default:
          // Stay on general dashboard
          break;
      }
    }
  }
}
