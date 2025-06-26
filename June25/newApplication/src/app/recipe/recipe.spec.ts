import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Recipe } from './recipe';
import { RecipeService } from '../services/Recipe.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RecipeModel } from '../models/Recipe';

describe('Recipe', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;
  let mockService: jasmine.SpyObj<RecipeService>;

  beforeEach(async () => {
    mockService = jasmine.createSpyObj('RecipeService',['getallrecipes']);
    await TestBed.configureTestingModule({
      imports: [Recipe],
      providers:[
        {provide: RecipeService, useValue: mockService},
        provideHttpClient(), provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render recipe details correctly', () => {
    const mockRecipe: RecipeModel = {
      id: 1,
      name: 'Test Dish',
      cuisine: 'Italian',
      ingredients: ['Tomato', 'Basil'],
      rating: 4.5,
      cookTimeMinutes: 25,
      image: 'https://example.com/image.jpg'
    };

    component.recipe = mockRecipe;
    fixture.detectChanges(); 

    const compiled = fixture.nativeElement;

    expect(compiled.querySelector('h3')?.textContent).toContain('Test Dish');
    //expect(compiled.querySelector('.recipe-image')?.src).toContain('example.com/image.jpg');
    expect(compiled.textContent).toContain('Italian');
    expect(compiled.textContent).toContain('Tomato');
    expect(compiled.textContent).toContain('4.5'); //value mismatch : error
    expect(compiled.textContent).toContain('25 minutes');
  });

});
