import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Navigation } from '../navigation/navigation';
import { BugService } from '../../services/BugService';
import { TesterService } from '../../services/TesterService'
import { CommentService } from '../../services/Comments.service';
import { AuthService } from '../../services/AuthService';
import { Bug, BugPriority, BugStatus } from '../../models/bug.model';
import { Comment, CommentRequestDTO } from '../../models/Comments.model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-bug-details',
  standalone: true,
  imports:  [CommonModule, FormsModule, Navigation, RouterLink],
  templateUrl: './bug-details.html',
  styleUrl: './bug-details.css'
})
export class BugDetails implements OnInit {
  bug: Bug | null = null;
  comments: Comment[] = [];
  newComment = '';
  selectedStatus: BugStatus = BugStatus.New;
  currentUser : any;
  
  isLoading = true;
  isAddingComment = false;
  isUpdating = false;
  errorMessage = '';
  successMessage = '';

  // Add getter for codeFileName to ensure it is accessible in template
  get codeFileName(): string | undefined {
    return this.bug?.codeFileName;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private bugService: BugService,
    private commentService: CommentService,
    private authService: AuthService,
    private testerService: TesterService
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();

    const bugId = Number(this.route.snapshot.paramMap.get('id'));
    if (bugId) {
      this.loadBugDetails(bugId);
      this.loadComments(bugId);
    }
  }

  loadBugDetails(id: number): void {
  console.log('Loading bug details for ID:', id);

  this.bugService.getBugById(id).subscribe({
    next: (response: any) => {
      this.bug = response;

      console.log('Bug object after assignment:', this.bug);

      this.selectedStatus = this.bug?.status ?? BugStatus.New;
      this.isLoading = false;
    },
    error: (error) => {
      console.error('Error loading bug details:', error);
      this.errorMessage = 'Failed to load bug details';
      this.isLoading = false;
    }
  });
}

  loadComments(bugId: number): void {
    this.commentService.getBugComments(bugId).subscribe({
      next: (response: any) => {
        this.comments = Array.isArray(response?.$values) ? response.$values : [];
      },
      error: (error) => {
        console.warn('Failed to load comments:', error);
      }
    });
  }

  addComment(): void {
    if (!this.bug || !this.newComment.trim()) return;

    this.isAddingComment = true;
    const commentData: CommentRequestDTO = {
      bugId: this.bug.id,
      message: this.newComment.trim()
    };

    this.commentService.addComment(commentData).subscribe({
      next: (comment) => {
        this.comments.push(comment);
        this.newComment = '';
        this.isAddingComment = false;
        this.successMessage = 'Comment added successfully';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        this.isAddingComment = false;
        this.errorMessage = 'Failed to add comment';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  updateStatus(): void {
    if (!this.bug) return;

    this.isUpdating = true;
    // this.bugService.updateBugStatus(this.bug.id, this.selectedStatus).subscribe({
    this.bugService.updateBugStatus(this.bug.id, Number(this.selectedStatus)).subscribe({
      next: (response) => {
        console.log('Update response:', response);
        if (this.bug) {
          this.bug.status = this.selectedStatus;
        }
        
        this.isUpdating = false;
        this.successMessage = response.message;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        this.isUpdating = false;
        this.errorMessage = 'Failed to update bug status';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }
   goBack(): void {
    this.location.back();
  }

  canComment(): boolean {
    return !!this.currentUser && !this.bug?.isDeleted && this.bug?.status !== BugStatus.Closed;
  }

  canUpdateStatus(): boolean {
    if (!this.currentUser || this.bug?.isDeleted) return false;
    
    const role = this.currentUser.role;
    const status = this.bug?.status;
    
    if (role === 'ADMIN') {
      return status === BugStatus.Verified; // Can only close verified bugs
    } else if (role === 'DEVELOPER') {
      return status === BugStatus.Assigned || status === BugStatus.InProgress || status === BugStatus.Reopened;
    } else if (role === 'TESTER') {
      return status === BugStatus.Fixed || status === BugStatus.Retesting;
    }
    return false;
  }

  getAvailableStatuses(): BugStatus[] {
    if (!this.currentUser || this.bug?.isDeleted) return [];
    
    const role = this.currentUser.role;
    const currentStatus = this.bug?.status;
    
    if (role === 'ADMIN') {
      return currentStatus === BugStatus.Verified ? [BugStatus.Closed] : [];
    } else if (role === 'DEVELOPER') {
      if (currentStatus === BugStatus.Assigned) return [BugStatus.InProgress];
       if (currentStatus === BugStatus.Reopened) return [BugStatus.InProgress];
      if (currentStatus === BugStatus.InProgress) return [BugStatus.Fixed];
      return [];
    } else if (role === 'TESTER') {
      if (currentStatus === BugStatus.Fixed) return [BugStatus.Retesting];
      if (currentStatus === BugStatus.Retesting) return [BugStatus.Verified, BugStatus.Reopened];
      return [];
    }
    return [];
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
  viewScreenshot(url: string): void {
    const fullUrl = `http://localhost:5088${url}`;
    window.open(fullUrl, '_blank');
  }
}

