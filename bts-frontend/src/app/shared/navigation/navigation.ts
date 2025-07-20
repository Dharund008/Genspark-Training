import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { NotificationService } from '../../services/notification.service';
import { User } from '../../models/UserModel';
import { NotificationItem } from '../../models/Notification.model';
import { Subscription } from 'rxjs';
import { CodeFileService } from '../../services/CodeFile.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navigation.html',
  styleUrl: './navigation.css'
})
export class Navigation implements OnInit{
 currentUser: User | null = null;
  isMenuOpen = false;
  showNotifications = false;
  notifications: NotificationItem[] = [];
  unreadCount = 0;
  showUploadModal = false;
  selectedFile: File | null = null;
  uploading = false;
  uploadMessage = '';
  uploadStatus = '';

  private subscriptions: Subscription[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
    private codeFileService: CodeFileService
  ) {}
  ngOnInit() {
    this.subscriptions.push(
      this.authService.currentUser$.subscribe(user => {
        this.currentUser = user;
        if (user) {
          console.log('navigation - Current user:', user);
          this.notificationService.startConnection(user.id, user.role);
        } else {
          this.notificationService.stopConnection();
        }
      })
    );

    // Subscribe to notification messages
    this.subscriptions.push(
      this.notificationService.messages$.subscribe(message => {
        if (message) {
          this.addNotification(message);
        }
      })
    );
  }


  private addNotification(message: string) {
    const notification: NotificationItem = {
      message,
      timestamp: new Date(),
      read: false
    };

    this.notifications.unshift(notification);
    this.updateUnreadCount();

    if (this.notifications.length > 50) {
      this.notifications = this.notifications.slice(0, 50);
    }
  }

  private updateUnreadCount() {
    this.unreadCount = this.notifications.filter(n => !n.read).length;
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
    if (this.showNotifications) {
      this.notifications.forEach(n => n.read = true);
      this.updateUnreadCount();
    }
  }

  clearNotifications() {
    this.notifications = [];
    this.unreadCount = 0;
    this.showNotifications = false;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
