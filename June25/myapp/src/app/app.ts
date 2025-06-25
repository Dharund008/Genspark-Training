import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Recipe } from "./recipe/recipe";
import { Recipies } from "./recipies/recipies";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Recipies],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'myapp';
}
