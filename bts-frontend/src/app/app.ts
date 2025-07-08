// import { Component } from '@angular/core';
// import { Login } from "./login/login";
// import { RouterOutlet } from '@angular/router';
// import { Signup } from "./signup/signup";

// @Component({
//   selector: 'app-root',
//   templateUrl: './app.html',
//   styleUrl: './app.css',
//   imports: [Login, RouterOutlet, Signup]
// })
// export class App {
//   protected title = 'myApp';
// }


import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css' 
})
export class App {
  title = 'BugSportz';
}