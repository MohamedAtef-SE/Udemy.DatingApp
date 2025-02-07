import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const _AccountService = inject(AccountService);
  const _ToastrService = inject(ToastrService);

  if(_AccountService.Roles().includes("Admin") || _AccountService.Roles().includes("Moderator")){
    return true;
  }
  else{
    _ToastrService.error("Admins Only have access");
    return false;
  }
};
