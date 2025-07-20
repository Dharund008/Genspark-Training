import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient , withInterceptors} from '@angular/common/http';
import { AuthService } from './services/AuthService';
import { AuthGuard } from './auth-guard';
import { NotificationService } from './services/notification.service';
import { RoleGuard } from './role-guard-guard';
import { AuthInterceptor } from './Authentication/auth-interceptor';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptor])),
    AuthService,
    NotificationService,
    AuthGuard,
    RoleGuard
  ]
};
