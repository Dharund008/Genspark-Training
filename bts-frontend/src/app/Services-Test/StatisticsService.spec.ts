import { TestBed } from '@angular/core/testing';
import { StatisticsService } from '../services/Statistics.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../services/AuthService';
import { DashboardStats } from '../models/Staticstics.model';

describe('StatisticsService', () => {
  let service: StatisticsService;
  let httpMock: HttpTestingController;

  const mockAuthService = {
    getToken: () => 'mock-token'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        StatisticsService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(StatisticsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch dashboard stats for a given role', () => {
   const mockStats: DashboardStats = {
  totalBugs: 15,
  resolvedBugs: 10,
  bugsFixedByUser: 3,
  totalUsers: 4,
  openBugs: 2,
  totalBugsDeleted: 1,
  totalBugsClosed: 9
};


    service.getDashboardStats('admin').subscribe(stats => {
      expect(stats.totalBugs).toBe(15);
      expect(stats.resolvedBugs).toBe(10);
      expect(stats.totalUsers).toBe(4);
      expect(stats.openBugs).toBe(2);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Statistics/admin');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
    req.flush(mockStats);
  });
});
