export interface DashboardStats {
  //admin
  totalUsers?: number;
  totalBugs: number;
  openBugs: number;
  totalBugsDeleted: number;
  totalBugsClosed: number;
  totalBugsInProgress?: number;
  resolvedBugs?: number;
  totalDevelopers?: number;
  totalTesters?: number;
  totalComments?: number;
  
  
  //tester
  testerComments?: number; //same for every roles.
  bugsCreated?: number;
  bugsDeleted?: number;
  bugsVerified?: number;
  myBugs?: number;
  //developer
  bugsAssigned?: number;
  bugsFixedByUser?: number;
  codeFiles?: number;
  comments?: number;


}
