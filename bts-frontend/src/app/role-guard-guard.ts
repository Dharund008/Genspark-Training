
import { Injectable } from '@angular/core';
import { CanActivate,ActivatedRouteSnapshot, Router} from '@angular/router';
import { AuthService } from './services/AuthService';
import { Observable, of } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

   canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const expectedRole = route.data['expectedRole'];
    
    return this.authService.currentUser$.pipe(
      take(1),
      map(user => {
        if (user && user.role === expectedRole) {
          return true;
        } else {
          // Redirect based on current user role
          if (user) {
            switch (user.role) {
              case 'ADMIN':
                this.router.navigate(['/admin/dashboard']);
                break;
              case 'DEVELOPER':
                this.router.navigate(['/developer/dashboard']);
                break;
              case 'TESTER':
                this.router.navigate(['/tester/dashboard']);
                break;
              default:
                this.router.navigate(['/dashboard']);
            }
          } else {
            this.router.navigate(['/login']);
          }
          return false;
        }
      })
    );
  }
}