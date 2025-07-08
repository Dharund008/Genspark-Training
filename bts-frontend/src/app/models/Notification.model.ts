export interface Notification {
  id: number;
  userId: number;
  message: string;
  type: 'Info' | 'Warning' | 'Success' | 'Error';
  isRead: boolean;
  createdAt: string;
}
