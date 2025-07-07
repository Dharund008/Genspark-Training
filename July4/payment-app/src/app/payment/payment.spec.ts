import { ComponentFixture, TestBed } from '@angular/core/testing';
 
import { Payment } from './payment';
 
describe('Payment', () => {
  let component: Payment;
  let fixture: ComponentFixture<Payment>;
 
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Payment]
    })
    .compileComponents();
 
    fixture = TestBed.createComponent(Payment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });
 
  it('should NOT open Razorpay if form is invalid', () => {
    const mockOpen = jasmine.createSpy('open');
    const mockRazorpay = jasmine.createSpy().and.returnValue({ open: mockOpen });
    component.form.setValue({
      amount: '',
      name: '',
      email: '',
      contact: ''
    });
    component.pay();
    expect(mockRazorpay).not.toHaveBeenCalled();
    expect(mockOpen).not.toHaveBeenCalled();
  });
  
  it('should call Razorpay.open if form is valid', () => {
    const mockOpen = jasmine.createSpy('open');
    (window as any).Razorpay = function () {
      this.open = mockOpen;
    };
 
    component.form.setValue({
      amount: 2,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890'
    });
 
    component.pay();
    setTimeout(() => {
      expect(mockOpen).toHaveBeenCalled();
    }, 350);
  });

  it('should set paymentStatus to success on handler', () => {
    let handlerFn: any;
    (window as any).Razorpay = function (options: any) {
      handlerFn = options.handler;
      this.open = jasmine.createSpy('open');
    };

    component.form.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890'
    });

    component.pay();
    handlerFn({ razorpay_payment_id: 'pay_123' });
    expect(component.paymentStatus).toContain('Success! Payment ID: pay_123');
  });

    it('should set paymentStatus to cancelled on modal dismiss', () => {
    let modalObj: any;
    (window as any).Razorpay = function (options: any) {
      modalObj = options.modal;
      this.open = jasmine.createSpy('open');
    };

    component.form.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890'
    });

    component.pay();
    modalObj.ondismiss();
    expect(component.paymentStatus).toContain('Payment Cancelled');
  });

  it('should have invalid form when empty', () => {
    expect(component.form.valid).toBeFalse();
  });

  it('should have valid form with correct values', () => {
    component.form.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890'
    });
    expect(component.form.valid).toBeTrue();
  });

  it('should show error if contact is not 10 digits', () => {
    component.form.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '12345'
    });
    expect(component.form.get('contact')?.valid).toBeFalse();
    expect(component.form.valid).toBeFalse();
  });
 
});
 
 