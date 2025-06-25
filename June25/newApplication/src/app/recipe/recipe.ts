import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../services/Recipe.service';
import { inject, Injectable, Input } from "@angular/core";
import { RecipeModel } from '../models/Recipe';


@Component({
  selector: 'app-recipe',
  imports: [],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {
@Input() recipe:RecipeModel|null = new RecipeModel();

  private recipeservice = inject(RecipeService);
  constructor()
  {
    // this.recipeservice.getRecipe(1).subscribe(
    // {
    //   next:(data)=>{
    //     // console.log(data);
    //     this.recipe=data as RecipeModel;
    //     console.log(this.recipe);
    //   },
    //   error:(err)=>{
    //     console.log(err);
    //   },
    //   complete:()=>{
    //     console.log("All done")
    //   }
    // })
  }
}