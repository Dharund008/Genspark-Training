import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AdminService } from '../services/AdminService';
import { AuthService } from '../services/AuthService';
import { DeveloperRequestDTO, TesterRequestDTO, Developer, Tester, User } from '../models/UserModel';
import { Bug } from '../models/bug.model';

describe('AdminService', () => {
  let service: AdminService;
  let httpMock: HttpTestingController;
  let mockAuthService: Partial<AuthService>;

  beforeEach(() => {
    mockAuthService = {
      getToken: () => 'mock-token'
    };

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AdminService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(AdminService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Make sure all requests have been flushed
  });

  it('should retrieve all users', () => {
    const mockUsers: User[] = [{ id: '1', username: 'test@gmail.com', password: '', role: 'ADMIN' }];

    service.getAllUsers().subscribe(users => {
      expect(users.length).toBe(1);
      expect(users[0].username).toBe('test@gmail.com');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Admin/list-users');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
    req.flush(mockUsers);
  });

  it('should create a developer', () => {
    const dev: DeveloperRequestDTO = {
        name: 'Dev1', email: 'dev@example.com',
        password: ''
    };

    service.createDeveloper(dev).subscribe(response => {
      expect(response).toEqual({ success: true });
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Admin/add-developer');
    expect(req.request.method).toBe('POST');
    req.flush({ success: true });
  });

  it('should delete a tester', () => {
    service.deleteTester('123').subscribe(response => {
      expect(response).toEqual({ deleted: true });
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Admin/delete-tester/' && r.params.has('testerId')
    );
    expect(req.request.method).toBe('DELETE');
    req.flush({ deleted: true });
  });

  it('should check if user exists', () => {
    service.checkUserExists('test@example.com').subscribe(exists => {
      expect(exists).toBeTrue();
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Admin/check-user/test@example.com');
    expect(req.request.method).toBe('GET');
    req.flush(true);
  });
});
