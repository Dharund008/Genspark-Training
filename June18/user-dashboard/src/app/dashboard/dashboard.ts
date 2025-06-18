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
  filteredUsers: User[] = []; // Stores filtered list
  // searchGender: string = "";
  searchText: string = "";

  constructor(private userService: UserService) {} // Injecting UserService to handle API calls

  ngOnInit() {
    this.userService.getUsers().subscribe({
      next: (data:any ) =>
      {
        console.log('Fetched Users:', data);
        this.Users = data.users as User[];
        this.filteredUsers = [...this.Users]; //initialising it so it would be filtered starting from the beginning
      },
      error: (error: any) =>
      {
        console.error('Error fetching users:', error);
      }
    });
  }

  filterUsers()
  {
      //this.searchGender = this.searchGender.toLowerCase();
      this.filteredUsers = this.Users.filter(user =>{
      // return user.gender.toLowerCase() == (this.searchGender);
      if (this.searchText.toLowerCase().includes(user.role.toLowerCase())){
        return true;
      }
      else if(this.searchText.toLowerCase().includes(user.address.state.toLowerCase()))
      {
        return true;
      }
      else if(this.searchText.toLowerCase() == (user.gender.toLowerCase()))
      {
        return true;
      }
      else
      {
        return false;
      }
    })
  }
 

  // addUser() {
  //   this.userService.addUser().subscribe(response => {
  //     console.log('User added:', response);
  //     this.users.push(response); // Update UI dynamically
  //   });
  // }
}
