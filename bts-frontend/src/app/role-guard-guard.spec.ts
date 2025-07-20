import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { RoleGuard } from './role-guard-guard';

describe('RoleGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => {
      const authService = TestBed.inject<any>(Object.getPrototypeOf({ provide: 'AuthService' }).constructor);
      const router = TestBed.inject<any>(Object.getPrototypeOf({ provide: 'Router' }).constructor);
      return new RoleGuard(authService, router)['canActivate'](guardParameters[0]);
    });

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: 'AuthService', useValue: {} },
        { provide: 'Router', useValue: {} }
      ]
    });
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
