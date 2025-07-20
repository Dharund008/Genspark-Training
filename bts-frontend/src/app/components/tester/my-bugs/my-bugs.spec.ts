import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyBugs } from './my-bugs';

describe('MyBugs', () => {
  let component: MyBugs;
  let fixture: ComponentFixture<MyBugs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyBugs]
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
