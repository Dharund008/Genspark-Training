import { Component, HostListener, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from "../models/UserModel";
import { UserService } from '../services/UserService';

@Component({
  selector: 'app-dashboard',
  imports: [FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  Users: User[] = []; //stors list of users.

  constructor(private userService: UserService) {} // Injecting UserService to handle API calls

  ngOnInit() {
    this.userService.getUsers().subscribe({
      next: (data:any ) =>
      {
        console.log('Fetched Users:', data);
        this.Users = data.users as User[];
      },
      error: (error: any) =>
      {
        console.error('Error fetching users:', error);
      }
    });
  }
 

  // addUser() {
  //   this.userService.addUser().subscribe(response => {
  //     console.log('User added:', response);
  //     this.users.push(response); // Update UI dynamically
  //   });
  // }
}
