import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BugManagement } from './bug-management';

describe('BugManagement', () => {
  let component: BugManagement;
  let fixture: ComponentFixture<BugManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BugManagement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BugManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
