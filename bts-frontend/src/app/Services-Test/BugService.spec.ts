import { TestBed } from '@angular/core/testing';
import { BugService } from '../services/BugService';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../services/AuthService';
import { Bug } from '../models/bug.model';

describe('BugService', () => {
  let service: BugService;
  let httpMock: HttpTestingController;
  let mockAuthService: Partial<AuthService>;

  beforeEach(() => {
    mockAuthService = {
      getToken: () => 'mock-token'
    };

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        BugService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(BugService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch all bugs', () => {
    const mockBugs: Bug[] = [{ id: 1, title: 'Bug 1' } as Bug];

    service.getAllBugs().subscribe(bugs => {
      expect(bugs.length).toBe(1);
      expect(bugs[0].title).toBe('Bug 1');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Bug');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
    req.flush(mockBugs);
  });

  it('should fetch bug by ID', () => {
    const mockBug: Bug[] = [{ id: 42, title: 'Bug Details' } as Bug];

    service.getBugById(42).subscribe(bugs => {
      expect(bugs[0].id).toBe(42);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Bug/bug-id/42');
    expect(req.request.method).toBe('GET');
    req.flush(mockBug);
  });

  it('should update bug status', () => {
    service.updateBugStatus(101, 2).subscribe(response => {
      expect(response.message).toBe('Status updated');
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Bug/update-bug-status/101' &&
      r.params.get('newStatus') === '2'
    );
    expect(req.request.method).toBe('PUT');
    req.flush({ message: 'Status updated' });
  });

  it('should fetch paginated bugs', () => {
    const mockBugs: Bug[] = [{ id: 1, title: 'Bug A' } as Bug];

    service.getPaginatedBugs(1, 7).subscribe(bugs => {
      expect(bugs.length).toBe(1);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Bug/paginated-bugsall?page=1&pageSize=7');
    expect(req.request.method).toBe('GET');
    req.flush(mockBugs);
  });

  it('should fetch assigned bugs', () => {
    const mockAssigned: Bug[] = [{ id: 5, title: 'Assigned Bug' } as Bug];

    service.getAssignedBugs().subscribe(bugs => {
      expect(bugs[0].title).toBe('Assigned Bug');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Bug/assigned-bugs');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssigned);
  });
});
