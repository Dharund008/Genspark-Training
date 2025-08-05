import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService } from './AuthService';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  public messages: string[] = [];
  private isStarted = false;
  
  private messagesSubject = new BehaviorSubject<string | null>(null);
  public messages$: Observable<string | null> = this.messagesSubject.asObservable();


  constructor(private zone: NgZone, private authService: AuthService) {}

  startConnection(loggedInUserId: string, loggedInUserRole: string): void {
    if (this.isStarted) {
      return;
    }
    this.isStarted = true;

    const hubUrl = `http://localhost:5088/notificationHub?userId=${loggedInUserId}&role=${loggedInUserRole}`;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        // skipNegotiation: true,
        skipNegotiation: false,
        accessTokenFactory: () => this.authService.getToken() || '',
        transport: signalR.HttpTransportType.WebSockets,
        //transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('✅ SignalR connected'))
      .catch(err => {
        console.error('SignalR connection error:', err);
        this.isStarted = false;
      });

    this.registerHandlers();
  }

  private registerHandlers(): void {
    this.hubConnection.on('ReceiveMessage', (message: string) => {
      this.zone.run(() => {
        console.log('📩 Notification received:', message);
        this.messages.push(message);
        this.messagesSubject.next(message);
      });
    });

  }

  stopConnection(): void {
    if (this.hubConnection && this.isStarted) {
      this.hubConnection.stop()
        .then(() => {
          console.log('SignalR connection stopped');
          this.isStarted = false;
        })
        .catch(err => console.error('Stop error:', err));
    }
  }

  getAllMessages(): string[] {
    return [...this.messages];
  }

  isConnectionActive(): boolean {
    return this.isStarted;
  }
}
