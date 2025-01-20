import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';


export const errorsInterceptor: HttpInterceptorFn = (req, next) => {

  const _ToastrService = inject(ToastrService);
  const _Router = inject(Router);

  return next(req).pipe(
    catchError((HttpErrorResponse)=>{
      if(HttpErrorResponse){
        switch(HttpErrorResponse.status){
          case 400:
            if(HttpErrorResponse.error.errors){
              const modalSatateErrors= [];
              for(const key in HttpErrorResponse.error.errors){
                if(HttpErrorResponse.error.errors[key]){
                  modalSatateErrors.push(HttpErrorResponse.error.errors[key]) // pushed Array of string with each iteration for each key
                }
              }
              throw modalSatateErrors.flat<string[],1>();
            }
            else{
              _ToastrService.error(HttpErrorResponse.error.message,HttpErrorResponse.status)
            }
              break;
            case 401:
              _ToastrService.error("Unauthorized",HttpErrorResponse.status)
              break;
            case 404:
              _Router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: HttpErrorResponse.error}};
              _Router.navigateByUrl('/server-error',navigationExtras);
              break;
            default:
              _ToastrService.error('Something unexpected went wrong');
              break;
        }
      }
      throw HttpErrorResponse;
    })
  );

};
