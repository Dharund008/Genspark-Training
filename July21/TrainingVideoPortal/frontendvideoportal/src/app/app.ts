import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { VideoList } from "./video-list/video-list";
import { UploadVideo } from "./upload-video/upload-video";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, VideoList, UploadVideo],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'frontendvideoportal';
}
