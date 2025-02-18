import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { delay, finalize, identity } from 'rxjs';
import { environment } from '../../../environments/environment';

export const ngxSpinnerInterceptor: HttpInterceptorFn = (req, next) => {

const _SpinnerService = inject(NgxSpinnerService);
_SpinnerService.show();

  return next(req).pipe(
    (environment.production ? identity : delay(1000)),
    finalize( ()=>_SpinnerService.hide())
  );
};
