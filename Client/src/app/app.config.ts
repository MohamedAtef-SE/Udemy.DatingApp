import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations} from '@angular/platform-browser/animations'

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { errorsInterceptor } from './core/interceptors/errors.interceptor';
import { headerInterceptor } from './core/interceptors/header.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ngxSpinnerInterceptor } from './core/interceptors/ngx-spinner.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch(),withInterceptors([errorsInterceptor,headerInterceptor,ngxSpinnerInterceptor])),
    provideRouter(routes),
    provideAnimations(),
    provideToastr({positionClass: 'toast-bottom-right'}), // Toastr providers
    importProvidersFrom(NgxSpinnerModule)
  ]
};
