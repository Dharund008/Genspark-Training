import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Navigation } from '../../../shared/navigation/navigation';
import { CodeFileService } from '../../../services/CodeFile.service';
import { UploadedFile} from '../../../models/FileModel';
import { AuthService } from '../../../services/AuthService';


@Component({
  selector: 'app-code-files',
  imports: [CommonModule, FormsModule, Navigation],
  templateUrl: './code-files.html',
  styleUrl: './code-files.css'
})
export class CodeFiles implements OnInit {
  files: UploadedFile[] = [];
  developerFileLogs: UploadedFile[] = [];
  selectedFile: File | null = null;
  isLoading = true;
  isUploading = false;
  errorMessage = '';
  successMessage = '';
  currentPage = 1;
  pageSize = 5;
  totalFiles = 0;
  currentUserId: string | null = null;
  public Math = Math;


  constructor(
    private codeFileService: CodeFileService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.getMyDetails().subscribe({
      next: (user) => {
        this.currentUserId = user?.id || null;
        if (this.currentUserId) {
          this.loadDeveloperFileLogs(this.currentUserId);
        }
      },
      error: (error) => {
        console.error('Failed to get user details:', error);
      }
    });
    this.loadFiles();
  }

  loadFiles(): void {
  this.isLoading = true;
  this.codeFileService.getAllCodeFiles(this.currentPage, this.pageSize).subscribe({
    next: (response: any) => {
      this.files = Array.isArray(response?.uploads?.$values) ? response.uploads.$values : [];
      this.totalFiles = response.total || 0;
      this.isLoading = false;
    },
    error: (error) => {
      this.errorMessage = 'Failed to load files';
      this.isLoading = false;
      console.error('Error loading files:', error);
    }
  });
}


  loadDeveloperFileLogs(developerId: string): void {
  this.codeFileService.getFileLogsByDeveloper(developerId).subscribe({
    next: (response: any) => {
      this.developerFileLogs = Array.isArray(response?.$values) ? response.$values : [];
    },
    error: (error) => {
      console.error('Failed to load developer file logs:', error);
    }
  });
}


  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file to upload';
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.codeFileService.uploadCodeFile(this.selectedFile).subscribe({
      next: (response) => {
        console.log('File uploaded successfully:', response);
        this.successMessage = 'File uploaded successfully';
        this.selectedFile = null;
        this.loadFiles();
        this.isUploading = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to upload file';
        this.isUploading = false;
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  nextPage(): void {
    if (this.currentPage * this.pageSize < this.totalFiles) {
      this.currentPage++;
      this.loadFiles();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadFiles();
    }
  }

  get hasNextPage(): boolean {
    return this.currentPage * this.pageSize < this.totalFiles;
  }

  get hasPreviousPage(): boolean {
    return this.currentPage > 1;
  }
}


    
