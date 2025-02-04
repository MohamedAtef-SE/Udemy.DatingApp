import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ICurrentUser, ILoginForm, IRegisterForm } from '../Models/Models';
import { LikesService } from './likes.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  CurrentUser: WritableSignal<ICurrentUser | null> = signal(null);

  constructor(private _HttpClient:HttpClient) { }

  _LikesService = inject(LikesService);

  login(data:ILoginForm):Observable<any>{
     return this._HttpClient.post<ICurrentUser>(`${environment.baseURL}/api/account/login`,data).pipe(
      map(user => {
        if(user){
          localStorage.setItem('DateAppUserToken',JSON.stringify(user))
          this.CurrentUser.set(user);
          this._LikesService.getLikeIds();
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