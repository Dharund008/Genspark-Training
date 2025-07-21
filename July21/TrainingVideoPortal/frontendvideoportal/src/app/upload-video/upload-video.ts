import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { VideoService } from '../Service/VideoService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-upload-video',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './upload-video.html',
  styleUrl: './upload-video.css'
})
export class UploadVideo {
  form: FormGroup;
  uploading = false;
  error = '';

  constructor(
    fb: FormBuilder,
    private videoService: VideoService
  ) {
    this.form = fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      file: [null, Validators.required]
    });
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.form.patchValue({ file: input.files![0] });
    }
  }

  submit(): void {
    if (this.form.invalid) return;
    this.uploading = true;
    const fd = new FormData();
    fd.append('Title', this.form.value.title);
    fd.append('Description', this.form.value.description);
    fd.append('File', this.form.value.file);

    this.videoService.upload(fd).subscribe({
      next: () => window.location.replace('/videos'),
      error: err => {
        this.error = err.message;
        this.uploading = false;
      }
    });
  }
}
