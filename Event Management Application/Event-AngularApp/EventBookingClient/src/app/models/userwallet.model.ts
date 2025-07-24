export interface UserWallet {
  id: number;
  userId: string;
  email: string;
  username: string;
  balance: number;
  expiry?: string; // or Date if you're converting it
  isExpired?: boolean;
}