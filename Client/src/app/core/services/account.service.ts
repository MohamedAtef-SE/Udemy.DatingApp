import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IUser, ILoginForm, IRegisterForm } from '../Models/Models';
import { LikesService } from './likes.service';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private _HttpClient:HttpClient) { }

  CurrentUser: WritableSignal<IUser | null> = signal(null);
  UserRoles:Signal<string[]> = computed(()=>{
    const user = this.CurrentUser();
    if(user && user.token){
      const role = JSON.parse(atob(user.token.split('.')[1])).role; // PAYLOAD.role
      return Array.isArray(role) ? role : [role]
    }
    return [];
  })

  _LikesService = inject(LikesService);
  _PresenceService = inject(PresenceService);

  login(data:ILoginForm):Observable<IUser>{
     return this._HttpClient.post<IUser>(`${environment.baseURL}/account/login`,data).pipe(
      map(user => {
        if(user){
          localStorage.setItem('DateAppUser',JSON.stringify(user))
          this._PresenceService.createHubConnection(user);
          this.CurrentUser.set(user);
          this._LikesService.getLikeIds();
        }
        return user;
      })
     )
  }

  register(data:IRegisterForm):Observable<any>{
    return this._HttpClient.post<IUser>(`${environment.baseURL}/account/register`,data).pipe(
      map((user) => {
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.CurrentUser.set(user);
        }
        return user
      })
    )
  }

  logout():void{
    localStorage.removeItem("DateAppUser");
    this._PresenceService.stopHUbConnection();
    this.CurrentUser.set(null);
  }
}