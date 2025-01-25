import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IMember } from '../Interfaces/IMember';
import { AccountService } from './account.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  constructor(private _HttpClient:HttpClient) { }

  _AccountService = inject(AccountService);

  // getHttpOptions(){
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: `Bearer ${this._AccountService.CurrentUser()?.token}`
  //     })
  //   }
  // }
  
  getAllMembers():Observable<any>{
    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/api/users`);
  }

  getMember(username:string):Observable<any>{
    return this._HttpClient.get<IMember>(`${environment.baseURL}/api/users/${username}`);
  }
}
