import { Routes } from '@angular/router';
import { VideoList } from '../app/video-list/video-list';
import { UploadVideo } from '../app/upload-video/upload-video';

export const routes: Routes = [
  { path: 'videos', component: VideoList },
  { path: 'upload', component: UploadVideo },
  { path: '', redirectTo: '/videos', pathMatch: 'full' },
];