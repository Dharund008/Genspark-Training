export interface AdminStatistics {
  totalUsers: number;
  totalDevelopers: number;
  totalTesters: number;
  totalAdmins: number;
  totalBugsCreated: number;
  totalBugsDeleted: number;
  totalBugsFixed: number;
  totalBugsInProgress: number;
  totalCommentsByAdmin: number;
  activeDevelopers: number;
  activeTesters: number;
}

export interface DeveloperStatistics {
  totalBugsAssigned: number;
  totalCodeFilesUploaded: number;
  totalCommentsMade: number;
  totalBugsFixed: number;
}

export interface TesterStatistics {
  totalBugsCreated: number;
  totalBugsRetested: number;
  totalBugsVerified: number;
  totalCommentsMade: number;
}

export interface CodeFileLog {
  id: number;
  developerId: string;
  fileName: string;
  filePath: string;
  bugId?: number;
  uploadedAt: string;
}