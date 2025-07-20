// import { HttpInterceptorFn } from '@angular/common/http';

// // export const authInterceptor: HttpInterceptorFn = (req, next) => {
// //   return next(req);
// // };
// import { Injectable } from '@angular/core';
// import {
//   HttpRequest,
//   HttpHandler,
//   HttpEvent,
//   HttpInterceptor
// } from '@angular/common/http';
// import { Observable } from 'rxjs';

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {

//   constructor() {}

//   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     const token = localStorage.getItem('token'); //token from localStorage.

//     if (token) {
//       const cloned = request.clone({
//         headers: request.headers.set('Authorization', `Bearer ${token}`)
//       });
//       return next.handle(cloned); 
//     }

//     return next.handle(request); // send original request if no token
//   }
// }
import { HttpInterceptorFn } from '@angular/common/http';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('authToken');
  
  if (token) {
    const cloned = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(cloned);
  }

  return next(req);
};
