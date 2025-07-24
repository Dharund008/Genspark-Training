export interface TransactionBill {
  totalPrice: number;
  useWallet: boolean;
  walletDeducted: number;
  remainingAmount: number;
  paymentType: number;
  transactionId: string;
}
