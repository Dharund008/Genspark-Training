import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CommentService } from '../services/Comments.service';
import { AuthService } from '../services/AuthService';
import { Comment, CommentRequestDTO } from '../models/Comments.model';

describe('CommentService', () => {
  let service: CommentService;
  let httpMock: HttpTestingController;

  const mockAuthService = {
    getToken: () => 'mock-token'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CommentService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(CommentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should add a comment', () => {
    const request: CommentRequestDTO = {
      bugId: 123,
      message: 'This bug is tricky'
    };

    const mockResponse: Comment = {
      id: 1,
      bugId: 123,
      message: 'This bug is tricky',
      userId: 'TES_qwe234',
      createdAt: '2025-07-20T14:30:00Z'
    };

    service.addComment(request).subscribe(comment => {
      expect(comment.message).toBe('This bug is tricky');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Comment/add-comment');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
    req.flush(mockResponse);
  });

  it('should get comments for a specific bug', () => {
    const mockComments: Comment[] = [
      {
        id: 1,
        bugId: 101,
        message: 'Investigated this issue',
        userId: 'TES_qwe234',
        createdAt: '2025-07-20T14:30:00Z'
      }
    ];

    service.getBugComments(101).subscribe(comments => {
      expect(comments.length).toBe(1);
      expect(comments[0].bugId).toBe(101);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Comment/bug/101');
    expect(req.request.method).toBe('GET');
    req.flush(mockComments);
  });

  it('should fetch all comments', () => {
    const mockComments: Comment[] = [
      {
        id: 1,
        bugId: 105,
        message: 'Looks good',
        userId: 'TES_qwe234',
        createdAt: '2025-07-20T14:30:00Z'
      }
    ];

    service.getAllComments().subscribe(comments => {
      expect(comments.length).toBe(1);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/Comment/comments-all');
    expect(req.request.method).toBe('GET');
    req.flush(mockComments);
  });

  it('should fetch comments by role', () => {
    const mockComments: Comment[] = [
      {
        id: 4,
        bugId: 99,
        message: 'This one was user-reported',
        userId: 'TES_qwe234',
        createdAt: '2025-07-20T14:30:00Z'
      }
    ];

    service.getCommentsByRole('tester').subscribe(comments => {
      expect(comments[0].userId).toBe('TES_qwe234');
    });

    const req = httpMock.expectOne(r =>
      r.url === 'http://localhost:5088/api/Comment/user-role' &&
      r.params.get('userRole') === 'tester'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockComments);
  });
});
