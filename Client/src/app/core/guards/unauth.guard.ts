import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';

export const unauthGuard: CanActivateFn = (route, state) => {
  const _AccountService = inject(AccountService);
  const _Router = inject(Router);
  if(_AccountService.CurrentUser() == null) return true;
  else{
    _Router.navigateByUrl('/members');
    return false;
  }
  
};
