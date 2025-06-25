// export class RecipeModel{
//     constructor(public id:number=0, public name:string="", public cuisine:string="", public cookTimeMinutes:number=0, public ingredients:string[]=[], public image:string=""){

//     }
// }

export class RecipeModel {
  public id:number=0;
  image: string = '';
  name: string = '';
  cuisine: string = '';
  ingredients: string[] = [];
  rating: number = 0;
  cookTimeMinutes: number = 0;
}