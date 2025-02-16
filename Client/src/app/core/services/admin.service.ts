import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private _HttpClient:HttpClient) { }


  getUsersWithRoles():Observable<any>{
    return this._HttpClient.get(`${environment.baseURL}/admin/users-with-roles`)
  }

  updateUserRoles(username:string,roles:string[]):Observable<any>{
    return this._HttpClient.post<string[]>(`${environment.baseURL}/admin/edit/roles/${username}?roles=${roles}`,{});
  }
}
