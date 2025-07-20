import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  feedbackForm: FormGroup;
  contactForm: FormGroup;
  isSubmittingFeedback = false;
  feedbackSubmitted = false;

  constructor(private fb: FormBuilder) {
    this.feedbackForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      message: ['', [Validators.required, Validators.minLength(10)]]
    });

    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      subject: ['', Validators.required],
      message: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  ngOnInit(): void {}

  onFeedbackSubmit(): void {
    if (this.feedbackForm.valid) {
      this.isSubmittingFeedback = true;
      
      setTimeout(() => {
        this.isSubmittingFeedback = false;
        this.feedbackSubmitted = true;
        this.feedbackForm.reset();
        
        setTimeout(() => {
          this.feedbackSubmitted = false;
        }, 3000);
      }, 1500);
    }
  }

  onContactSubmit(): void {
    if (this.contactForm.valid) {
      console.log('Contact form submitted:', this.contactForm.value);
      this.contactForm.reset();
    }
  }

  scrollToSection(sectionId: string): void {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }
}