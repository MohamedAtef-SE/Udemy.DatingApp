import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const _AccountService = inject(AccountService);
  const _ToastrService = inject(ToastrService);
  const _Router = inject(Router);

  if(_AccountService.UserRoles().includes("Admin")){
    return true;
  }
  else{
    _ToastrService.error("Admins Only have access 😏");
    _Router.navigateByUrl('/members')
    return false;
  }
};
