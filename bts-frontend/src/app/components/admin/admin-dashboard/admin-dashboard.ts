import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Navigation } from '../../../shared/navigation/navigation';
import { StatisticsService } from '../../../services/Statistics.service';
import { DashboardStats } from '../../../models/Staticstics.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, Navigation, RouterLink],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard implements OnInit {
  stats: DashboardStats | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.statisticsService.getDashboardStats('ADMIN').subscribe({
      next: (stats) => {
        this.stats = stats;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load dashboard statistics';
        this.isLoading = false;
      }
    });
  }
}
