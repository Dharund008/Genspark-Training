import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedBugs } from './assigned-bugs';

describe('AssignedBugs', () => {
  let component: AssignedBugs;
  let fixture: ComponentFixture<AssignedBugs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignedBugs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignedBugs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
