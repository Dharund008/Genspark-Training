import { TestBed } from '@angular/core/testing';
import { CodeFileService } from '../services/CodeFile.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../services/AuthService';
import { UploadedFile } from '../models/FileModel';

describe('CodeFileService', () => {
  let service: CodeFileService;
  let httpMock: HttpTestingController;

  const mockAuthService = {
    getToken: () => 'mock-token'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CodeFileService,
        { provide: AuthService, useValue: mockAuthService }
      ]
    });

    service = TestBed.inject(CodeFileService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

//   it('should upload a code file', () => {
//     const mockFile = new File(['test content'], 'test.js', { type: 'application/javascript' });

//     service.uploadCodeFile(mockFile).subscribe(response => {
//       expect(response.message).toBe('Upload successful');
//     });

//     const req = httpMock.expectOne('http://localhost:5088/api/CodeFile/upload');
//     expect(req.request.method).toBe('POST');
//     expect(req.request.body instanceof FormData).toBeTrue();
//     expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
//     req.flush(mockFile);
//   });

  it('should get all code files with pagination', () => {
    service.getAllCodeFiles(2, 10).subscribe(response => {
      expect(response).toEqual({ files: ['file1.js', 'file2.ts'] });
    });

    const req = httpMock.expectOne('http://localhost:5088/api/CodeFile/get-all-code-files?page=2&pageSize=10');
    expect(req.request.method).toBe('GET');
    req.flush({ files: ['file1.js', 'file2.ts'] });
  });

  it('should get file logs by developer', () => {
    const mockLogs: UploadedFile[] = [
  {
    id: 2,
    fileName: 'dev-bugfix.ts',
    filePath: '/uploads/dev-bugfix.ts',
    uploadedAt: new Date('2025-07-20T14:30:00Z'),
    developerId: 'dev001'
  }
];

    service.getFileLogsByDeveloper('dev001').subscribe(files => {
      expect(files.length).toBe(1);
      expect(files[0].fileName).toBe('dev-bugfix.ts');
    });

    const req = httpMock.expectOne('http://localhost:5088/api/CodeFile/filter-developers-filelogs?developerId=dev001');
    expect(req.request.method).toBe('GET');
    req.flush(mockLogs);
  });

  it('should download file as blob', () => {
    const mockBlob = new Blob(['file content'], { type: 'application/octet-stream' });

    service.downloadFile('test.js').subscribe(blob => {
      expect(blob instanceof Blob).toBeTrue();
      expect(blob.size).toBeGreaterThan(0);
    });

    const req = httpMock.expectOne('http://localhost:5088/api/CodeFile/downloadfile?filename=test.js');
    expect(req.request.method).toBe('GET');
    expect(req.request.responseType).toBe('blob');
    req.flush(mockBlob);
  });
});
