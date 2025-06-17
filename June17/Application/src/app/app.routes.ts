import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { About } from './about/about';
import { LoginComponent } from './login/login';
import { ProductDetailComponent } from './productlist/productlist';
import { AuthGuard } from './auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'home', component: HomeComponent },  // Default child → product list
      { path: ':id', component: ProductDetailComponent }  // Displays details based on `id`
    ]
  },
  { path: 'about', component: About },
  { path: '',component: LoginComponent, pathMatch: 'full' }, // Redirect to login by default
  { path: '**', redirectTo: 'login' } // Wildcard route to catch unknown paths
];
