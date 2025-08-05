import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { DeveloperDashboard } from './developer-dashboard';

describe('DeveloperDashboard', () => {
  let component: DeveloperDashboard;
  let fixture: ComponentFixture<DeveloperDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeveloperDashboard, HttpClientTestingModule],
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

    fixture = TestBed.createComponent(DeveloperDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
