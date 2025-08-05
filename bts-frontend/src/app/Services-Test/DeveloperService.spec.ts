import { TestBed } from '@angular/core/testing';
import { DeveloperService } from '../services/DeveloperService';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../services/AuthService';
import { Bug, BugStatus } from '../models/bug.model';

describe('DeveloperService', () => {
  let service: DeveloperService;
  let httpMock: HttpTestingController;

  const mockAuthService = {
    getToken: () => 'mock-token'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        DeveloperService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(DeveloperService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch assigned bugs', () => {
    const mockBugs: Bug[] = [{ id: 1, title: 'Login Issue' } as Bug];

    service.getAssignedBugs().subscribe(bugs => {
      expect(bugs.length).toBe(1);
      expect(bugs[0].title).toBe('Login Issue');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Developer/assigned-bugs');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
    req.flush(mockBugs);
  });

  it('should update bug status', () => {
    service.updateBugStatus(42, BugStatus.InProgress).subscribe(response => {
      expect(response).toEqual({ success: true });
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Developer/update-bug-status' &&
      r.params.get('bugId') === '42' &&
      r.params.get('newStatus') === BugStatus.InProgress.toString()
    );
    expect(req.request.method).toBe('PUT');
    req.flush({ success: true });
  });

  it('should fetch developer by email', () => {
    const mockDev = { id: 'dev123', email: 'dev@example.com' };

    service.getDeveloperByEmail('dev@example.com').subscribe(dev => {
      expect(dev.email).toBe('dev@example.com');
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Developer/developer-by-email' &&
      r.params.get('email') === 'dev@example.com'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockDev);
  });

  it('should fetch all developers', () => {
    const mockDevs = [{ id: 'dev001', email: 'a@example.com' }];

    service.getAllDevelopers().subscribe(devs => {
      expect(devs.length).toBeGreaterThan(0);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Developer/all-developers');
    expect(req.request.method).toBe('GET');
    req.flush(mockDevs);
  });

  it('should fetch testers for developer bugs', () => {
    const mockTesters = [{ id: 'test001', name: 'Tester One' }];

    service.getTestersForMyBugs().subscribe(testers => {
      expect(testers[0].name).toBe('Tester One');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Developer/my-bugs-testers');
    expect(req.request.method).toBe('GET');
    req.flush(mockTesters);
  });

  it('should fetch developers with bugs', () => {
    const mockList = [{ developer: 'dev001', bugs: 3 }];

    service.getDevelopersWithBugs().subscribe(devs => {
      expect(devs.length).toBe(1);
      expect(devs[0].bugs).toBe(3);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Developer/developer-with-bugs');
    expect(req.request.method).toBe('GET');
    req.flush(mockList);
  });
});
