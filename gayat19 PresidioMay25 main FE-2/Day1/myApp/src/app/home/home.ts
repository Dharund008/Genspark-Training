import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [ RouterOutlet],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
   uname:string="";
   router = inject(ActivatedRoute) //activateRoute is a curretroute

  ngOnInit(): void {
    console.log("init");
    this.uname = this.router.snapshot.params["un"] as string
  }



}