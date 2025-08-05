import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { TesterDashboard } from './tester-dashboard';

describe('TesterDashboard', () => {
  let component: TesterDashboard;
  let fixture: ComponentFixture<TesterDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TesterDashboard, HttpClientTestingModule],
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

    fixture = TestBed.createComponent(TesterDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
