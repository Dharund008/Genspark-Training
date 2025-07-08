export interface Bug {
  id: string;
  title: string;
  description: string;
  status: BugStatus;
  severity: BugSeverity;
  priority: BugPriority;
  screenshotPath?: string;
  createdAt: string;
  updatedAt: string;
  createdByUser?: string;
  assignedToUser?: string;
}

export enum BugStatus {
  New = 'New',
  Assigned = 'Assigned',
  InProgress = 'InProgress',
  Verfied = 'Verified',
  Fixed = 'Fixed',
  Retesting = 'Retesting',
  Reopened = 'Reopened',
  Closed = 'Closed'
}

export enum BugSeverity {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Critical = 'Critical'
}

export enum BugPriority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Urgent = 'Urgent'
}

export interface CreateBugRequest {
  title: string;
  description: string;
  priority: 'Low' | 'Medium' | 'High' | 'Critical';
  screenshotPath?: string;
}

export interface UpdateBugRequest {
  title?: string;
  description?: string;
  screenshotPath?: string;
}

export interface UpdateBugStatusRequest {
  bugId: number;
  status: 'InProgress' | 'Fixed' | 'Verified' | 'Retesting' | 'Reopened' | 'Closed';
}


export interface AssignBugRequest {
  bugId: number;
  developerId: number;
}

