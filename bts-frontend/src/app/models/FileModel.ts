
export interface UploadedFile {
  id: number;
  fileName: string;
  filePath: string;
  developerId: string;
  uploadedAt: Date;
}

export interface UploadResponse {
  url: string;
}

