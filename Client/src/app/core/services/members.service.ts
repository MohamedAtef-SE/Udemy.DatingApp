import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, of, Subscription, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMember, IMemberUpdateForm } from '../Models/IMember';
import { IPhoto } from '../Models/IPhoto';
import { UserParams } from '../Models/userParams';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeader } from './paginationHelper';
import { PaginationService } from './pagination.service';


@Injectable({
  providedIn: 'root'
})

export class MembersService {

  constructor(private _HttpClient:HttpClient) {
    
    // don't invoke this method in service because its depend on
    // CurrentUser() who initiate on AppComponent and Services
    // in Angular built before components.
    //this.loadMember();
  }

 

  _AccountService = inject(AccountService);
  _PaginatationService = inject(PaginationService);
  Member: WritableSignal<IMember> = signal({} as IMember);
  pickedUserparams:WritableSignal<UserParams | null> = signal(null)
  memberCache = new Map();

  getAllMembers(userParams:UserParams): Subscription | void{
    
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) return setPaginatedResponse(response,this._PaginatationService.paginatedResult);
 
    let params = setPaginationHeader(userParams);
    params = params.append('gender',userParams.gender);
    params = params.append('maxAge',userParams.maxAge);
    params = params.append('minAge',userParams.minAge);
    params = params.append('orderby',userParams.orderBy);

    if(userParams.city !== undefined)
       params = params.append('city',userParams.city)

    if(userParams.country !== undefined)
      params = params.append('city',userParams.country)

    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/users`,{observe:'response',params})
    .subscribe({
      next: (response:HttpResponse<IMember[]>)=> {
       setPaginatedResponse<IMember>(response,this._PaginatationService.paginatedResult);
       this.memberCache.set(Object.values(userParams).join('-'),response);
      }
      })
  }
  setPaginationHeader(userParams: UserParams) {
    throw new Error('Method not implemented.');
  }


  getMember(username:string):Observable<any>{
    const member:IMember | undefined = this.findCachedMember(username)
    if(member) return of(member);

    return this._HttpClient.get<IMember>(`${environment.baseURL}/users/${username}`);
  }

  findCachedMember(username: string):IMember | undefined {
    const member:IMember | undefined = [...this.memberCache.values()]
    .reduce((cachedMembers,response)=> cachedMembers.concat(response.body),[])
    .find((m:IMember) => m.userName == username);
    return member;
  }
  
  updateMember(data:IMemberUpdateForm): Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/Users`,data).pipe(
      // tap(()=> {
      //   const username = this._AccountService.CurrentUser()?.username;
      //   if(username !== undefined){
      //     const member : IMember | undefined = this.Members().find(m => m.userName === username);
      //     if(member !== undefined){
      //       member.introduction = data.Introduction;
      //       member.lookingFor = data.LookingFor;
      //       member.interests = data.Interests;
      //       member.city = data.City;
      //       member.country = data.Country;
      //       this.Members.update( prevMembers =>
      //          prevMembers.map(m => m.userName == member.userName ? member : m)
      //          );
      //     }}}
      //   )
      )
  }

  setMemberPhoto(photo:IPhoto):Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/users/set-main-photo/${photo.id}`,{})
    .pipe(
       tap(()=>{
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
    return this._HttpClient.delete(`${environment.baseURL}/users/delete-photo/${photo.id}`)
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
          localStorage.setItem("HommiesUser",userJSON)
        }

      })
    )
  }

  loadMember = ()=>{
    console.log("loading member...")
    const user = this._AccountService.CurrentUser();
    if(!user) return;
    this.getMember(user.username).subscribe({
      next:(res:IMember)=>{
        this.Member.set(res);
        console.log("User loaded...")
      }
    })
  }
}