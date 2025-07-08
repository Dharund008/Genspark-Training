// import { Routes } from '@angular/router';
// import { Login } from './login/login';
// import { Signup } from './signup/signup';
// import { NotificationComponent } from './notification/notification';
// import { AuthGuard } from './auth-guard';
// import { HomeComponent } from './home/home';
// // import { NotificationComponent } from './notification/notification';




// export const routes: Routes = [
//     { path: '', component: HomeComponent },
//   { path: 'about', component: HomeComponent }, // smooth scroll using fragment
//   { path: 'feedback', component: HomeComponent },
//     // {path:'login',component:Login},
//     // {path:'signup',component:Signup},
//     {path:'notificiation', component:NotificationComponent}
// ];


// import { Routes } from '@angular/router';
// import { ForgotPasswordComponent } from './forgot-password/forgot-password';

// export const routes: Routes = [
//   { path: '', redirectTo: '/home', pathMatch: 'full' },
//   { 
//     path: 'home', 
//     loadComponent: () => import('./home/home').then(m => m.HomeComponent)
//   },
//   { 
//     path: 'login', 
//     loadComponent: () => import('./login/login').then(m => m.LoginComponent)
//   },
//   { path: 'forgot-password', component: ForgotPasswordComponent },
//   { path: '**', redirectTo: '/home' }
// ];

import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { LoginComponent } from './login/login';
import { ForgotPasswordComponent } from './forgot-password/forgot-password';
// import { DashboardComponent } from './components/dashboard/dashboard';
// import { SummaryComponent } from './components/dashboard/summary/summary.component';
// import { ProfileComponent } from './components/dashboard/profile/profile.component';
import { AuthGuard } from './auth-guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  // { 
  //   path: 'dashboard', 
  //   component: DashboardComponent,
  //   canActivate: [AuthGuard],
  //   children: [
  //     { path: '', redirectTo: 'summary', pathMatch: 'full' },
  //     { path: 'summary', component: SummaryComponent },
  //     { path: 'profile', component: ProfileComponent },
  //     // Additional routes will be added here
  //   ]
  // },
  { path: '**', redirectTo: '' }
];
