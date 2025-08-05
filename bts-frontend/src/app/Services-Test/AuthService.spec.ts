import { TestBed } from '@angular/core/testing';
import { AuthService } from '../services/AuthService';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { LoginRequest, RegisterRequest, AuthResponse, ForgotPasswordDTO, ResetPasswordDTO } from '../models/AuthModel';
import { User } from '../models/UserModel';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);

  });

  afterEach(() => {
    localStorage.clear();
    httpMock.verify();
  });

  it('should login and update user state', () => {
    const loginData: LoginRequest = { Username: 'testUser@gmail.com', password: 'pass123' };
    const mockResponse: AuthResponse = {
      token: 'mock-jwt-token',
      username: 'testUser',
      role: 'admin'
    };

    service.login(loginData).subscribe(response => {
      expect(response.token).toBe('mock-jwt-token');
    });

    const loginReq = httpMock.expectOne('http://localhost:5088/api/Authentication/login');
    expect(loginReq.request.method).toBe('POST');
    loginReq.flush(mockResponse);

    expect(localStorage.getItem('authToken')).toBe('mock-jwt-token');
    expect(service.isAuthenticated()).toBeTrue();

    const userDetailsReq = httpMock.expectOne('http://localhost:5088/api/Authentication/my-details');
    userDetailsReq.flush({
      id: '123',
      username: 'testUser@gmail.com',
      password: '',
      role: 'admin'
    });
  });

//   it('should register a user', () => {
//     const registerData: RegisterRequest = {
//       username: 'newUser',
//       email: 'test@example.com',
//       password: 'newpass'
//     };

//     service.register(registerData).subscribe(response => {
//       expect(response).toEqual({ success: true });
//     });

//     const req = httpMock.expectOne('http://localhost:5088/api/Authentication/register');
//     expect(req.request.method).toBe('POST');
//     req.flush({ success: true });
//   });

  it('should handle forgot password', () => {
    const forgotData: ForgotPasswordDTO = { email: 'test@example.com' };

    service.forgotPassword(forgotData).subscribe(response => {
      expect(response).toEqual({ sent: true });
    });

    const req = httpMock.expectOne('http://localhost:5088/api/User/forgot-password');
    expect(req.request.method).toBe('POST');
    req.flush({ sent: true });
  });

  it('should reset password', () => {
    const resetData: ResetPasswordDTO = {
      email: 'test@example.com',
      token: 'reset-token',
      newPassword: 'newpass'
    };

    service.resetPassword(resetData).subscribe(response => {
      expect(typeof response).toBe('string'); // API returns text
    });

    const req = httpMock.expectOne('http://localhost:5088/api/User/reset-password');
    expect(req.request.method).toBe('POST');
    req.flush('Password reset successful');
  });

  it('should store and retrieve token', () => {
    service.setToken('dummy-token');
    expect(service.getToken()).toBe('dummy-token');
    expect(service.isAuthenticated()).toBeTrue();

    service.clearToken();
    expect(service.getToken()).toBeNull();
    expect(service.isAuthenticated()).toBeFalse();
  });

  it('should return user details', () => {
    const mockUser: User = {
      id: '1',
      username: 'testUser',
      password: '',
      role: 'admin'
    };

    spyOn(service, 'getToken').and.returnValue('mock-jwt-token');

    service.getMyDetails().subscribe(user => {
      expect(user.username).toBe('testUser');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Authentication/my-details');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-jwt-token');
    req.flush(mockUser);
  });
});
