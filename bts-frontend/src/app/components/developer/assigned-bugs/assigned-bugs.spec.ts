import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { AssignedBugs } from './assigned-bugs';

describe('AssignedBugs', () => {
  let component: AssignedBugs;
  let fixture: ComponentFixture<AssignedBugs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignedBugs, HttpClientTestingModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: { paramMap: { get: () => null } }
          }
        }
      ]
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
