import { HttpClient } from '@angular/common/http';
import { Injectable, signal, WritableSignal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ILoginForm, ILoginResponse, IRegisterForm, IRegisterResponse } from '../Interfaces/Models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  CurrentUser: WritableSignal<ILoginResponse | null> = signal(null);

  constructor(private _HttpClient:HttpClient) { }

  login(data:ILoginForm):Observable<any>{
     return this._HttpClient.post<ILoginResponse>(`${environment.baseURL}/api/account/login`,data).pipe(
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
    return this._HttpClient.post<IRegisterResponse>(`${environment.baseURL}/api/account/register`,data).pipe(
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