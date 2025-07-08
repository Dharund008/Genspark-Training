
export interface Comment {
  id: number;
  bugId: number;
  userId: string;
  content: string;
  createdAt: string;
  username?: string;
}

export interface CreateCommentRequest {
  bugId: number;
  content: string;
}
