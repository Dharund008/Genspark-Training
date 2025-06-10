import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-first',
  imports: [FormsModule],
  templateUrl: './first.html',
  styleUrl: './first.css'
})
export class First {

 //name: string = 'John Doe'; - default name string
  name:string;
  uname:string;
  like:boolean = false;
  clasName:string = "bi bi-balloon-heart";

  constructor()
  {
    this.name = "Rahul";
    this.uname = "John";
  }

  onButtonClick(hname:string) {
    alert(`Hello, ${hname}!`); 
  }// Show updated name in alert

  // onButtonClick(){
  //   alert(`Hello, ${this.uname}!`);
  // }

  toggleLike(){
    this.like = !this.like;
    if(this.like)
    {
      this.clasName = "bi bi-balloon-heart-fill";
    }
    else
    {
      this.clasName ="bi bi-balloon-heart";
    }
  }
  


}
