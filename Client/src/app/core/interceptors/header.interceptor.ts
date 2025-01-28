import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';

export const headerInterceptor: HttpInterceptorFn = (req, next) => {
  if(req.url.includes('account')){
    return next(req);
  }

  const _AccountService = inject(AccountService);
  const token = _AccountService.CurrentUser()?.token;

  if(token){
    //req = req.clone({headers: new HttpHeaders({Authorization: `Bearer ${token}` }) })
    req = req.clone({setHeaders: {Authorization: `Bearer ${token}`}})
   }
   
   return next(req);
};
