import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { AdminService } from '../../../services/AdminService';
import { Bug, BugStatus, BugPriority } from '../../../models/bug.model';
import { Developer } from '../../../models/UserModel';
import { NotificationService } from '../../../services/notification.service'; // Import if not already


@Component({
  selector: 'app-bug-management',
  standalone: true,
  imports: [CommonModule, FormsModule, Navigation, RouterLink],
  templateUrl: './bug-management.html',
  styleUrl: './bug-management.css'
})
export class BugManagement implements OnInit {
  bugs: Bug[] = [];
  filteredBugs: Bug[] = [];
  availableDevelopers: Developer[] = [];
  selectedStatus = '';
  selectedPriority = '';
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  selectedDeveloperIds: { [bugId: number]: string } = {};
  currentPage = 1;
  pageSize = 4;
  totalBugs = 0;
  showReasonInput: { [bugId: number]: boolean } = {};
  deleteReason: { [bugId: number]: string } = {};
  public Math = Math;

  constructor(private adminService: AdminService, private notificationService: NotificationService) { }

  ngOnInit() {
    this.loadBugs();
    this.loadAvailableDevelopers();

    // Auto-refresh bug list on notification
    this.notificationService.messages$.subscribe(message => {
      if (message) {
        this.loadBugs();
        this.loadAvailableDevelopers();
      }
    });
  }

  loadBugs() {
  this.isLoading = true;
  this.errorMessage = '';

  this.adminService.getAllBugs(this.currentPage, this.pageSize).subscribe({
    next: (response: any) => {
      this.bugs = Array.isArray(response?.bugs?.$values) ? response.bugs.$values : [];
      this.filteredBugs = [...this.bugs];


      // this.currentPage = response.page;
      // this.pageSize = response.pageSize;
      this.totalBugs = response.total || 0;

      this.isLoading = false;
    },
    error: (error) => {
      this.setTimedMessage('error', 'Failed to load bugs');
      this.bugs = [];
      this.filteredBugs = [];
      this.isLoading = false;
    }
  });
}


  loadAvailableDevelopers() {
  this.adminService.getAvailableDevelopers().subscribe({
    next: (response: any) => {
      this.availableDevelopers = Array.isArray(response?.$values) ? response.$values : [];
    },
    error: () => {
      this.setTimedMessage('error', 'Failed to load developers');
    }
  });
}


  filterBugs() {
    this.filteredBugs = this.bugs.filter(bug => {
      const statusMatch = !this.selectedStatus || bug.status.toString() === this.selectedStatus;
      const priorityMatch = !this.selectedPriority || bug.priority.toString() === this.selectedPriority;
      return statusMatch && priorityMatch;
    });
    if (this.filteredBugs.length === 0) {
      this.errorMessage = 'No data for the current filter criteria.';
    } else {
      this.errorMessage = '';
    }
  }

  assignBug(bugId: number, developerId: string): void {
   
    if (!developerId) {
      this.errorMessage = 'Please select a developer';
      return;
    }

    this.adminService.assignBug(bugId, developerId).subscribe({
      next: () => {
        this.setTimedMessage('success', 'Bug assigned successfully');
         this.successMessage = 'Bug assigned successfully';
        this.loadBugs();
        this.loadAvailableDevelopers();
        // Clear selected developer for this bug
      delete this.selectedDeveloperIds[bugId];

      setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
      this.errorMessage = error.error?.message || 'Failed to assign bug';
      setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  closeBug(bugId: number) {
    if (confirm('Are you sure you want to close this bug?')) {
      this.adminService.closeBug(bugId).subscribe({
        next: () => {
          this.setTimedMessage('success', 'Bug closed successfully');
          this.loadBugs();
        },
        error: () => {
          this.setTimedMessage('error', 'Failed to close bug');
        }
      });
    }
  }

  // deleteBug(bugId: number) {
  //   if (confirm('Are you sure you want to delete this bug? This action cannot be undone.')) {
  //     this.adminService.deleteBug(bugId).subscribe({
  //       next: () => {
  //         this.setTimedMessage('success', 'Bug deleted successfully');
  //         this.loadBugs();
  //       },
  //       error: () => {
  //         this.setTimedMessage('error', 'Failed to delete bug');
  //       }
  //     });
  //   }
  // }

  deleteBug(bugId: number) {
  const bug = this.bugs.find(b => b.id === bugId);
  let reason = '';

  // Require reason if bug is not closed
  if (bug && bug.status !== BugStatus.Closed) {
    reason = this.deleteReason[bugId]?.trim() || '';
    if (!reason) {
      this.errorMessage = 'Please provide a reason for deletion.';
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    }
  }

  if (confirm('Are you sure you want to delete this bug? This action cannot be undone.')) {
    this.adminService.deleteBug(bugId, reason).subscribe({
      next: () => {
        this.setTimedMessage('success', 'Bug deleted successfully');
        this.loadBugs();
        this.showReasonInput[bugId] = false;
        this.deleteReason[bugId] = '';
      },
      error: () => {
        this.setTimedMessage('error', 'Failed to delete bug');
      }
    });
  }
}

  getPriorityText(priority: BugPriority): string {
    return BugPriority[priority] ?? 'Unknown';
  }

  getPriorityClass(priority: BugPriority): string {
    const classes = ['low', 'medium', 'high', 'critical'];
    return classes[priority] || 'low';
  }

  getStatusText(status: BugStatus): string {
    return BugStatus[status] ?? 'Unknown';
  }

  getStatusClass(status: BugStatus): string {
    const classes = ['new', 'assigned', 'in-progress', 'fixed', 'retesting', 'verified', 'reopened', 'closed'];
    return classes[status] || 'new';
  }

  canAssign(bug: Bug): boolean {
    return bug.status === BugStatus.New || bug.status === BugStatus.Reopened;
  }

  canClose(bug: Bug): boolean {
    return bug.status === BugStatus.Verified;
  }
  showDeleteReason(bugId: number) {
    this.showReasonInput[bugId] = true;
  }

  cancelDelete(bugId: number) {
    this.showReasonInput[bugId] = false;
    this.deleteReason[bugId] = '';
    this.errorMessage = '';
  }

   nextPage(): void {
    if (this.currentPage * this.pageSize < this.totalBugs) {
      this.currentPage++;
      this.loadBugs();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadBugs();
    }
  }

  get hasNextPage(): boolean {
    return this.currentPage * this.pageSize < this.totalBugs;
  }

  get hasPreviousPage(): boolean {
    return this.currentPage > 1;
  }

  setTimedMessage(type: 'error' | 'success', message: string) {
    if (type === 'error') this.errorMessage = message;
    else this.successMessage = message;

    setTimeout(() => {
      this.errorMessage = '';
      this.successMessage = '';
    }, 3000);
  }
}
