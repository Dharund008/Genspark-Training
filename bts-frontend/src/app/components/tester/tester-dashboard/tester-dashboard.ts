import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { StatisticsService } from '../../../services/Statistics.service';
import { TesterService } from '../../../services/TesterService';
import { DashboardStats } from '../../../models/Staticstics.model';
import { CodeFileService } from '../../../services/CodeFile.service';

@Component({
  selector: 'app-tester-dashboard',
  imports: [CommonModule, FormsModule, Navigation, RouterLink],
  templateUrl: './tester-dashboard.html',
  styleUrl: './tester-dashboard.css'
})
export class TesterDashboard implements OnInit {
  stats: DashboardStats | null = null;
  isLoading = true;
  errorMessage = '';

  showDownloadUI = false;
  filename = '';
  isDownloading = false;
  downloadError = '';
  successMessage = '';

  constructor(
    private statisticsService: StatisticsService,
    private testerService: TesterService,
    private codeFileService: CodeFileService
  ) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.statisticsService.getDashboardStats('TESTER').subscribe({
      next: (stats) => {
        this.stats = stats;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tester dashboard statistics:', error);
        this.errorMessage = 'Failed to load dashboard statistics';
        this.isLoading = false;
      }
    });
  }

  toggleDownloadUI(): void {
    this.showDownloadUI = !this.showDownloadUI;
    this.filename = '';
    this.downloadError = '';
    this.successMessage = '';
  }

  downloadCodeFile(): void {
    if (!this.filename) {
      this.downloadError = 'Please enter a filename.';
      this.successMessage = '';
      return;
    }
    this.isDownloading = true;
    this.downloadError = '';
    this.codeFileService.downloadFile(this.filename).subscribe({
      next: (blob) => {
        this.isDownloading = false;
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = this.filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this.successMessage = 'File downloaded successfully.';
        
        //this.toggleDownloadUI();
      },
      error: (error) => {
        console.error('Downloadfile error:', error);
        this.isDownloading = false;
        this.downloadError = 'Failed to download file. Please check the filename and try again.';
        this.successMessage = '';
      }
    });
  }
}
