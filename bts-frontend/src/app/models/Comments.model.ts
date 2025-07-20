
export interface Comment {
  id: number;
  bugId: number;
  userId: string;
  message: string;
  createdAt: string;
  userRole?: string;
}

export interface CommentRequestDTO {
  bugId: number;
  message: string;
}
