// import { CanActivateFn } from '@angular/router';

// export const authGuard: CanActivateFn = (route, state) => {
//   return true;
// };

import { Injectable } from '@angular/core';
import { UserService } from '../app/services/UserService';
import { ActivatedRouteSnapshot, CanActivate,  Router, RouterStateSnapshot } from '@angular/router';
@Injectable()
export class AuthGuard implements CanActivate{
  constructor(private userService: UserService,private router:Router){

  }
 canActivate(route:ActivatedRouteSnapshot, state:RouterStateSnapshot):boolean{
  //const isAuthenticated = localStorage.getItem("token")?true:false;
  if(!this.userService.isAuthenticated())
  {
    this.router.navigate(["login"]);
    return false;
  } 
  return true;
 }
  
}