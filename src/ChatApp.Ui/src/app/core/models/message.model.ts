export interface Message {
  id: string;
  conversationId: string;
  senderDisplayName: string;
  content: string;
  sentAtUtc: string;
}
