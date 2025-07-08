import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  public messages: string[] = [];
  private isStarted = false;

  constructor(private zone: NgZone) {}

  startConnection(): void {
    if (this.isStarted) {
      return;
    }
    this.isStarted = true;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5077/notificationHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch((err) => {
        console.error('SignalR error:', err);
        this.isStarted = false;
      });

    this.hubConnection.on('ReceiveNotification', (notification: string) => {
      this.zone.run(() => {
        this.messages.push(notification);
      });
    });
  }
}