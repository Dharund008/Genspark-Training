import { Component, OnInit } from '@angular/core';
import { Video } from '../Model/TrainingVideo';
import { VideoService } from '../Service/VideoService';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './video-list.html',
  styleUrl: './video-list.css'
})
export class VideoList implements OnInit {
  videos: Video[] = [];

  constructor(private videoService: VideoService, private router: Router) {}

  ngOnInit(): void {
    console.log("ngonit called")
    this.videoService.getAll().subscribe((response: any) => {
      console.log("Videos fetched",response);
      this.videos = Array.isArray(response?.$values) ? response.$values : [];
    });
  }

  navigateToUpload(): void {
    this.router.navigate(['/upload']);
  }
}
