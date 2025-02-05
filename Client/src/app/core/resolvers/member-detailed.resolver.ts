import { ResolveFn } from '@angular/router';
import { IMember } from '../Models/IMember';
import { inject } from '@angular/core';
import { MembersService } from '../services/members.service';

export const memberDetailedResolver: ResolveFn<IMember | null> = (route, state) => {

  const _MemberService = inject(MembersService);
  var username = route.params['username']
  if(username == undefined) return null;
  return _MemberService.getMember(username);
};
