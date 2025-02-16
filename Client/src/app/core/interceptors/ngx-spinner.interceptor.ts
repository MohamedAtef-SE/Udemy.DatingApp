import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { delay, finalize } from 'rxjs';

export const ngxSpinnerInterceptor: HttpInterceptorFn = (req, next) => {

const _SpinnerService = inject(NgxSpinnerService);
_SpinnerService.show();

  return next(req).pipe(
    delay(1000),
    finalize( ()=>_SpinnerService.hide())
  );
};
