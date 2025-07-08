
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { SignalRService } from '../../services/Signalr.service';
import { User } from '../../models/UserModel';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  unreadNotifications = 0;
  private subscriptions: Subscription[] = [];

  constructor(
    private authService: AuthService,
    private signalRService: SignalRService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.authService.currentUser$.subscribe(user => {
        this.currentUser = user;
        if (user) {
          this.signalRService.startConnection(user.role);
        }
      })
    );

    this.subscriptions.push(
      this.signalRService.unreadCount$.subscribe(count => {
        this.unreadNotifications = count;
      })
    );

    // Load notifications
    this.signalRService.loadNotifications();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    this.signalRService.stopConnection();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/home']);
  }

  markAllNotificationsRead(): void {
    this.signalRService.markAllAsRead();
  }

    getSidebarItems() {
    if (!this.currentUser) return [];

    const commonItems = [
      { icon: 'fas fa-user', label: 'My Profile', route: '/dashboard/profile' },
      { icon: 'fas fa-chart-bar', label: 'Summary', route: '/dashboard/summary' }
    ];

    const roleSpecificItems = [
      { icon: 'fas fa-cogs', label: 'Manage', route: '/dashboard/manage' },
      { icon: 'fas fa-bug', label: 'View Bugs', route: '/dashboard/view-bugs' }
    ];

    return [...commonItems, ...roleSpecificItems];
  }
}