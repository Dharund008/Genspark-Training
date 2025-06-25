import { Component, OnInit ,signal} from '@angular/core';
import { RecipeModel } from '../models/Recipe';
import { RecipeService } from '../services/Recipe.service';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipies',
  imports: [Recipe],
  templateUrl: './recipies.html',
  styleUrl: './recipies.css'
})
export class Recipies implements OnInit{
  recipes = signal<RecipeModel[]>([]);
  // recipes:RecipeModel[]|undefined=undefined;
  constructor(private recipeservice:RecipeService){

  }
  ngOnInit(): void {
    this.recipeservice.getallRecipes().subscribe(
      {
        next:(data:any)=>{
          console.log(data);
         this.recipes.set(data.recipes);
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
  }
}
