import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BugDetails } from './bug-details';

describe('BugDetails', () => {
  let component: BugDetails;
  let fixture: ComponentFixture<BugDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BugDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BugDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
