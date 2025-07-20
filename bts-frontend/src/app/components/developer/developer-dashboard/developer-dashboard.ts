import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { StatisticsService } from '../../../services/Statistics.service';
import { DeveloperService } from '../../../services/DeveloperService';
import { CodeFileService } from '../../../services/CodeFile.service';
import { DashboardStats } from '../../../models/Staticstics.model';

@Component({
  selector: 'app-developer-dashboard',
  imports: [CommonModule, Navigation, RouterLink],
  templateUrl: './developer-dashboard.html',
  styleUrl: './developer-dashboard.css'
})
export class DeveloperDashboard implements OnInit {
  stats: DashboardStats | null = null;
  isLoading = true;
  errorMessage = '';
  showUploadModal = false;
  selectedFile: File | null = null;
  uploading = false;
  uploadMessage = '';
  uploadStatus = '';

  constructor(
    private statisticsService: StatisticsService,
    private developerService: DeveloperService,
    private codeFileService: CodeFileService
  ) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.statisticsService.getDashboardStats('DEVELOPER').subscribe({
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


  // File Upload Methods
  openUploadModal() {
    this.showUploadModal = true;
  }
  closeUploadModal() {
    this.showUploadModal = false;
    this.selectedFile = null;
    this.uploadMessage = '';
    this.uploadStatus = '';
  }

  onFileSelect(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const target = event.currentTarget as HTMLElement;
    target.classList.add('drag-over');
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const target = event.currentTarget as HTMLElement;
    target.classList.remove('drag-over');
  }

  onFileDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const target = event.currentTarget as HTMLElement;
    target.classList.remove('drag-over');
    
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.selectedFile = files[0];
    }
  }

  uploadFile() {
    if (!this.selectedFile) return;

    this.uploading = true;
    this.uploadMessage = '';

    this.codeFileService.uploadCodeFile(this.selectedFile).subscribe({
      next: (response) => {
        console.log('Codefile Upload response:', response);
        this.uploadMessage =  response.message || 'File uploaded successfully!';
        this.uploadStatus = 'success';
        this.uploading = false;

        this.selectedFile = null;
      },
      error: (error) => {
        console.error('Codefile Upload error:', error);
        this.uploadMessage = 'Upload failed. Please try again.';
        this.uploadStatus = 'error';
        this.uploading = false;
      }
    });
  }

}

