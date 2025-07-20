import { Routes } from '@angular/router';
import { AuthGuard } from './auth-guard';
import { RoleGuard } from './role-guard-guard';

export const routes: Routes = [

  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { 
    path: 'home', 
    loadComponent: () => import('./home/home').then(m => m.HomeComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login').then(m => m.LoginComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./forgot-password/forgot-password').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./dashboard/dashboard/dashboard').then(m => m.DashboardComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: { expectedRole: 'ADMIN' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/admin/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard)
      },
      {
        path: 'users',
        loadComponent: () => import('./components/admin/user-management/user-management').then(m => m.UserManagement)
      },
      {
        path: 'bugs',
        loadComponent: () => import('./components/admin//bug-management/bug-management').then(m => m.BugManagement)
      }
    ]
  },
  {
    path: 'developer',
    canActivate: [AuthGuard],
    data: { expectedRole: 'DEVELOPER' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/developer/developer-dashboard/developer-dashboard').then(m => m.DeveloperDashboard)
      },
      {
        path: 'bugs',
        loadComponent: () => import('./components/developer/assigned-bugs/assigned-bugs').then(m => m.AssignedBugs)
      },
      {
        path: 'files',
        loadComponent: () => import('./components/developer/code-files/code-files').then(m => m.CodeFiles)
      }
    ]
  },
  {
    path: 'tester',
    canActivate: [AuthGuard],
    data: { expectedRole: 'TESTER' },
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/tester/tester-dashboard/tester-dashboard').then(m => m.TesterDashboard)
      },
      {
        path: 'bugs',
        loadComponent: () => import('./components/tester/my-bugs/my-bugs').then(m => m.MyBugs)
      },
      {
        path: 'create-bug',
        loadComponent: () => import('./components/tester/create-bug/create-bug').then(m => m.CreateBug)
      }
    ]
  },
  {
    path: 'bug/:id',
    loadComponent: () => import('./shared/bug-details/bug-details').then(m => m.BugDetails),
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: '/home'
  }
];

