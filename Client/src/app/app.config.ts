import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations} from '@angular/platform-browser/animations'

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { errorsInterceptor } from './core/interceptors/errors.interceptor';
import { headerInterceptor } from './core/interceptors/header.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch(),withInterceptors([errorsInterceptor,headerInterceptor])),
    provideRouter(routes),
    provideAnimations(),
    provideToastr({positionClass: 'toast-bottom-right'}), // Toastr providers
  ]
};
