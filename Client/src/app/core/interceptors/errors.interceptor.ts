import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';


export const errorsInterceptor: HttpInterceptorFn = (req, next) => {

  const _ToastrService = inject(ToastrService);
  const _Router = inject(Router);

  return next(req).pipe(
    catchError((httpErrorResponse)=>{
      if(httpErrorResponse){
        switch(httpErrorResponse.status){
          case 400:
            if(httpErrorResponse.error.errors){
              const modalSatateErrors= [];
              for(const key in httpErrorResponse.error.errors){
                if(httpErrorResponse.error.errors[key]){
                  modalSatateErrors.push(httpErrorResponse.error.errors[key]) // pushed Array of string with each iteration for each key
                }
              }
              throw modalSatateErrors.flat<string[],1>();
            }
            else{
              console.log(httpErrorResponse)
              _ToastrService.error(httpErrorResponse.error.message,httpErrorResponse.status)
            }
              break;
            case 401:
              _ToastrService.error("Unauthorized",httpErrorResponse.status)
              break;
            case 404:
              _Router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: httpErrorResponse.error}};
              _Router.navigateByUrl('/server-error',navigationExtras);
              break;
            default:
              _ToastrService.error('Something unexpected went wrong');
              break;
        }
      }
      throw httpErrorResponse;
    })
  );

};
