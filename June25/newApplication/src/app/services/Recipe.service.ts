import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RecipeService
{
    private http = inject(HttpClient);

    getRecipe(id:number=1){
        return this.http.get('https://dummyjson.com/recipes/'+id);
    }
    getallRecipes():Observable<any[]>{
        return this.http.get<any[]>('https://dummyjson.com/recipes')
    }
}