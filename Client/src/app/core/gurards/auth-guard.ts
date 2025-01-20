import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {

  const _AccountService = inject(AccountService);
  const _ToastrService = inject(ToastrService);
  const _Router = inject(Router);

  if(_AccountService.CurrentUser()){
    return true;
  }

  _ToastrService.error('You should Login first 😏')
  _Router.navigateByUrl('');
  return false;
};
