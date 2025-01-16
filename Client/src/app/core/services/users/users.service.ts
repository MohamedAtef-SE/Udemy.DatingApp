import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environments } from '../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private _HttpClient:HttpClient) { }

  getAllUsers():Observable<any>{
    return this._HttpClient.get(`${environments.baseURL}/api/users`);
  }

  getUser(id:number):Observable<any>{
    return this._HttpClient.get(`${environments.baseURL}/api/users/${id}`);
  }
}
