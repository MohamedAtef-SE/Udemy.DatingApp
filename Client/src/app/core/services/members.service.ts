import { HttpClient } from '@angular/common/http';
import { effect, inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, of, Subscription, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMember, IMemberUpdateForm } from '../Interfaces/IMember';
import { IPhoto } from '../Interfaces/IPhoto';
import { AccountService } from './account.service';


@Injectable({
  providedIn: 'root'
})

export class MembersService {

  constructor(private _HttpClient:HttpClient) {
    
  }

 

  _AccountService = inject(AccountService);
  Members : WritableSignal<IMember[]> = signal<IMember[]>([]);
  Member: WritableSignal<IMember> = signal({} as IMember);

  getAllMembers():Subscription{
    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/api/users`)
    .subscribe({
      next: (res:IMember[])=> this.Members.set(res)
      })
  }

  getMember(username:string):Observable<any>{
    const member:IMember | undefined = this.Members().find(m => m.userName === username)
    if(member !== undefined && member.photoURL == this._AccountService.CurrentUser()?.photoURL) return of(member);
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

  setMemberPhoto(photo:IPhoto):Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/api/users/set-main-photo/${photo.id}`,{})
    .pipe(
      tap(()=>{
        this.Members.update(members => members.map(m => {
         
          if(m.userName == this.Member().userName){
            console.log("found photo in this user")
            m.photoURL = photo.url
            m.photos.map(p => {
              if(p.url == photo.url){
                p.isMain = true;
              }
              else{
                p.isMain = false;
              }
              return p
            })
          }
          return m;
        }))

        this.Member.update(prev => {
          if(prev == null) return prev;
          const updatedMember : IMember = JSON.parse(JSON.stringify(prev));
          const photos = updatedMember.photos.map(p => {
            if(p.isMain == true) p.isMain = false;
            if(p.url == photo.url) p.isMain = true;
            return p;
          })

          return {
            ...updatedMember,
            photos: photos,
            photoURL: photo.url
          }
        })
      })
    )
  }

  deleteMemberPhoto(photo:IPhoto):Observable<any>{
    return this._HttpClient.delete(`${environment.baseURL}/api/users/delete-photo/${photo.id}`)
    .pipe(
      tap(()=>{

        this.Member.update(prev => {
          if(prev == null) return prev;
          const updatedMember : IMember = JSON.parse(JSON.stringify(prev));

          // set default photo if profile picture was deleted
          if(updatedMember.photoURL == photo.url){
            updatedMember.photoURL = "./assets/images/default.webp"
          }
          return {
            ...updatedMember,
            photos: updatedMember.photos.filter(p => p.id !== photo.id)
          }
        })

        if(this._AccountService.CurrentUser()?.photoURL == photo.url){
           // update-CurrentUser()
           this._AccountService.CurrentUser.update(prev =>{
            if(prev == null) return prev;
            return{
            ...prev,
            photoURL: "./assets/images/default.webp"
            }
          });
          const userJSON = JSON.stringify(this._AccountService.CurrentUser());
          localStorage.setItem("DateAppUserToken",userJSON)
        }

        // update all members list in case main profile pic deleted
        this.Members.update(members => members.map(m => {
          if(m.photoURL == photo.url && photo.isMain){
            m.photoURL = "../../../assets/images/default.webp"
          }

          return m;
        }))
      })
    )
  }

  loadMember = ()=>{
    console.log("loading...")
    const user = this._AccountService.CurrentUser();
    console.log("USERNAME: ",user?.username)
    if(!user) return;
    this.getMember(user.username).subscribe({
      next:(res:IMember)=>{
        this.Member.set(res);
        console.log("User loaded...")
      }
    })
  }

}