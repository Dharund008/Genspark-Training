export class RecipeModel {
  public id:number=0;
  image: string = '';
  name: string = '';
  cuisine: string = '';
  ingredients: string[] = [];
  rating: number = 0;
  cookTimeMinutes: number = 0;
}