import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBug } from './create-bug';

describe('CreateBug', () => {
  let component: CreateBug;
  let fixture: ComponentFixture<CreateBug>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateBug]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateBug);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
