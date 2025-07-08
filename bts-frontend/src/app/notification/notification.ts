import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-notification',
  templateUrl: './notification.html',
  imports: [CommonModule],
  styleUrls:['./notification.css']
})
export class NotificationComponent implements OnInit {

  constructor(public notifyService: NotificationService) {}

  ngOnInit(): void {
    this.notifyService.startConnection();
    console.log(this.notifyService.messages);
  }

  // send(): void {
  //   if (this.username && this.message) {
  //     this.notifyService.sendMessage(this.username, this.message);
  //     this.message = '';
  //   }
  // }
}
