
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './services/AuthService';
import { Observable, of } from 'rxjs';
import { map, take, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

    canActivate(): Observable<boolean> {
      return this.authService.currentUser$.pipe(
        take(1),
        switchMap(user => {
          if (user || this.authService.isAuthenticated()) {
            return of(true);
          } else {
            this.router.navigate(['/login']);
            return of(false);
          }
        })
      );
  }
}
