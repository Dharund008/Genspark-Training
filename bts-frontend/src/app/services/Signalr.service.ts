// import { Injectable } from '@angular/core';
// import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
// import { BehaviorSubject, Observable } from 'rxjs';
// import { AuthService } from './AuthService';

// export interface Notification {
//   id: string;
//   message: string;
//   type: 'info' | 'success' | 'warning' | 'error';
//   timestamp: Date;
//   userId: string;
//   isRead: boolean;
// }

// @Injectable({
//   providedIn: 'root'
// })
// export class SignalRService {
//   private hubConnection: HubConnection | undefined;
//   private notificationsSubject = new BehaviorSubject<Notification[]>([]);
//   public notifications$ = this.notificationsSubject.asObservable();
//   private unreadCountSubject = new BehaviorSubject<number>(0);
//   public unreadCount$ = this.unreadCountSubject.asObservable();

//   constructor(private authService: AuthService) {}

//   public startConnection(): Promise<void> {
//     const user = this.authService.getCurrentUser();
//     if (!user) return Promise.reject('No authenticated user');

//     this.hubConnection = new HubConnectionBuilder()
//       .withUrl('http://localhost:5088/notificationHub', {
//         accessTokenFactory: () => this.authService.getToken() || ''
//       })
//       .build();

//     return this.hubConnection
//       .start()
//       .then(() => {
//         console.log('SignalR Connected');
//         this.registerEventHandlers();
//         this.joinUserGroup(user.id, user.role);
//       })
//       .catch(err => console.log('Error while starting connection: ' + err));
//   }

//   public stopConnection(): void {
//     if (this.hubConnection) {
//       this.hubConnection.stop();
//     }
//   }

//   private registerEventHandlers(): void {
//     if (!this.hubConnection) return;

//     this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
//       const currentNotifications = this.notificationsSubject.value;
//       const updatedNotifications = [notification, ...currentNotifications];
//       this.notificationsSubject.next(updatedNotifications);
//       this.updateUnreadCount();
//     });

//     this.hubConnection.on('BugStatusChanged', (bugId: string, newStatus: string, message: string) => {
//       this.addNotification({
//         id: Date.now().toString(),
//         message: `Bug ${bugId}: ${message}`,
//         type: 'info',
//         timestamp: new Date(),
//         userId: this.authService.getCurrentUser()?.id || '',
//         isRead: false
//       });
//     });

//     this.hubConnection.on('BugAssigned', (bugId: string, developerId: string, message: string) => {
//       this.addNotification({
//         id: Date.now().toString(),
//         message: `Bug ${bugId}: ${message}`,
//         type: 'success',
//         timestamp: new Date(),
//         userId: this.authService.getCurrentUser()?.id || '',
//         isRead: false
//       });
//     });

//     this.hubConnection.on('CodeFileUploaded', (fileName: string, uploaderId: string, message: string) => {
//       this.addNotification({
//         id: Date.now().toString(),
//         message: `Code file uploaded: ${fileName}`,
//         type: 'info',
//         timestamp: new Date(),
//         userId: this.authService.getCurrentUser()?.id || '',
//         isRead: false
//       });
//     });
//   }

//   private joinUserGroup(userId: string, userRole: string): void {
//     if (this.hubConnection) {
//       this.hubConnection.invoke('JoinUserGroup', userId, userRole);
//     }
//   }

//   private addNotification(notification: Notification): void {
//     const currentNotifications = this.notificationsSubject.value;
//     const updatedNotifications = [notification, ...currentNotifications];
//     this.notificationsSubject.next(updatedNotifications);
//     this.updateUnreadCount();
//   }

//   public markAsRead(notificationId: string): void {
//     const notifications = this.notificationsSubject.value;
//     const updatedNotifications = notifications.map(n => 
//       n.id === notificationId ? { ...n, isRead: true } : n
//     );
//     this.notificationsSubject.next(updatedNotifications);
//     this.updateUnreadCount();
//   }

//   public markAllAsRead(): void {
//     const notifications = this.notificationsSubject.value;
//     const updatedNotifications = notifications.map(n => ({ ...n, isRead: true }));
//     this.notificationsSubject.next(updatedNotifications);
//     this.updateUnreadCount();
//   }

//   private updateUnreadCount(): void {
//     const unreadCount = this.notificationsSubject.value.filter(n => !n.isRead).length;
//     this.unreadCountSubject.next(unreadCount);
//   }
// }


import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService } from './AuthService';
import { Notification } from '../models/Notification.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection | null = null;

  
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();
  
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private authService: AuthService) {}

  startConnection(role: string): void {
    const token = this.authService.getToken();
    if (!token) return;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`http://localhost:5088/notificationHub/${role}`, {
        accessTokenFactory: () => token
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection started');
        this.setupEventListeners();
      })
      .catch(err => console.error('Error while starting connection: ', err));
  }

  private setupEventListeners(): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      const currentNotifications = this.notificationsSubject.value;
      this.notificationsSubject.next([notification, ...currentNotifications]);
      this.updateUnreadCount();
    });

    this.hubConnection.on('NotificationRead', (notificationId: number) => {
      const currentNotifications = this.notificationsSubject.value;
      const updatedNotifications = currentNotifications.map(n => 
        n.id === notificationId ? { ...n, isRead: true } : n
      );
      this.notificationsSubject.next(updatedNotifications);
      this.updateUnreadCount();
    });
  }

  loadNotifications(): void {
   
  }

  markAsRead(notificationId: number): void {
    if (this.hubConnection) {
      this.hubConnection.invoke('MarkAsRead', notificationId);
    }
  }

  markAllAsRead(): void {
    if (this.hubConnection) {
      this.hubConnection.invoke('MarkAllAsRead');
    }
  }

  private updateUnreadCount(): void {
    const unreadCount = this.notificationsSubject.value.filter(n => !n.isRead).length;
    this.unreadCountSubject.next(unreadCount);
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = null;
    }
  }
}
