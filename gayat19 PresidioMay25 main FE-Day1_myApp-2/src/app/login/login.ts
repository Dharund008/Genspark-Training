// import { Component } from '@angular/core';
// import { UserLoginModel } from '../models/UserLoginModel';
// import { UserService } from '../services/UserService';
// import { FormsModule } from '@angular/forms';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-login',
//   imports: [FormsModule],
//   templateUrl: './login.html',
//   styleUrl: './login.css'
// })
// export class Login {
// user:UserLoginModel = new UserLoginModel();
// constructor(private userService:UserService,private route:Router){

// }
// handleLogin(){
//   this.userService.validateUserLogin(this.user);
//   this.route.navigateByUrl("/home/"+this.user.username);
// }
// }

// import { Component } from '@angular/core';
// import { UserLoginModel } from '../models/UserLoginModel';
// import { UserService } from '../services/UserService';
// import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-login',
//   imports: [FormsModule,ReactiveFormsModule],
//   templateUrl: './login.html',
//   styleUrl: './login.css'
// })
// export class Login {
// user:UserLoginModel = new UserLoginModel();
// loginForm:FormGroup;

// constructor(private userService:UserService,private route:Router){
//   this.loginForm = new FormGroup({
//     un:new FormControl(null,Validators.required),
//     pass:new FormControl(null,[Validators.required])
//   })
// }

// public get un() : any {
//   return this.loginForm.get("un")
// }
// public get pass() : any {
//   return this.loginForm.get("pass")
// }

// // handleLogin(un:any,pass:any){
// //   console.log(un.control.touched)
// //   if(un.control.errors || pass.control.errors)
// //     return;

// //   this.userService.validateUserLogin(this.user);
// //   this.route.navigateByUrl("/home/"+this.user.username);
// // }
// handleLogin(){

//   if(this.loginForm.invalid)
//     return;

//   this.user.username = this.loginForm.value.un;
//   this.user.password = this.loginForm.value.pass;

//   this.userService.validateUserLogin(this.user);
//   this.route.navigateByUrl("/home/"+this.user.username);
// }
// }


//custom validator
import { Component } from '@angular/core';
import { UserLoginModel } from '../models/UserLoginModel';
import { UserService } from '../services/UserService';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { textValidator } from '../Misc/TextValidator';

// @Component({
//   selector: 'app-login',
//   imports: [FormsModule],
//   templateUrl: './login.html',
//   styleUrl: './login.css'
// })
// export class Login {
// user:UserLoginModel = new UserLoginModel();
// constructor(private userService:UserService,private route:Router){

// }
// handleLogin(un:any,pass:any){
//   console.log(un.control.touched)
//   if(un.control.errors || pass.control.errors)
//     return;

//   this.userService.validateUserLogin(this.user);
//   this.route.navigateByUrl("/home/"+this.user.username);
// }
// }

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user: UserLoginModel = new UserLoginModel();
  loginForm: FormGroup;

  constructor(private userService: UserService, private route: Router) {
    this.loginForm = new FormGroup({
      un: new FormControl(null, Validators.required),
      pass: new FormControl(null, [Validators.required, textValidator()])
    });
  }

  public get un(): any {
    return this.loginForm.get("un");
  }
  public get pass(): any {
    return this.loginForm.get("pass");
  }

  handleLogin() {
    if (this.loginForm.invalid)
      return;

    this.user.username = this.loginForm.value.un;
    this.user.password = this.loginForm.value.pass;

    this.userService.validateUserLogin(this.user);
    this.route.navigateByUrl("/home/" + this.user.username);
  }
}
