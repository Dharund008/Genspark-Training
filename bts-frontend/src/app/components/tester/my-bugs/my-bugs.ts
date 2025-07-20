import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { TesterService } from '../../../services/TesterService';
import { Bug, BugStatus, BugPriority } from '../../../models/bug.model';

@Component({
  selector: 'app-my-bugs',
  standalone: true,
  imports: [CommonModule, FormsModule, Navigation, RouterLink],
  templateUrl: './my-bugs.html',
  styleUrl: './my-bugs.css'
})
export class MyBugs implements OnInit {
  bugs: Bug[] = [];
  filteredBugs: Bug[] = [];
  selectedStatus = '';
  selectedPriority = '';
  isLoading = true;
  errorMessage = '';

  constructor(private testerService: TesterService) {}

  ngOnInit(): void {
    this.loadMyBugs();
  }

  loadMyBugs(): void {
    this.isLoading = true;
    this.testerService.getMyBugs().subscribe({
      next: (response : any) => {
      this.bugs = Array.isArray(response?.$values) ? response.$values : [];
      this.filteredBugs = [...this.bugs];
        this.isLoading = false;
      },
      error: (error) => {
      console.error('Error loading your bugs:', error);
      this.errorMessage = 'Failed to load your bugs';
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
  screenshot(url: string){
    const fullUrl = `http://localhost:5088${url}`;
    return fullUrl;
  }
}

