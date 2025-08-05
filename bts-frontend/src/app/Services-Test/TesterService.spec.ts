import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TesterService } from '../services/TesterService';
import { AuthService } from '../services/AuthService';
import { Bug, BugSubmissionDTO, UpdateBugPatchDTO } from '../models/bug.model';
import { Tester } from '../models/UserModel';

describe('TesterService', () => {
  let service: TesterService;
  let httpMock: HttpTestingController;

  const mockAuthService = {
    getToken: () => 'mock-token'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        TesterService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(TesterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create a bug', () => {
    const newBug: BugSubmissionDTO = {
      title: 'Bug title',
      description: 'Bug description',
      priority : 0
    };

    const mockResponse: Bug = { id: 1, title: 'Bug title' } as Bug;

    service.createBug(newBug).subscribe(bug => {
      expect(bug.title).toBe('Bug title');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Tester/create-bug');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should upload screenshot file', () => {
    const mockFile = new File(['image content'], 'screenshot.png', { type: 'image/png' });

    service.uploadScreenshot(mockFile).subscribe(response => {
      expect(response).toEqual('Upload successful');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Tester/upload-screenshot');
    expect(req.request.method).toBe('POST');
    expect(req.request.body instanceof FormData).toBeTrue();
    req.flush('Upload successful');
  });

  it('should update bug details', () => {
    const updates: UpdateBugPatchDTO = { description: 'Updated bug info' , priority: 1};

    const mockBug: Bug = { id: 99, title: 'Updated bug title' } as Bug;

    service.updateBugDetails(99, updates).subscribe(bug => {
      expect(bug.id).toBe(99);
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Tester/update-bug-details' &&
      r.params.get('bugId') === '99'
    );
    expect(req.request.method).toBe('PATCH');
    req.flush(mockBug);
  });

//   it('should update bug status', () => {
//     service.updateBugStatus(42, 1).subscribe(response => {
//       expect(response.message).toBe('Status updated');
//     });

//     const req = httpMock.expectOne('http://localhost:5088/api/Tester/update-bug-status/42');
//     expect(req.request.method).toBe('PUT');
//     expect(req.request.params.get('newStatus')).toBe('1');
//     req.flush({ message: 'Status updated' });
//   });

  it('should fetch my bugs', () => {
    const mockBugs: Bug[] = [{ id: 10, title: 'Bug A' } as Bug];

    service.getMyBugs().subscribe(bugs => {
      expect(bugs.length).toBe(1);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Tester/my-bugs');
    expect(req.request.method).toBe('GET');
    req.flush(mockBugs);
  });

  it('should fetch tester by email', () => {
    const mockTester: Tester = { id: 't01', email: 'tester@example.com' } as Tester;

    service.getTesterByEmail('tester@example.com').subscribe(tester => {
      expect(tester.email).toBe('tester@example.com');
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Tester/tester-by-email' &&
      r.params.get('email') === 'tester@example.com'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockTester);
  });

  it('should fetch all testers', () => {
    const mockTesters: Tester[] = [{ id: 't01', email: 'tester@example.com' } as Tester];

    service.getAllTesters().subscribe(testers => {
      expect(testers.length).toBeGreaterThan(0);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Tester/all-testers');
    expect(req.request.method).toBe('GET');
    req.flush(mockTesters);
  });

  it('should fetch testers associated with bugs', () => {
    const mockTesters: Tester[] = [{ id: 't02', email: 'linked@example.com' } as Tester];

    service.getTestersWithBugs().subscribe(testers => {
      expect(testers[0].email).toBe('linked@example.com');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Tester/testers-associated-with-bugs');
    expect(req.request.method).toBe('GET');
    req.flush(mockTesters);
  });
});
