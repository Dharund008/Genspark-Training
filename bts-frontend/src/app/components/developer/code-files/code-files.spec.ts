import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeFiles } from './code-files';

describe('CodeFiles', () => {
  let component: CodeFiles;
  let fixture: ComponentFixture<CodeFiles>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CodeFiles]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CodeFiles);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
