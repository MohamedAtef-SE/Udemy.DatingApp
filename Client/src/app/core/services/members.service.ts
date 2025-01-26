import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, of, Subscription, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountService } from './account.service';
import { IMember, IMemberUpdateForm } from '../Interfaces/IMember';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  constructor(private _HttpClient:HttpClient) { }

  _AccountService = inject(AccountService);
  Members: WritableSignal<IMember[]> = signal([]);

  // getHttpOptions(){
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: `Bearer ${this._AccountService.CurrentUser()?.token}`
  //     })
  //   }
  // }
  
  getAllMembers():Subscription{
    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/api/users`).subscribe({
      next: (res:IMember[])=> this.Members.set(res)
      })
    }

  getMember(username:string):Observable<any>{
    const member:IMember | undefined = this.Members().find(m => m.userName === username)
    if(member !== undefined) return of(member);
    return this._HttpClient.get<IMember>(`${environment.baseURL}/api/users/${username}`);
  }
  
  updateMember(data:IMemberUpdateForm): Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/api/Users`,data).pipe(
      tap(()=> {
        const username = this._AccountService.CurrentUser()?.username;
        if(username !== undefined){
          const member : IMember | undefined = this.Members().find(m => m.userName === username);
          if(member !== undefined){
            member.introduction = data.Introduction;
            member.lookingFor = data.LookingFor;
            member.interests = data.Interests;
            member.city = data.City;
            member.country = data.Country;
            this.Members.update( prevMembers =>
               prevMembers.map(m => m.userName == member.userName ? member : m)
               );
          }}}
        ))
      }
    }