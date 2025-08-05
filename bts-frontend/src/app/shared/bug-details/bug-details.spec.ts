import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { BugDetails } from './bug-details';

describe('BugDetails', () => {
  let component: BugDetails;
  let fixture: ComponentFixture<BugDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BugDetails, HttpClientTestingModule],
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

    fixture = TestBed.createComponent(BugDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
