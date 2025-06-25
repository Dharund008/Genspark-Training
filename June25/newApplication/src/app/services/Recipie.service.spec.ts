import { TestBed } from '@angular/core/testing';
import { RecipeService } from './Recipe.service';
import { HttpClientTestingModule, HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('RecipeService',()=>{
    let service:RecipeService;
    let httpMock:HttpTestingController;

    beforeEach(()=>{
        TestBed.configureTestingModule({
            imports:[],
            providers:[RecipeService,provideHttpClient(),provideHttpClientTesting()]
        });
        service = TestBed.inject(RecipeService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(()=>{
        httpMock.verify();
    }) //verifying after injection,config

    it('GetRecipe : should retrive recipies from API',()=> 
    {
        const recipes = {
            "id":1,
            "name":"Fried loaded Chicken",
            "ingredients":["chicken,fries"],
            "instructions":["Cook well!"],
            "prepTimeMinutes":20,
            "cookTimeMinutes":15,
            "servings":2,
            "difficulty":"Easy",
            "cuisine":"Western",
            "caloriesPerServing":300,
            "tags":["chicken","Fries"],
            "userId":888,
            "image":"https://cdn.dummyjson.com/recipe-images/1.webp",
            "rating":9.9,
            "reviewCount":999,
            "mealType":["Lunch"]
        };

        service.getRecipe(1).subscribe(recipe=>{
            //expect(recipe).toEqual(recipe);
            expect(recipe).toBe(recipes);
        })
        const req = httpMock.expectOne('https://dummyjson.com/recipes/1');
        expect(req.request.method).toBe('GET');
        req.flush(recipes); // flush
    })

    it('should handle error when getRecipe fails', () => {
    service.getRecipe(1).subscribe({
        next: () => fail('should have failed with 500 error'),
        error: error => {
        expect(error.status).toBe(500);
        }
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipes/1');
    req.flush('Internal Server Error', { status: 500, statusText: 'Server Error' });
    });


    it('GetAllRecipe : should retrive recipies from API',()=> 
    {
        const mockResponse = 
        {
          "recipes": [
            {
              "id": 1,
              "name": "Fried loaded Chicken",
              "ingredients": ["chicken,fries"],
              "instructions": ["Cook well!"],
              "prepTimeMinutes": 20,
              "cookTimeMinutes": 15,
              "servings": 2,
              "difficulty": "Easy",
              "cuisine": "Western",
              "caloriesPerServing": 300,
              "tags": ["chicken", "Fries"],
              "userId": 888,
              "image": "https://cdn.dummyjson.com/recipe-images/1.webp",
              "rating": 9.9,
              "reviewCount": 999,
              "mealType": ["Lunch"]
            }
          ]
        };
        service.getallRecipes().subscribe(response=>{
            expect(response).toEqual(mockResponse.recipes);
        })
        const req = httpMock.expectOne('https://dummyjson.com/recipes');
        expect(req.request.method).toBe('GET');
        req.flush(mockResponse.recipes); // flush
    })



})