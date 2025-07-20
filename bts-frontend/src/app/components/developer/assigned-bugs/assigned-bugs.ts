import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { DeveloperService } from '../../../services/DeveloperService';
import { Bug, BugStatus, BugPriority, UpdateBugPatchDTO } from '../../../models/bug.model';

@Component({
  selector: 'app-assigned-bugs',
  standalone: true,
  imports: [CommonModule, FormsModule, Navigation, RouterLink],
  templateUrl: './assigned-bugs.html',
  styleUrl: './assigned-bugs.css'
})
export class AssignedBugs implements OnInit {
  bugs: Bug[] = [];
  filteredBugs: Bug[] = [];
  selectedStatuses: { [bugId: number]: BugStatus } = {};
  selectedStatus = '';
  selectedPriority = '';
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private developerService: DeveloperService) {}

  ngOnInit(): void {
    this.loadAssignedBugs();
  }

  loadAssignedBugs() {
  this.isLoading = true;
  this.errorMessage = '';

  this.developerService.getAssignedBugs().subscribe({
    next: (bugs: any) => {
      this.bugs = Array.isArray(bugs?.$values) ? bugs.$values : [];
      this.filteredBugs = [...this.bugs];
      this.isLoading = false;

      this.bugs.forEach(bug => {
        if (bug.status === BugStatus.Assigned) {
          this.selectedStatuses[bug.id] = BugStatus.InProgress;
        } else if (bug.status === BugStatus.InProgress) {
          this.selectedStatuses[bug.id] = BugStatus.Fixed;
        }
      });
    },
    error: (error) => {
      console.error('Error loading assigned bugs:', error);
      this.errorMessage = 'Failed to load assigned bugs';
      this.isLoading = false;
      this.bugs = [];
      this.filteredBugs = [];
    }
  });
}


  filterBugs(): void {
    this.filteredBugs = this.bugs.filter(bug => {
      const statusMatch = !this.selectedStatus || bug.status.toString() === this.selectedStatus;
      const priorityMatch = !this.selectedPriority || bug.priority.toString() === this.selectedPriority;
      return statusMatch && priorityMatch;
    });
  }

  updateBugStatus(bugId: number, newStatus: BugStatus): void {
    if (!newStatus) return;

    this.developerService.updateBugStatus(bugId, newStatus).subscribe({
      next: () => {
        this.successMessage = 'Bug status updated successfully';
        this.loadAssignedBugs();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        console.error('Error updating bug status:', error);
        this.errorMessage = 'Failed to update bug status';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getPriorityText(priority: BugPriority): string {
    const priorities = ['Low', 'Medium', 'High', 'Critical'];
    return priorities[priority] || 'Unknown';
  }

  getPriorityClass(priority: BugPriority): string {
    const classes = ['low', 'medium', 'high', 'critical'];
    return classes[priority] || 'low';
  }

  getStatusText(status: BugStatus): string {
    const statuses = ['New', 'Assigned', 'In Progress', 'Fixed', 'Retesting', 'Verified', 'Reopened', 'Closed'];
    return statuses[status] || 'Unknown';
  }

  getStatusClass(status: BugStatus): string {
    const classes = ['new', 'assigned', 'in-progress', 'fixed', 'retesting', 'verified', 'reopened', 'closed'];
    return classes[status] || 'new';
  }

  canUpdateStatus(bug: Bug): boolean {
    // Developer can only update status from Assigned to InProgress or InProgress to Fixed
    return bug.status === BugStatus.Assigned || bug.status === BugStatus.InProgress || bug.status === BugStatus.Reopened;
  }

  getAvailableStatuses(currentStatus: BugStatus): BugStatus[] {
    if (currentStatus === BugStatus.Assigned) {
      return [BugStatus.InProgress];
      
    }
    else if (currentStatus === BugStatus.Reopened) {
      return [BugStatus.InProgress];
    }
    else if (currentStatus === BugStatus.InProgress) {
      return [BugStatus.Fixed];
    }
    return [];
  }
}

