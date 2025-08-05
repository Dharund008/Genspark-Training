import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { MyBugs } from './my-bugs';

describe('MyBugs', () => {
  let component: MyBugs;
  let fixture: ComponentFixture<MyBugs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyBugs, HttpClientTestingModule],
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

    fixture = TestBed.createComponent(MyBugs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
