import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ILoginForm, ICurrentUser, IRegisterForm } from '../Models/Models';
import { environment } from '../../../environments/environment';
import { MembersService } from './members.service';
import { IMember } from '../Models/IMember';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  CurrentUser: WritableSignal<ICurrentUser | null> = signal(null);

  constructor(private _HttpClient:HttpClient) { }

  login(data:ILoginForm):Observable<any>{
     return this._HttpClient.post<ICurrentUser>(`${environment.baseURL}/api/account/login`,data).pipe(
      map(user => {
        if(user){
          localStorage.setItem('DateAppUserToken',JSON.stringify(user))
          this.CurrentUser.set(user);
        }
        return user;
      })
     )
  }

  register(data:IRegisterForm):Observable<any>{
    return this._HttpClient.post<ICurrentUser>(`${environment.baseURL}/api/account/register`,data).pipe(
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
    localStorage.removeItem("DateAppUserToken");
    this.CurrentUser.set(null);
  }
}