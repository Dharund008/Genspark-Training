import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Recipies } from './recipies';
import { RecipeService } from '../services/Recipe.service';
import { of } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('Recipies', () => {
  let component: Recipies;
  let fixture: ComponentFixture<Recipies>;
  let mockService: jasmine.SpyObj<RecipeService>;

  beforeEach(async () => {

    const mockSpy = jasmine.createSpyObj('RecipeService', ['getallRecipes']);

    await TestBed.configureTestingModule({
      imports: [Recipies],
      providers: [
        { provide: RecipeService, useValue: mockSpy },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Recipies);
    component = fixture.componentInstance;
    //fixture.detectChanges();
    mockService = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should call getallRecipies and updating the component', () =>
  {
    const Data = {
      "recipes" : [
        { "id": 1, "name": "Recipe1", "ingredients": ["Ingredient1"] },
        { "id": 2, "name": "Recipe2", "ingredients": ["Ingredien2"] }
      ]
    };
    mockService.getallRecipes.and.returnValue(of(Data as any));
    fixture.detectChanges(); 

    expect(mockService.getallRecipes).toHaveBeenCalled();
    expect(component.recipes().length).toBe(2); //1 : error
    expect(component.recipes()[0].name).toBe("Recipe1");
  });

  it('should render the recipe list in DOM', () => {
    const Data = {
      recipes: [{ id: 1, name: 'Rendered!' }]
    };

    mockService.getallRecipes.and.returnValue(of(Data as any));
    fixture.detectChanges();

    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('app-recipe')).toBeTruthy();
  });


  it('should show "No Recipes Found!" when list is empty', () => {
    mockService.getallRecipes.and.returnValue(of({ recipes: [] })as any);
    fixture.detectChanges();

    const compiled = fixture.nativeElement;
    expect(compiled.textContent).toContain('No Recipes Found!');
  });
});
