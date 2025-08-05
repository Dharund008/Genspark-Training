import { TestBed } from '@angular/core/testing';
import { AuthGuard } from './auth-guard';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { AuthService } from './services/AuthService';
import { User } from './models/UserModel';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let router: Router;
  let route: ActivatedRouteSnapshot;
  let state: RouterStateSnapshot;
  let mockAuthService: Partial<AuthService>;

  beforeEach(() => {
    mockAuthService = {
      currentUser$: of(null),
      isAuthenticated: jasmine.createSpy().and.returnValue(false)
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    router = TestBed.inject(Router);
    route = {} as ActivatedRouteSnapshot;
    state = { url: '/protected' } as RouterStateSnapshot;
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should allow activation if token exists', (done) => {
    localStorage.setItem('token', 'fake-token');

    // Update AuthService behavior
    const mockUser: User = {
      id: 'TES_SWERF',
      username: 'test@gmail.com',
      password: 'fakepass',
      role: 'TESTER'
    };

    mockAuthService.currentUser$ = of(mockUser);

    //mockAuthService.currentUser$ = of({ username: 'test@gmail.com' });
    (mockAuthService.isAuthenticated as jasmine.Spy).and.returnValue(true);

    guard.canActivate().subscribe(result => {
      expect(result).toBeTrue();
      done();
    });
  });

  it('should block activation and redirect if token is missing', (done) => {
    spyOn(router, 'navigate');
    localStorage.removeItem('token');

    mockAuthService.currentUser$ = of(null);
    (mockAuthService.isAuthenticated as jasmine.Spy).and.returnValue(false);

    guard.canActivate().subscribe(result => {
      expect(result).toBeFalse();
      expect(router.navigate).toHaveBeenCalledWith(['/login']);
      done();
    });
  });
});
